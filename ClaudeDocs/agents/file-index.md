# File Index

Root: `Assets/_RPS/`

---

## Top-Level Folders

| Folder | Purpose |
|---|---|
| `Prefabs/` | Unity prefabs (GameCore manager prefab + UI prefabs per scene) |
| `Scenes/` | Unity scene files (00_Intro, 01_MainMenu, 02_Town, etc.) |
| `ScriptableObjects/` | SO assets: player data, town view configs |
| `Scripts/` | All C# source — see below |

---

## Scripts/AppEvents/

| File | Description |
|---|---|
| `AppEvents.cs` | Static class holding all global `UnityAction` event definitions; the entire inter-system event bus |

---

## Scripts/ConstAndEnums/

| File | Description |
|---|---|
| `GameConsts.cs` | Scene name strings + enum↔string maps, training costs per stat, level-up modifier table (`SCISSOR_MODS`), experience-per-level list |
| `GameEnums.cs` | `GameScenes`, `Stats` (11 stats), `ModifierType`, `TownMenu` (10 locations), `SkillNode` (12 nodes) |

---

## Scripts/Data/

| File | Description |
|---|---|
| `Character.cs` | Abstract base with all flat combat fields and roll methods (DamageRoll, CritRoll, ThornsRoll, CompareActionsAndSetMultiplier); used by Enemy and PlayerOld only |
| `Enemy.cs` | Runtime enemy: `ActionRoll()`, `SetAction()`, `MentalityRollAgainst()` (dual-sided), mentality tracking, gambit list, gold reward |
| `Item.cs` | Data class for items (type, no behavior) |
| `LanguageWords.cs` | Dialogue/language data container |

### Data/Attributes/

| File | Description |
|---|---|
| `StatAttribute.cs` | A single stat: holds `AttributeValue` (base) + `List<BaseModifier>`; `TotalValue` sums all |
| `Modifiers/BaseModifier.cs` | Abstract modifier with stat, modifier value, level, and experience; tagged `[JsonConverter(typeof(BaseModifierConverter))]` for polymorphic deserialization |
| `Modifiers/TrainingHouse/TrainingHouseModifier.cs` | Additive modifier; only applies when `IsUnlocked=true`; has `IsTraining` flag for UI selection state |
| `Modifiers/ScissorBonfire/ScissorBonfireModifier.cs` | Additive level-up modifier; value driven externally from `GameConsts.SCISSOR_MODS` |
| `Modifiers/ScissorBonfire/ScissorBonfireModLevel.cs` | Per-level stat variation table; indexer `[Stats]` returns the delta for each stat at that level |
| `Modifiers/PaperTree/PaperTreeModifier.cs` | Additive skill-tree modifier; has a `SkillTreeData` bool (unlock state) |
| `Modifiers/Library/LibraryModifier.cs` | Additive modifier from library quests (defined but minimally implemented) |

### Data/Context/

| File | Description |
|---|---|
| `AppContext/AppContext.cs` | Static global accessor for current `GameContext`, `Player`, `Attributes`, `TownData` |
| `GameContext/GameContext.cs` | Serializable save state: game name, timestamp, date, `Player`, `List<TownData>`, `List<LibraryQuestProgress>` |
| `Player/Player.cs` | Active player data model using NAttribute/NInt for all stats; also contains legacy `PlayerOld : Character` at bottom of same file |

### Data/Dictionaries/

| File | Description |
|---|---|
| `RPSDictionary.cs` | Custom dictionary implementation (specifics not read; likely a SerializedDictionary variant) |

### Data/Icons/

| File | Description |
|---|---|
| `IconsScrObj.cs` | ScriptableObject holding icon sprites |

### Data/Combat/

| File | Description |
|---|---|
| `Combat/EnemyData.cs` | Design-time enemy config: string `_id` slug, stats, probabilities, costs, rewards, languages, gambit list |
| `Combat/EnemyScrObj.cs` | ScriptableObject wrapping `EnemyData` |
| `Combat/GambitScrObj.cs` | Shareable NPC behavior rule: type (PRIMARY/SECONDARY/TERTIARY), condition, result action; `Evaluate(Enemy, round, playerAction)` |

### Data/Town/

