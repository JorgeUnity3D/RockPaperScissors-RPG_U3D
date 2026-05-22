# Phase 5 — Session 3 (2026-05-21)

## What was done

### Phase5 Item 4: Enemy thought bubble on mentality roll
- `Language.GetActionIcon()`: added null guard — returns null + LogWarning if action not found in atlas (class ref, not struct)
- Enemy thought bubble logic confirmed: `Actions.NONE` maps to question mark sprite already registered under `None` in Language SOs

### Phase5 Item 10: Gold x2 on superpoder kill
- `CombatStepManager`: added `_playerSuperKill` bool; set when `playerIsSuper && enemy.CurrentHealth <= 0` (Cases 1 and 3a); `EndCombat()` doubles `goldEarned` if true

### Architecture: Dynamic step generation
- `MapLevel`: removed `_steps` / `StepCount`; added `_possibleEnemies`, `_boss`, `_treasureGoldAmount`, `_treasureSprite`, `_targetBuilding`, `_npcSprite`, `_npcDialogueLines`, runtime `_isCompleted` + `SetCompleted()`
- `MapStep`: added 4 constructors for runtime creation (COMBAT/BOSS, TREASURE, NPC_RESCUE, SURPRISE_BOX)
- `CombatContext`: `GenerateSteps()` builds 4×combat-or-treasure + SURPRISE_BOX + 4×combat-or-treasure + BOSS + optional NPC_RESCUE; `COMBAT_TREASURE_CHANCE = 0.25f` in GameConsts
- `RPSEnums`: added `MapStepType.SURPRISE_BOX`
- `StepManager`: SURPRISE_BOX stub — LogWarning + advance
- `GameManager`: `ctx.SelectedLevel.StepCount` → `ctx.StepCount`; boss kill → `SetCompleted()`
- `MapLevelEditorWindow`: 3-panel (Levels|Steps|Step Detail) → 2-panel (Levels|Level Detail)
- `MapLevels.asset`: migrated YAML — 7 biomes with enemies, bosses, treasure data; NPC data filled by user in Editor

### Town: NPC gate
- `TownUIController.UpdateTownButton()`: building interactable = `!IsUnlocked || !HasNpc || NpcUnlocked`

### Bug fixes
- `PlayerHUD _openBackpackButton`: YAML fileID pointed to LayoutElement component → corrected to Button component fileID
- `NPCStepManager.Initialize()`: added `_playerHUD.HidePlayerBubbles()` before dialogue starts

### ConsolePro filters
- Renamed `CombatManager` → `CombatStepManager` in Combat filter group
- Added `StepManager`, `NPCStepManager`, `TreasureStepManager` entries to Combat filter group

## Files changed
- `Scripts/Data/Language.cs`
- `Scripts/Managers/03_Combat/CombatStepManager.cs`
- `Scripts/Managers/03_Combat/NPCStepManager.cs`
- `Scripts/Managers/03_Combat/StepManager.cs`
- `Scripts/Managers/GameManager.cs`
- `Scripts/Data/Town/Travel/MapLevel.cs`
- `Scripts/Data/Town/Travel/MapStep.cs`
- `Scripts/Data/Context/CombatContext/CombatContext.cs`
- `Scripts/ConstAndEnums/GameConsts.cs`
- `Scripts/Util/RPSEnums.cs`
- `Scripts/Util/Editor/MapLevelEditorWindow.cs`
- `Scripts/UIControllers/02_Town/TownUIController.cs`
- `ScriptableObjects/Levels/MapLevels.asset`
- `Prefabs/UI/03_Combat/PlayerHUD_UIController.prefab`
- `_ThirdParty/ConsolePro/Settings/RPS-CustomFilters.cpf`
- `ClaudeDocs/agents/roadmap.md`

## Commits
- 0e8f19d, 4cf3e31, 69957e4, edc1648 pushed to develop

## Follow-up
- `MapLevels._npcDialogueLines`: all 7 levels empty — content TBD
- `_isCompleted` is runtime-only; persistence via GameContext deferred to Phase 6
- NPCHUD `_playerDialogueBubble`: user created GO and assigned in Editor
- `_stoneSmithyScrObj` still needs Inspector assignment on CombatStepManager GO
- Phase5 remaining: Item 7 (NPC Gambits), Item 8 (Caja Sorpresa gameplay), Item 9 (Round 10 Boss), Item 11 (PauseMenuUIController)

---

# Phase 5 — Session 3 (continued, 2026-05-22)

## What was done

### Bar animations fix
- `PlayerHUDUIController.AnimateFill`: `DOFillAmount` → `DOAnchorMax(new Vector2(target, 1f), dur).SetLink(fill.gameObject)` + ghost bar
- `EnemyHUDUIController.AnimateFill`: → `DOAnchorMin(new Vector2(1f - target, 0f), dur).SetLink(fill.gameObject)` + ghost bar
- Root cause: images have no sprite, so Unity falls back to `Graphic.OnPopulateMesh`, ignoring `fillAmount`
- `SetData()` on both HUDs: `fillAmount = 1f` → `rectTransform.anchorMax/Min = Vector2.one/zero`

### HP continuity between steps
- `CombatStepManager.Initialize()`: `_playerHP = _player.MaxHealth.TotalValue` → `_player.CurrentHealth`

