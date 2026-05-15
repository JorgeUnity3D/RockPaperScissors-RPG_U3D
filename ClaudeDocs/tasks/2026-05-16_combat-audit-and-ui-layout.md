# 2026-05-16 — Combat audit, fixes & UI layout

## What was done

### Full codebase audit (5 parallel agents)
Reviewed scene names, combat logic, AppEvents wiring, credits system, and UI layer before first combat test.

### Fixes applied

| File | Fix |
|------|-----|
| `GameConsts.cs` | Added `#region COMBAT` with `COMBAT_MAX_ROUNDS`, `COMBAT_MAX_ENERGY`, `COMBAT_ROUND_DELAY`. Removed stale `MAP_SCENE` constant and `SceneEnums` entry that caused compile error. Changed `COMBAT_SCENE` to `"03_Combat"` (was `"04_Combat"`). |
| `CombatManager.cs` | Removed private const duplicates; used `GameConsts.*` throughout. Moved super-attack check BEFORE energy payment. Added `_enemy.LanguageRoll()` in `Initialize()`. Added null guard for `CombatContext`. |
| `CombatContext.cs` | Removed `Player` parameter from constructor — was triggering 2 NInt disk writes on every combat start. |
| `TravelManager.cs` | Updated `CombatContext` construction call (removed `_player` arg). |
| `GameManager.cs` | Moved `AppEvents.OnGameContextUpdated += UpdateSaveGame` from `LoadSelectedGame()` to `Subscribe()` — was causing double-subscription. Added null guard. |
| `Enemy.cs` | `MaxEnergy = GameConsts.COMBAT_MAX_ENERGY` (was hardcoded `100`). |
| `CombatResolver.cs` | `ThornsRoll`: removed `- 10` bias that produced negative values at low levels. |
| `CreditsTimeCounterUIController.cs` | Added `_timeCounterHolder.SetActive(!_creditTimeCounter.CreditsAtMax)` — holder was never toggled. |
| `EditorBuildSettings.asset` | Added `03_Combat.unity` scene registration. |

### Action icons
`CombatManager.GetActionIcon()` now delegates to `_enemy.CurrentLanguage.GetActionIcon(action)` — icons come from the enemy's Language system (LanguageAtlas), not from individual sprite fields or a separate ScriptableObject.

### Combat UI layout (Combat_UIController.prefab)
8 RectTransform edits for a 2-zone layout:

1. **PlayerZone** `anchorMax.x`: 0.333 → 0.5 (expands to half screen)
2. **EnemyZone** `anchorMin.x`: 0.666 → 0.5 (starts from half screen)
3. **CenterZone** collapsed to 0-width at x=0.5 (anchor min/max both x=0.5)
4. **Settings_Button** anchor.y → 1.0 (sits on arena bar boundary), `sizeDelta` 120×120 → 60×60
5. **PlayerSprite** `anchorMin {0.1,0}`, `anchorMax {0.9,0.52}` (smaller, bottom 52%)
6. **EnemySprite** same as PlayerSprite
7. **EnemyAction_Bubble** `localScale.x = -1` (X-mirrored)
8. **EnemyThought_Bubble** `localScale.x = -1` (X-mirrored)

## Commits
- `f39eec3` — Phase 4 combat core MVP + audit fixes
- `4805b51` — CombatManager: action icons from enemy language + UI prefab tweaks
- (this session) — Combat UI layout: 2-zone, smaller sprites, bubble inversion, settings button

## Gotchas / follow-up
- Settings_Button stays in CenterZone (collapsed to 0-width) — avoids reparenting complexity in YAML. Works because anchorY=1 puts it on the arena boundary.
- Bar heights were not resized in this pass — only zone widths and sprite anchors changed.
- CenterZone is intentionally 0-width; it exists only as a logical anchor point for Settings_Button.