| File | Description |
|---|---|
| `TownData.cs` | Per-location save state: `TownMenu` key, name, message, level, experience, cost, unlock flags, NPC flags |
| `TownView.cs` | Visual config for a town location (name, sprite, color — data for display only) |
| `TownViewScrObj.cs` | ScriptableObject wrapping `List<TownView>` |
| `Library/LibraryQuestData.cs` | Quest definition SO data: `EnemyScrObj` target, kill count, reward stat + amount; used only at materialization time |
| `Library/LibraryQuestProgress.cs` | Runtime quest state (serialized in `GameContext.LibraryQuests`): `_enemyId` string, kill counts, completion flag, page index |
| `Library/LibraryQuest.cs` | Legacy stub — superseded by `LibraryQuestData` + `LibraryQuestProgress` |
| `PaperTree/PaperTreeNode.cs` | Single skill tree node (SkillNode enum key + modifier data) |
| `PaperTree/PaperTreeSkillTree.cs` | Container for `List<PaperTreeNode>` |
| `PaperTree/PaperTreeScrObj.cs` | ScriptableObject for the skill tree definition |
| `TimeCounter/CreditTimeCounter.cs` | Config-only credit counter data (MaxCredits, HoursForACredit); mutable state lives in GameContext |
| `TimeCounter/CreditsTimeCounterScrObj.cs` | ScriptableObject wrapper for credit time counter |
| `Travel/MapLevel.cs` | Level definition: level number, name, steps, reward, sprites, enemy list, availability flags |
| `Travel/MapLevelScrObj.cs` | ScriptableObject wrapping map level data |

---

## Scripts/Managers/

| File | Description |
|---|---|
| `BaseManager.cs` | Abstract MonoBehaviour: `Awake→SetUp()+Subscribe()`, `OnDestroy→UnSubscribe()`, `Initialize()` called externally |
| `GameManager.cs` | Root manager (persists); listens to `SceneManager.sceneLoaded`; orchestrates save/load; dispatches `Initialize()` to scene-root manager |
| `00_Intro/IntroManager.cs` | Manages intro sequence |
| `01_MainMenu/MainMenuManager.cs` | Shows/hides main menu, new game, load game panels; triggers persistence on game select |
| `02_Town/TownManager.cs` | Hub manager: dispatches to sub-managers and sub-controllers on `OnOpenTownMenu`; handles unlock flow |
| `02_Town/TrainingHouseManager.cs` | Manages stat unlock and training selection; deducts gold on unlock |
| `02_Town/ScissorBonfireManager.cs` | Manages level-up: deducts gold, increments player level, applies `SCISSOR_MODS` delta to all ScissorBonfireModifiers |
| `02_Town/PaperTreeManager.cs` | Passes player attributes + skill tree SO to PaperTreeUIController |
| `02_Town/HouseManager.cs` | Player statistics display manager |
| `02_Town/TravelManager.cs` | Map/encounter selection manager |
| `02_Town/LibraryManager.cs` | Library manager: materializes quests into `GameContext` on first open (`MaterializeIfNeeded`), passes progress list to UIController |
| `02_Town/TheaterManager.cs` | Theater/comic viewer manager |
| `02_Town/CreditsTimeCounterManager.cs` | Credits time counter location manager |
| `02_Town/StablesManager.cs` | Stables location manager |
| `02_Town/StoneSmithyManager.cs` | Stone smithy location manager |
| `03_Combat/StepManager.cs` | Dispatches each `MapStep` to the appropriate sub-manager (Combat/Treasure/NPC/SurpriseBox); sets level background; Skip Step debug button |
| `03_Combat/CombatStepManager.cs` | Orchestrates the combat round loop: gambit evaluation (Primary→Secondary→ActionRoll, Tertiary after mentality), damage resolution via `CombatResolver`, HP/energy tracking, training EXP, gold accumulation |
| `03_Combat/TreasureStepManager.cs` | Manages treasure steps: action selection, gold reward, accumulates to CombatContext |
| `03_Combat/NPCStepManager.cs` | Manages NPC rescue dialogue steps |
| `03_Combat/SurpriseBoxStepManager.cs` | Manages Surprise Box steps: random HP or energy effect, shows SurpriseBoxHUDUIController |

---

## Scripts/Services/

