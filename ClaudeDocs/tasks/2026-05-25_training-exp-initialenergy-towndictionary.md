# Training EXP conditions, InitialEnergy refactor, TownDictionary consolidation

**Date:** 2026-05-25

## What was done

### Training EXP for all stats (CombatStepManager)

Extended `AccumulateTrainingExp` with full per-stat conditions:

| Stat | Condition |
|------|-----------|
| HEALTH | Player survived the round (HP > 0) |
| INITIAL_ENERGY | Player survived the round (HP > 0) |
| MENTALITY | Player won the mentality roll |
| DEFENSE | Shield held (damage was blocked) |
| THORNS | Thorns dealt > 0 damage |
| ENERGY_RECOVERY | Player used the ENERGY action |
| CRIT | critBonus > 0 |
| SUPERPOWER | playerIsSuper == true |
| ROCK/PAPER/SCISSOR | Existing multiplier logic |

Added `_playerWonMentalityRoll` field — set in `EnemyThink()`, read in `AccumulateTrainingExp()`. Locals `shieldHeld` and `thornsDealt` added to `ResolveRound()` and threaded into the accumulator call.

### InitialEnergy: NInt → NAttribute

- Added `Stats.INITIAL_ENERGY = 7` to `GameEnums.cs`
- Changed `Player._initialEnergy` from `NInt` to `NAttribute` (base value 25)
- `Player.InitialEnergy` property now returns `StatAttribute` instead of `int`
- Both constructors (new player + JSON deserialization) updated
- `_statAttributes` dictionary updated in both constructors
- `TravelManager` updated: `AppContext.Player.CurrentEnergy = Mathf.Min(GameConsts.COMBAT_MAX_ENERGY, AppContext.Player.InitialEnergy.TotalValue);`

### TownDictionary refactor

- Added `TownButtonEntry { Button, GameObject }` serializable class to `RPSDictionary.cs`
- `TownDictionary` changed from `TownMenu → Button` to `TownMenu → TownButtonEntry`
- Removed `_npcDictionary` from `TownUIController`
- `UpdateTownButton` now reads `entry.NpcObject` to toggle NPC visibility
- All 9 Button + NpcObject fileID pairs wired directly in `Town_UIController.prefab` YAML

### TownManager debug buttons

Added `#region DEBUG` with Odin Inspector buttons:
- `Buy Building (TownMenu)` — sets `IsUnlocked = true`, refreshes button, saves
- `Rescue NPC (TownMenu)` — sets `NpcUnlocked = true`, refreshes button, saves

## Files changed

- `Scripts/Managers/03_Combat/CombatStepManager.cs`
- `Scripts/ConstAndEnums/GameEnums.cs`
- `Scripts/Data/Context/Player/Player.cs`
- `Scripts/Managers/02_Town/TravelManager.cs`
- `Scripts/Data/Dictionaries/RPSDictionary.cs`
- `Scripts/UIControllers/02_Town/TownUIController.cs`
- `Scripts/Managers/02_Town/TownManager.cs`
- `Prefabs/UI/02_Town/Town_UIController.prefab`

## Gotchas / follow-up

- Save file backward-compatibility is broken (InitialEnergy type changed). Acceptable during development — old saves will fail to deserialize. Fresh saves work correctly.
- NPC GameObjects under buttons that have no NPC support (e.g. Arena) should remain inactive — `townView.HasNpc` gates the `SetActive` call so this is safe.
