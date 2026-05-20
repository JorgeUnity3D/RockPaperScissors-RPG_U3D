# Combat Design Clarifications — from PDF review 2026-05-20

Pending implementation changes derived from re-reading the design PDF.
Each point documents the gap vs. current code.

---

## 1. Player action selection — 2-step confirmation

**Design:**
- First tap on an action button → shows the player's thought bubble with the action icon (common language). No round resolved yet.
- Second tap on the **same** button → confirms. Thought bubble hides, action bubble appears, round resolves.
- Tapping a **different** button on the second tap → changes the pending selection (thought bubble updates).

**Current code:**
- Each button fires `AppEvents.OnCombatActionSelected` immediately on first tap. One-step, no confirmation.
- `CombatUIController` has no pending-action state.

**What needs to change:**
- `CombatUIController`: add `_pendingAction: Actions` internal state.
  - First tap: set `_pendingAction`, show `_playerThoughtBubble` with common-language icon, do NOT fire the event.
  - Second tap on same action: fire `AppEvents.OnCombatActionSelected`, hide thought bubble.
  - Second tap on different action: update `_pendingAction` and thought bubble only.
- `CombatManager`: no changes needed — it still receives the confirmed action via `OnCombatActionSelected`.

---

## 2. Enemy thought bubble — always visible, icon depends on mentality roll

**Design:**
- At the start of each round, during the mentality phase, the enemy thought bubble is shown.
- If player **wins** the mentality roll → bubble shows the enemy's actual thinking action icon (in the enemy's current language).
- If player **loses** the mentality roll → bubble shows a **question mark** icon instead.
- The bubble hides once the round resolves (together with all other bubbles via `HideBubbles()`).

**Current code:**
- Win: `ShowPlayerThoughtBubble(icon)` — shows a bubble but on the player side; should be enemy side.
- Lose: `ShowEnemyThoughtBubble(null)` — shows the bubble but with no icon (null → empty).

**What needs to change:**
- Both win and lose should call `ShowEnemyThoughtBubble(...)`.
  - Win: pass the language icon for `_enemy.ThinkingAction`.
  - Lose: pass a `_questionMarkSprite` (new `[SerializeField]` field in `CombatUIController`).
- `ShowEnemyThoughtBubble(Sprite icon)`: remove null-check workaround; caller always passes a valid sprite.
- Confirm in-editor which side each bubble GameObject sits on.

---

## 3. Action bubbles always use common language

**Design:**
- When actions are revealed (after confirmation), the action bubbles on both sides always show the **common language** icons, regardless of the enemy's current language.
- Enemy language symbols only appear in the **thought bubble** (when mentality roll succeeds).

**Current code:**
- `CombatManager.GetActionIcon(action)` returns `_enemy.CurrentLanguage.GetActionIcon(action)`.
- Both player and enemy action bubbles call `GetActionIcon()` — so action bubbles currently use the enemy's language, not common.

**What needs to change:**
- Add a `_commonLanguage: Language` reference to `CombatManager` or `CombatUIController`.
  - Source: a dedicated `LanguageScrObj` ScriptableObject with `language = Languages.COMMON` (user needs to create this asset in Editor).
  - `GetActionIcon()` for **action bubbles** → always reads from `_commonLanguage`.
  - `GetActionIcon()` for **thought bubbles** → reads from `_enemy.CurrentLanguage` (existing behavior).
- Until the Common language asset exists: leave a `[SerializeField]` field wired in Inspector, log a warning if null.

---

## 4. Action bubbles inflate proportionally — bigger wins, smaller is destroyed

**Design:**
- When two attacking actions clash, each bubble inflates to a size proportional to that combatant's effective power (stat + roll + multiplier).
- The larger bubble "wins": it hits the smaller one, which gets destroyed (pop animation).
- If both are equal → both destroyed (tie).

**Current code:**
- `ShowPlayerActionBubble(icon, debugDamage)` and `ShowEnemyActionBubble(icon, debugDamage)` just `SetActive(true)` on a GameObject. No size logic.
- Damage difference is already computed in `ResolveRound` but only logged.

**What needs to change:**
- `CombatUIController`: expose a method that takes both effective values and animates:
  ```csharp
  public void ShowActionClash(Sprite playerIcon, int playerEffective,
                               Sprite enemyIcon,  int enemyEffective)
  ```
  - Scale each bubble relative to the max of the two effective values.
  - Animate: both inflate → larger hits smaller → smaller pops → larger deflates.
  - Tie: both inflate equally → both pop simultaneously.
- `CombatManager.ResolveRound()`: replace the two separate `ShowPlayerActionBubble` / `ShowEnemyActionBubble` calls with one `ShowActionClash(...)` call, passing `playerEffective` and `enemyEffective`.
- Note: `playerEffective` and `enemyEffective` are already computed in `ResolveRound` — no new logic needed, just pass them through.

---

## 5. Damage resolution — verify against PDF

**Design (from PDF):**
- Attack vs Attack: difference-based. Winner deals `|effective_A - effective_B|` to loser.
- Attack vs Defense: attacker deals damage only if `attacker_effective > defender_effective`. Damage = difference.
- Defense vs Defense: no damage.
- Energy vs anything: Energy side deals no damage. If attacked while using Energy, attacker resolves normally against 0 defense.
- Thorns: fire **only when enemy attacks and player defends** (Case 3b). Formula: `(multiplier × thorns) + thorns + variabilityRoll`. Crit bonus: `+thorns × 0.5`.
- Thorns do NOT fire when player attacks and enemy defends.

**Current code:**
- Case 1 (attack vs attack / attack vs energy): ✅ difference-based
- Case 2 (both passive): ✅ no damage
- Case 3a (player attacks, enemy defends): ✅ attacker wins if effective > shield
- Case 3b (enemy attacks, player defends): ✅ thorns fire before damage check; damage = difference if enemy breaks shield
- Thorns formula in `CombatResolver.ThornsRoll`: `Mathf.FloorToInt(multiplier × thorns) + thorns + variabilityRoll`. Crit: `+thorns × 0.5`. ✅ matches PDF.

**Gap:** Energy vs Attack currently falls into Case 1 (attack vs attack path). The Energy user has `playerEffective = 0` (since `DamageRoll` returns 0 for ENERGY). The attacker then deals `|attacker_effective - 0| = attacker_effective` damage. This is correct per the design (Energy side has 0 effective). No change needed.

**Action:** Verify once in-editor that the Thorns formula produces reasonable numbers. No code change needed based on current read.

---

## Implementation order (suggested)

1. **Point 3 first** (common language reference) — needed by points 1 and 4 since action bubbles need the correct icons.
2. **Point 1** (2-step player confirmation) — pure UI state change, isolated.
3. **Point 2** (enemy thought bubble always visible) — small UI fix, needs question mark sprite from user.
4. **Point 4** (bubble inflation animation) — requires DOTween; depends on point 3 for correct icons.
5. **Point 5** — no code change needed per current read; confirm in-editor.