| File | Description |
|---|---|
| `ServiceLocator/ServiceLocator.cs` | Singleton (-9999); holds `ServiceDictionary<Type, Component>`; `GetService<T>()`, `SubscribeService<T>()`, `UnsubscribeService<T>()` |
| `ServiceLocator/ServiceSubscriber.cs` | Abstract base (-9989); self-registers/unregisters with ServiceLocator in Awake/OnDestroy |
| `Persistence/PersistenceService.cs` | Save/load JSON to `persistentDataPath/<subfolder>/`; `SaveGame`, `LoadGame`, `LoadGameList`, `DeleteGame`, `UpdateSaveGame`, `GetGamesCount` |
| `SceneService/SceneService.cs` | Async scene loading via coroutine; holds activation until `asyncLoad.progress >= 0.9` |
| `UIService/UIService.cs` | Finds all `UIController` children on Awake; `GetController<T>()` with IsAssignableFrom fallback; `GetController(TownMenu)` switch |
| `ManagerLocator/ManagerService.cs` | Finds all `BaseManager` children on Awake; `GetManager<T>()` with IsAssignableFrom fallback; `GetManager(TownMenu)` switch (partially wired) |

---

## Scripts/UIControllers/

### UIHelpers/Base/

| File | Description |
|---|---|
| `BaseUIElement.cs` | Abstract MonoBehaviour (namespace `Kapibara.UI`); `ShowCanvas()`/`HideCanvas()` via DOTween fade on CanvasGroup; `EnableInteraction()`/`DisableInteraction()`; `SetUp()` abstract |
| `UIController.cs` | Abstract subclass of `BaseUIElement` — currently empty body; all concrete controllers extend this |

### UIHelpers/Tabs/

| File | Description |
|---|---|
| `Tab.cs` | Single tab component |
| `TabContent.cs` | Content panel linked to a tab |
| `UITabsController.cs` | Manages tab selection and content switching |

### UIHelpers/LineRenderer/

| File | Description |
|---|---|
| `UILineRenderer.cs` | Canvas-based line renderer (used by PaperTree to draw node connections) |

### 00_Intro/

| File | Description |
|---|---|
| `IntroUIController.cs` | Intro screen UI |

### 01_MainMenu/

| File | Description |
|---|---|
| `MainMenuUIController.cs` | Main menu panel; `EnableContinueButton(bool)` |
| `NewGameUIController.cs` | New game name input panel |
| `LoadGameUIController.cs` | Save slot list panel; `InstanceGames(List<GameContext>)` populates buttons |
| `Helpers/GameContextButton.cs` | Reusable button component for a save slot |

### 02_Town/

| File | Description |
|---|---|
| `TownUIController.cs` | Location hub grid; `SetData()` populates location buttons; `UpdateTownButton()` refreshes one button after unlock |
| `PlayerUIController.cs` | Persistent player status bar (gold display) |
| `InMenuUIController.cs` | In-menu header/info panel shown when inside a town location |
| `UnlockMenuUIController.cs` | Unlock confirmation dialog; `SetData(TownData, TownView)` |
| `TrainingHouse/TrainingHouseUIController.cs` | Training stat grid; `SetData()`, `SelectTrainingButton()`, `UpdateView()` |
| `TrainingHouse/TrainingButton.cs` | Individual stat training button component |
| `ScissorsBonfire/ScissorsBonfireUIController.cs` | Level-up UI; `SetData(level)`, `UpdateLevelUpView(level)` |
| `ScissorsBonfire/ScissorBonfireVariation.cs` | Per-stat variation display widget |
| `PaperTree/PaperTreeUIController.cs` | Skill tree graph view; `SetData(attributes, scrObj)` |
| `PaperTree/PaperTreeButton.cs` | Skill node button component |
| `PaperTree/PaperTreeLine.cs` | Line connecting two skill nodes |
| `House/HouseUIController.cs` | Player stats display at the house location |
| `House/HouseStat.cs` | Single stat row widget |
| `Library/LibraryUIController.cs` | Library location UI |
| `Theater/TheaterUIController.cs` | Theater location UI |
| `Theater/ComicPlayerUIController.cs` | Comic panel sequence player |
| `Travel/TravelUIController.cs` | Map level selection UI |
| `CreditsTimeCounter/CreditsTimeCounterUIController.cs` | Credits time counter display |
| `Stables/StablesUIController.cs` | Stables location UI |
| `StoneSmithy/StoneSmithyUIController.cs` | Stone smithy location UI |
| `StoneSmithy/StoneSmithyButton.cs` | Stone smithy item button |