### Step flow without scene reload
- `GameManager.OnStepFinished()`: on win + more steps → calls `stepManager.Initialize()` directly, no `LoadScene(COMBAT)`

### CombatResult deferred to end of level
- `CombatContext`: added `TotalGoldEarned`, `TotalTrainingExp`, `AddGold()`, `AddTrainingExp()`
- `CombatStepManager.EndCombat()`: accumulates via `AppContext.CombatContext?.AddGold/AddTrainingExp()`, fires `OnStepFinished` (no result UI)
- `TreasureStepManager`: same — accumulates gold via `AddGold()`
- `GameManager.ShowLevelResult()`: grabs totals, nullifies CombatContext, shows `CombatResultUIController`

### Rename OnCombatFinished → OnStepFinished
- `AppEvents.cs`, `CombatResultUIController`, `NPCStepManager`, `TreasureStepManager`, `CombatStepManager`

### Colored combat logs
- `CombatStepManager.ColorAction()`: ROCK=#FF6666 PAPER=#66AAFF SCISSOR=#66FF88 DEFENSE=#FFDD44 ENERGY=#44DDFF

### Enemy thought bubble fix after boss kill
- `CombatStepManager.EndCombat()`: added `_playerHUD.HidePlayerBubbles()` + `_enemyHUD.HideBubbles()` before `HideCanvas()`

### Level background in PlayerHUD
- `PlayerHUDUIController`: added `_backgroundImage` + `SetBackground(Sprite)` method
- `StepManager.Initialize()`: calls `_playerHUD.SetBackground(ctx.SelectedLevel.LevelPortrait)`

### NPC dialogue lines
- All 7 `MapLevel` assets populated with 5 example dialogue lines each

### Caja Sorpresa — full implementation
- `SurpriseBoxStepManager` (NEW): `Initialize()` picks random HP/energy effect, applies to player, shows HUD; `OnCollected()` fires `OnStepFinished(true)`
- `SurpriseBoxHUDUIController` (NEW): shows effect icon + label ("+X HP" / "+X Energy")
- `GameConsts`: `SURPRISE_BOX_HEAL_PERCENT = 0.3f`, `SURPRISE_BOX_ENERGY_AMOUNT = 30`
- `AppEvents`: added `OnSurpriseBoxCollected`
- `PlayerHUDUIController`: added `_surpriseBoxActionsGroup` + `_surpriseBoxContinueButton`; `SetStep()` activates group on SURPRISE_BOX
- `StepManager`: dispatches `SURPRISE_BOX` to `_surpriseBoxManager.Initialize()`
- `StepManager`: added `[Button("Skip Step")]` debug method

### Bug fix: MentalityRoll always failing
- `Enemy.MentalityRollAgainst()`: was `Roll(Level) + StoredMentality - playerMentality` (always > 0 since Roll ≥ 1)
- Fix: dual-sided roll — `enemyRoll = Roll + StoredMentality; playerRoll = Roll + playerMentality; return enemyRoll - playerRoll`
- With equal mentality stats: ~50% chance. Player mentality training has real impact now.

### Bug investigation: Credits not spent
- Code is correct — `CreditsTimeCounterManager.OnTravelRequested` deducts credit and fires `OnTravelConfirmed`
- Root cause: ScriptableObject `_creditsLeft` persists between Editor Play Mode sessions. If it reaches 0, travel is blocked with "No credits left" warning.
- Fix for dev: in Editor, manually reset `CreditsTimeCounter.asset → _creditsLeft` to 5 after running out.

### Editor tasks pending (user will do in Editor)
- `PlayerHUD` prefab: add `SurpriseBox_Actions` GO with Continue button, wire `_surpriseBoxActionsGroup` + `_surpriseBoxContinueButton`
- `Combat_UIService` prefab: add GO with `SurpriseBoxHUDUIController`, wire `_effectIcon`, `_effectLabel`, `_healSprite`, `_energySprite`
- `StepManager` GO: add child GO with `SurpriseBoxStepManager` component

## Files changed
- `Scripts/Data/Enemy.cs`
- `Scripts/Data/Context/Player/Player.cs` (SetData HP continuity)
- `Scripts/Managers/GameManager.cs`
- `Scripts/Managers/03_Combat/CombatStepManager.cs`
- `Scripts/Managers/03_Combat/StepManager.cs`
- `Scripts/Managers/03_Combat/TreasureStepManager.cs`
- `Scripts/Managers/03_Combat/NPCStepManager.cs`
- `Scripts/Managers/03_Combat/SurpriseBoxStepManager.cs` (NEW)
- `Scripts/UIControllers/03_Combat/PlayerHUDUIController.cs`
- `Scripts/UIControllers/03_Combat/EnemyHUDUIController.cs`
- `Scripts/UIControllers/03_Combat/CombatResultUIController.cs`
- `Scripts/UIControllers/03_Combat/SurpriseBoxHUDUIController.cs` (NEW)
- `Scripts/AppEvents/AppEvents.cs`
- `Scripts/Data/Context/CombatContext/CombatContext.cs`
- `Scripts/ConstAndEnums/GameConsts.cs`
- `ScriptableObjects/Levels/MapLevels.asset`

## Phase 5 remaining
- Item 7: NPC Gambits
- Item 9: Round 10 Boss
- Item 11: PauseMenuUIController
- Level unlock on completion: deferred to Phase 6
