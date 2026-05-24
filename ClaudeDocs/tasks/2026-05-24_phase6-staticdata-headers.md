# 2026-05-24 — Phase 6: StaticDataService + NPCSprite removal + Header cleanup

## What was done

### 1. StaticDataService
Created `Scripts/Services/StaticData/StaticDataService.cs` — a new `ServiceSubscriber<StaticDataService>` that centralizes all ScriptableObjects in one place on GameCore (DontDestroyOnLoad). All 8 SOs previously scattered across individual managers are now exposed as read-only properties.

SOs consolidated:
- `TownViews` (TownViewScrObj)
- `LibraryQuests` (LibraryScrObj)
- `CreditsTimeCounter` (CreditsTimeCounterScrObj)
- `TheaterData` (TheaterScrObj)
- `StoneSmithyItems` (StoneSmithyScrObj)
- `PaperTreeSkillTrees` (PaperTreeScrObj)
- `MapLevels` (MapLevelScrObj)
- `CommonLanguage` (LanguageScrObj)

### 2. Managers migrated (9 files)
All SO `[SerializeField]` fields removed; replaced with `StaticDataService` lookups in `SetUp()`:
- TownManager, CreditsTimeCounterManager, LibraryManager, NPCStepManager
- TheaterManager, StoneSmithyManager, PaperTreeManager, TravelManager, CombatStepManager

### 3. NPCSprite removed from MapStep/MapLevel
`MapStep._npcSprite` duplicated `TownView._npcIcon`. Since `TargetBuilding` is available on the step, the sprite can be looked up at runtime.

Files changed:
- `MapStep.cs` — removed `_npcSprite`/`NPCSprite`, updated constructor signature
- `MapLevel.cs` — removed `_npcSprite`/`NpcSprite`
- `CombatContext.cs` — updated `new MapStep(...)` call (removed sprite arg)
- `NPCStepManager.cs` — now looks up sprite via `TownViewScrObj` in `Initialize()`
- `MapLevelEditorWindow.cs` — removed NPC Sprite field from editor panel

### 4. GameCore.prefab updated
Added `StaticDataService` child GO with all 8 SO references assigned in the prefab YAML.

### 5. Header cleanup across Managers and UIControllers
Added `[Header("DEBUG")]` above all `[SerializeField, ReadOnly]` auto-assigned fields in:
- IntroManager, MainMenuManager, HouseManager, ScissorBonfireManager, TownManager
- TrainingHouseManager, GameManager, CombatStepManager, StepManager
Removed orphaned stacked `[Header]` lines from 4 UIControllers.

## Files changed
- `Scripts/Services/StaticData/StaticDataService.cs` (NEW)
- `Scripts/Managers/02_Town/*` (9 files — SO migration)
- `Scripts/Managers/03_Combat/NPCStepManager.cs`
- `Scripts/Managers/03_Combat/CombatStepManager.cs`
- `Scripts/Managers/GameManager.cs`
- `Scripts/Managers/00_Intro/IntroManager.cs`
- `Scripts/Managers/01_MainMenu/MainMenuManager.cs`
- `Scripts/Data/Town/Travel/MapStep.cs`
- `Scripts/Data/Town/Travel/MapLevel.cs`
- `Scripts/Data/Context/CombatContext/CombatContext.cs`
- `Scripts/Util/Editor/MapLevelEditorWindow.cs`
- `Prefabs/Managers/GameCore.prefab`
- Multiple UIController files (header cleanup)

## Gotchas / follow-up
- None. StaticDataService follows the same `ServiceSubscriber<T>` pattern as other services.
- Adding a new SO = one field in StaticDataService + one assignment in GameCore prefab YAML.