---

## Scripts/Util/

### NotificableFields/

| File | Description |
|---|---|
| `NotificableField<T>.cs` | Generic wrapper; value change fires `OnValueChanged` + `AppEvents.OnGameContextUpdated` |
| `NInt.cs` | `NotificableField<int>` |
| `NBool.cs` | `NotificableField<bool>` |
| `NString.cs` | `NotificableField<string>` |
| `NFloat.cs` | `NotificableField<float>` |
| `NList<T>.cs` | `NotificableField<List<T>>` |
| `NAttribute.cs` | `NotificableField<StatAttribute>`; constructor `(Stats, int)` auto-adds TrainingHouseModifier + PaperTreeModifier |

### Serializer/

| File | Description |
|---|---|
| `BaseModifierConverter.cs` | Newtonsoft `JsonConverter` for `BaseModifier`; reads `ModifierType` discriminator to pick concrete type; `CanWrite=false` |
| `BaseSpecifiedConcreteClassConverter.cs` | `DefaultContractResolver` used by `BaseModifierConverter` to handle concrete class resolution |

### Singleton/

| File | Description |
|---|---|
| `SingletonMonoBehaviour<T>.cs` | Destroys duplicate instances; no DontDestroyOnLoad built in |

### Extensions/

| File | Description |
|---|---|
| `AnimatorExtensions.cs` | Animator helpers |
| `ArrayExtensions.cs` | Array helpers |
| `AudioExtensions.cs` | AudioSource helpers |
| `ButtonExtensions.cs` | UnityEngine.UI.Button helpers |
| `DictionaryExtensions.cs` | Dictionary helpers (includes `CreateDictionary` from list) |
| `EnumExtensions.cs` | Enum → Description attribute string (used by `Stats` enum) |
| `ListExtensions.cs` | List helpers |
| `RendererExtensions.cs` | Renderer helpers |
| `StringExtensions.cs` | String helpers |
| `TransformExtensions.cs` | Transform helpers |

### Other Util/

| File | Description |
|---|---|
| `CoroutineQueue/CoroutineQueue.cs` | Sequential coroutine execution queue |
| `DontDestroyOnLoad.cs` | MonoBehaviour that calls `DontDestroyOnLoad(gameObject)` in Awake |
| `RNGGenerator.cs` | `Roll1D(max)`, `RandomBetween(min,max)`, `RandomEnumValue<T>()`; uses Unix timestamp as seed |
| `RPSTimestamp.cs` | Unix timestamp get/convert utilities |
| `RPSEnums.cs` | `Actions`, `Languages`, `ItemType`, `MapStepType`, `EnemyId`, `GambitType`, `GambitCondition`, `VignetteAnimation`, `ComicPageLayout` enums |
| `RPSEditorConst.cs` | Editor-only string constants |
| `SerializedDictionary/UnitySerializedDictionary.cs` | Serializable dictionary base for Unity inspector |
| `Editor/DataEditorWindow.cs` | Custom EditorWindow for data inspection |
| `Editor/HierarchyActions.cs` | Hierarchy right-click context menu extensions |

---

## Scripts/_oldScriptables/ (do not use)

Legacy ScriptableObject data classes, replaced by the current plain C# + JSON approach.

| File | Description |
|---|---|
| `EnemyDataObject.cs` | Old enemy SO |
| `IconsDataObjet.cs` | Old icons SO |
| `ItemDataObject.cs` | Old item SO |
| `LanguageDataObject.cs` | Old language SO |
| `PlayerDataObject.cs` | Old player SO |

---

## Scripts/_Tests/ColorTest/

`Test0.cs` through `Test9.cs` — scratch test files, not NUnit tests, not relevant to game logic.

---

## Non-Script Folders

| Path | Contents |
|---|---|
| `Prefabs/Managers/GameCore.prefab` | The persistent GameManager + services prefab |
| `Prefabs/UI/00_Intro/` | Intro scene UI prefabs |
| `Prefabs/UI/01_MainMenu/` | Main menu UI prefabs |
| `Prefabs/UI/02_Town/` | Town scene UI prefabs (one per location) |
| `ScriptableObjects/Player/` | Player-related SO assets |
| `ScriptableObjects/Town/` | TownViewScrObj + PaperTreeScrObj + MapLevelScrObj assets |
