# Phase 3 – Travel System + MapLevel Step Redesign + MapLevel Editor

**Date:** 2026-05-09  
**Branch:** develop

## What was done

### ScrObj naming unification
Renamed all `*DataObject` ScriptableObject wrappers to `*ScrObj` for consistency:
- `LanguageDataObject` → `LanguageScrObj` (moved to `Scripts/Data/`)
- `ItemDataObject` → `ItemScrObj`
- `PlayerDataObject` → `PlayerScrObj`
- `EnemyDataObject` → replaced entirely by new `EnemyScrObj`
- `IconsDataObjet` deleted (empty MonoBehaviour stub, unused)

Updated all references: `Character.cs`, `Enemy.cs`.

### EnemyData + EnemyScrObj
- **`Scripts/Data/Combat/EnemyData.cs`** — standalone `[Serializable]` enemy config (no Character inheritance). Fields: EnemyId, name, portrait, HP, attack, defense, energy, actionCosts, actionProbabilities (must sum 100), mentalityMod, goldMin/Max, languages.
- **`Scripts/Data/Combat/EnemyScrObj.cs`** — ScriptableObject wrapper. Menu: `RPSRPG/Combat/Enemy`.
- **8 enemy assets** filled with game-design data in `ScriptableObjects/Enemies/`:
  - Enemy_00 Goblin, Enemy_01 Orc Brute, Enemy_02 Elf Scout, Enemy_03 Stone Guard,
  - Enemy_04 Shadow Mage, Enemy_05 Berserker, Enemy_06 Assassin, Enemy_07 Dragon Knight

### MapLevel step-based dungeon redesign
Replaced single-enemy MapLevel with a step-sequence model:

- **`RPSEnums.cs`** — added `MapStepType` enum: `Combat`, `Boss`, `Treasure`, `NpcRescue`; extended `EnemyId` to `ENEMY_0..7`.
- **`Scripts/Data/Town/Travel/MapStep.cs`** — `[Serializable]` union struct: `_type` + one ref per payload type.
- **`Scripts/Data/Combat/CombatStepScrObj.cs`** — holds `EnemyScrObj _enemy`.
- **`Scripts/Data/Town/Travel/TreasureStepScrObj.cs`** — holds `int _goldAmount`.
- **`Scripts/Data/Town/Travel/NpcRescueScrObj.cs`** — holds `TownMenu _targetBuilding`.
- **`Scripts/Data/Town/Travel/MapLevel.cs`** — removed `_enemy`, `_steps (int)`, `_reward`, `_isSpecialLevel`; added `List<MapStep> _steps`, `StepCount` property.

Old `ScriptableObjects/Town/MapLevels.asset` (single-enemy format) deleted. New `ScriptableObjects/Levels/MapLevels.asset` created (step-based).

### TravelManager implementation
- **`TravelManager.cs`** — implemented `TravelToLevel()`: credits gate → `UseCredit()` → set `AppContext.CombatContext` → `LoadScene(COMBAT)`.
- **`AppContext.cs`** — added `static CombatContext CombatContext { get; set; }`.
- **`Scripts/Data/Context/CombatContext/CombatContext.cs`** — transient class: `MapLevel SelectedLevel`.
- **`CreditsTimeCounterManager.cs`** — exposed `public int CreditsLeft` getter.
- **`TravelUIController.cs`** — `levelButton.interactable = level.IsAvailable`.

### MapLevel Editor Window
**`Scripts/Util/Editor/MapLevelEditorWindow.cs`** — 3-panel custom EditorWindow (menu: `Kapibara/Map Level Editor`):
- **LEVELS** (left 210px): scrollable list with availability badge and step count; +/- create/delete; fields for number, name, available, icon, portrait.
- **STEPS** (middle 190px): per-level step list with type tags [C][B][T][N]; +/-/↑/↓ add/remove/reorder.
- **STEP DETAIL** (right): type picker + conditional ScrObj field (CombatData, TreasureData, NpcRescueData).

All state via `SerializedObject`/`SerializedProperty` — no direct asset mutation outside Unity's undo stack.

## Files changed

| File | Change |
|------|--------|
| `Scripts/Data/Combat/EnemyData.cs` | NEW standalone enemy config |
| `Scripts/Data/Combat/EnemyScrObj.cs` | NEW SO wrapper |
| `Scripts/Data/Combat/CombatStepScrObj.cs` | NEW step asset |
| `Scripts/Data/Town/Travel/MapStep.cs` | NEW union step class |
| `Scripts/Data/Town/Travel/TreasureStepScrObj.cs` | NEW step asset |
| `Scripts/Data/Town/Travel/NpcRescueScrObj.cs` | NEW step asset |
| `Scripts/Data/Town/Travel/MapLevel.cs` | MODIFIED — step list replaces single enemy |
| `Scripts/Data/Context/CombatContext/CombatContext.cs` | NEW transient context |
| `Scripts/Data/Context/AppContext/AppContext.cs` | MODIFIED — added CombatContext property |
| `Scripts/Data/LanguageScrObj.cs` | NEW (moved from _oldScriptables) |
| `Scripts/Data/Character.cs` | MODIFIED — LanguageScrObj ref |
| `Scripts/Data/Enemy.cs` | MODIFIED — .Data accessor |
| `Scripts/Managers/02_Town/TravelManager.cs` | MODIFIED — implemented TravelToLevel() |
| `Scripts/Managers/02_Town/CreditsTimeCounterManager.cs` | MODIFIED — CreditsLeft getter |
| `Scripts/UIControllers/02_Town/Travel/TravelUIController.cs` | MODIFIED — interactable flag |
| `Scripts/Util/RPSEnums.cs` | MODIFIED — MapStepType + EnemyId extension |
| `Scripts/Util/Editor/MapLevelEditorWindow.cs` | NEW editor window |
| `ScriptableObjects/Enemies/Enemy_00..07.asset` | NEW + filled with design data |
| `ScriptableObjects/Levels/MapLevels.asset` | NEW (step-based format) |
| `_oldScriptables/EnemyDataObject.cs` | DELETED |
| `_oldScriptables/IconsDataObjet.cs` | DELETED |
| `_oldScriptables/LanguageDataObject.cs` | DELETED |
| `ScriptableObjects/Player/Enemy.asset` | DELETED (old location) |
| `ScriptableObjects/Town/MapLevels.asset` | DELETED (old format) |

## Gotchas / follow-up

- **Unity Editor wiring needed**: Open `Kapibara/Map Level Editor`, assign the new `MapLevels.asset`, and populate each level's steps using the CombatStep/TreasureStep/NpcRescue assets.
- **Enemy portraits**: Sprites not yet assigned in the 8 EnemyScrObj assets.
- **MapLevels.asset**: Step refs are empty — needs manual wiring in editor.
- **Phase 4 (Combat Core)** is next: Combat scene, CombatManager, action loop, damage matrix, reward.
