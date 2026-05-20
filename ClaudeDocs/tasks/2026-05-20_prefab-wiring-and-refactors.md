# 2026-05-20 — Prefab Wiring + Refactors

## What was done

### Rename: CombatManager → CombatStepManager
- `CombatManager.cs` + `.meta` renamed to `CombatStepManager.cs` + `.meta`
- Class declaration updated; all log strings updated
- `StepManager.cs`: field type + `GetComponentInChildren` + error log updated
- `EnemyHUDUIController.cs`, `CombatContext.cs`, `CombatResolver.cs`: comments updated

### PlayerHUDUIController.cs
- Removed `_settingsButton` field and its `onClick` listener — belongs in `PauseMenuUIController` (deferred)
- `_pendingAction` 2-step confirmation already implemented (marked ✅ in roadmap)
- `SetCommonLanguage()` already implemented (marked ✅ in roadmap)

### Prefab reference assignment
All SerializeField refs assigned in YAML for:
- `TreasureHUD_UIController.prefab`: `_treasureImage`, `_goldText`
- `NPCHUD_UIController.prefab`: `_npcImage`, `_npcDialogueBubble`, `_npcDialogueText` (`_playerDialogueBubble`/`_playerDialogueText` left null — elements don't exist yet)
- `PlayerHUD_UIController.prefab`: all 26 refs — bars, sprite, bubbles, 3 action groups + all 10 buttons

### Ghost bars — PlayerHUD
- User created `Ghost` Image GOs inside `PlayerHP_Bar` and `PlayerEnergy_Bar`
- Both are Type Filled (`m_Type: 3`) → `DOFillAmount` in `AnimateFill` is correct, no code change needed
- `_playerHPGhost` → fileID `2446357393832416386`
- `_playerEnergyGhost` → fileID `7735148146643488613`
- Assigned in prefab YAML

### Ghost bars — EnemyHUD
- User created Ghost GOs and assigned refs in prefab directly
- `EnemyHUDUIController.cs` already uses `DOFillAmount` — no change needed

## Files changed
- `CombatStepManager.cs` (renamed from CombatManager.cs)
- `CombatStepManager.cs.meta` (renamed)
- `StepManager.cs`
- `EnemyHUDUIController.cs` (comment only)
- `CombatContext.cs` (comment only)
- `CombatResolver.cs` (comment only)
- `PlayerHUDUIController.cs`
- `TreasureHUD_UIController.prefab`
- `NPCHUD_UIController.prefab`
- `PlayerHUD_UIController.prefab`
- `EnemyHUD_UIController.prefab` (by user)
- `roadmap.md`

## Gotcha learned
Unity hides the Image Type dropdown in the Inspector when no sprite is assigned. Debug mode needed to see/change `m_Type` on sprite-less Images.
