# Naming Conventions

Inferred from the actual source in `Assets/_RPS/Scripts/`. Where a rule has exceptions, both are documented.

---

## Namespaces

| Namespace | Used for |
|---|---|
| `Kapibara.RPS` | All game code |
| `Kapibara.UI` | UI base classes (`BaseUIElement`, `UIController`, `UITabsController`, `Tab`, `TabContent`) |
| `Kapibara.Util.NotificableFields` | `NotificableField<T>` and all N-typed wrappers |
| `Kapibara.Util.Extensions` | All extension method classes |
| `Kapibara.Util.Singleton` | `SingletonMonoBehaviour<T>` |
| `Kapibara.Util.SerializedDictionary` | `UnitySerializedDictionary<K,V>` |
| `Kapibara.Util.Coroutines` | `CoroutineQueue` (note: file lives in folder `CoroutineQueue/`, namespace is `Coroutines`) |
| `Kapibara.Util.Serialization` | `BaseModifierConverter`, `BaseSpecifiedConcreteClassConverter` |

All concrete game-layer dictionaries (`ServiceDictionary`, `TownDictionary`, etc.) live in `Kapibara.RPS` despite being in `Data/Dictionaries/`.

---

## Classes

All class names are **PascalCase**. No exceptions found.

### Concrete MonoBehaviours
Plain noun or compound noun: `GameManager`, `TownManager`, `TrainingHouseManager`, `ServiceLocator`, `DontDestroyOnLoad`, `TrainingButton`, `GameContextButton`.

### Concrete UI Controllers
Suffix `UIController`: `MainMenuUIController`, `LoadGameUIController`, `TrainingHouseUIController`, `TownUIController`, `ScissorsBonfireUIController`, `PaperTreeUIController`, `PlayerUIController`, `InMenuUIController`, `UnlockMenuUIController`. Each maps 1:1 to a scene/panel.

Sub-components of a UI area do not use the `UIController` suffix: `TrainingButton`, `ScissorBonfireVariation`, `PaperTreeButton`, `PaperTreeLine`, `HouseStat`, `GameContextButton`.

### Abstract Classes
Prefix `Base` for framework-level abstractions: `BaseManager`, `BaseModifier`, `BaseUIElement`.

No `Base` prefix for domain abstractions: `Character` (abstract base for combat entities), `UIController` (abstract, extends `BaseUIElement`), `ServiceSubscriber<T>` (abstract), `UnitySerializedDictionary<K,V>` (abstract).

### Generic Classes
`NotificableField<T>`, `SingletonMonoBehaviour<T>`, `ServiceSubscriber<T>`, `UnitySerializedDictionary<TKey, TValue>`, `NList<T>`. Type parameters: single uppercase letter `T`, or `TKey`/`TValue` for two-parameter cases.

### ScriptableObjects
Suffix `ScrObj` (not `SO`, not `ScriptableObject`): `TownViewScrObj`, `PaperTreeScrObj`, `MapLevelScrObj`, `IconsScrObj`, `CreditsTimeCounterScrObj`. All extend `SerializedScriptableObject` (Odin), not plain `ScriptableObject`. `CreateAssetMenu` uses `menuName = "RPSRPG/Xxx"` and `fileName = "Xxx"` (PascalCase, no spaces).

### Plain C# Data Classes
No suffix: `Player`, `GameContext`, `TownData`, `TownView`, `StatAttribute`, `MapLevel`, `PaperTreeNode`, `PaperTreeSkillTree`, `ScissorBonfireModLevel`.

### Concrete Modifier Classes
Suffix `Modifier`: `TrainingHouseModifier`, `ScissorBonfireModifier`, `PaperTreeModifier`, `LibraryModifier`. All extend `BaseModifier`.

### Concrete Dictionary Classes
All collected in `RPSDictionary.cs`, one file, all one-liners. Suffix `Dictionary`: `ServiceDictionary`, `ManagerDictionary`, `TownDictionary`, `IconsDictionary`, `TrainingDictionary`, `ScissorBonfireDictionary`, `LevelButtonsDictionary`, `LanguageDictionary`. Key/value types implied by name prefix.

### Static Utility Classes
Suffix varies: `AppEvents`, `AppContext`, `GameConsts`, `RPSTimestamp`, `RPSEditorConst`, `RNGGenerator`. These are all `static class`.

### Extension Classes
Suffix `Extensions`: `EnumExtensions`, `DictionaryExtensions`, `ListExtensions`, `ButtonExtensions`, `AnimatorExtensions`, `AudioExtensions`, `RendererExtensions`, `StringExtensions`, `TransformExtensions`, `ArrayExtensions`. All `static class`.

---

## Interfaces

No interfaces are **defined** in the codebase. `ISerializationCallbackReceiver` (from Unity) is *implemented* by `UnitySerializedDictionary`.

---

## Fields

### Private fields — primary convention
`_camelCase` with leading underscore:
```csharp
[SerializeField] private PersistenceService _persistenceService;
[SerializeField] private NString _gameName;
private BaseUIElement _currentTownUIController;
```
Applied consistently across all main game classes: managers, services, data classes, UI controllers.

### Private fields — exceptions

**`CoroutineQueue.cs`** uses `m_` prefix (Unity old-style/imported code):
```csharp
MonoBehaviour m_Owner = null;
Coroutine m_InternalCoroutine = null;
Queue<IEnumerator> actions = new Queue<IEnumerator>();  // no prefix
IEnumerator currentAction;                              // no prefix
```

**`UnitySerializedDictionary.cs`** uses plain camelCase, no prefix, no access modifier:
```csharp
[SerializeField, HideInInspector] private List<TKey> keyData = new List<TKey>();
[SerializeField, HideInInspector] private List<TValue> valueData = new List<TValue>();
```

**`TrainingButton.cs`** uses plain camelCase with `[SerializeField]`, no underscore:
```csharp
[SerializeField] private Image icon;
[SerializeField] private List<Image> levels;
[SerializeField] private Image selectionOverlay;
```

**`Character.cs`** (legacy abstract base) uses bare public fields, no underscore, no access modifier specified (defaults to public via explicit modifier):
```csharp
public string name;
public int level;
public int rock;
public int mentality;
```

### Public fields
Only on legacy `Character`/`Enemy` classes. Plain `camelCase`:
```csharp
public int mentalityMod;
public int storedMentality;
public int rockProb;
```

---

## Properties

All **PascalCase**. Always backed by a private `_camelCase` field.

**Get-only expression body** (most common):
```csharp
public TownMenu TownMenu { get => _townMenu; }
public List<TownView> Data { get => _data; }
```

**Get + Set expression body**:
```csharp
public string Name { get => _name.Value; set => _name.Value = value; }
public int Level { get => _level.Value; set => _level.Value = value; }
```

**Computed (no backing field)**:
```csharp
public int TotalValue { get { int total = ...; foreach (...) total += ...; return total; } }
public float LevelProgress { get { ... return (...) / (...); } }
public bool CanUnlock { get { return _previousNodes.TrueForAll(ptn => ptn.IsUnlocked); } }
```

**Event properties** (add/remove, on `Player`):
```csharp
public event UnityAction<int> OnGoldValueChanged
{
    add { _gold.OnValueChanged += value; }
    remove { _gold.OnValueChanged -= value; }
}
```

**Indexers** — used in `StatAttribute`, `Player`, `PaperTreeScrObj`, `ScissorBonfireModLevel`:
```csharp
public StatAttribute this[Stats stat] { get { return _statAttributes[stat].Value; } }
public List<PaperTreeNode> this[Stats stat] { get { switch (stat) { ... } } }
```

---

## Methods

All **PascalCase**. No exceptions.

### Standard lifecycle overrides
`SetUp()`, `Subscribe()`, `UnSubscribe()`, `Initialize()` — defined on `BaseManager`, overridden in all concrete managers. Note: `UnSubscribe` (not `Unsubscribe`) — the capital S is consistent.

### Unity lifecycle
`Awake()`, `Start()`, `OnDestroy()` — standard Unity names.

### UI methods on `BaseUIElement`
`ShowCanvas()`, `HideCanvas()`, `RefreshUi()`, `EnableInteraction()`, `DisableInteraction()`, `SetUp()` (abstract).

### Controller data-binding methods
`SetData(...)` — used across all UIControllers to receive data from managers. `UpdateView(...)` for partial refreshes. `SetUp()` for one-time initialization (called from `Awake`).

### Event handler methods (private, in managers)
Named after the action they perform, not after the event that triggered them:
- `ConfirmNewGame(string playerName)` — handles `OnConfirmNewGame`
- `LoadSceneAfterIntro()` — handles `OnIntroCompleted`
- `BackFromTownMenu()` — handles `OnBackFromTownMenu`
- `UpdatePlayerGold(int gold)` — handles `Player.OnGoldValueChanged`

### Coroutine methods
Prefixed with `CR`: `CRLoadSceneAsync(string targetSceneName)`. Only one coroutine exists; the pattern is established but not widely used.

### Private helpers
Same PascalCase, no naming distinction from public methods:
```csharp
private void UpdateTrainingButton(TrainingHouseModifier m) { ... }
private void SetStatView(TrainingHouseModifier m) { ... }
private void SplitActionProbabilities() { ... }
```

### Debug log format
```csharp
Debug.Log($"[ClassName] MethodName() -> ");
Debug.Log($"[ClassName] MethodName() -> param {value}");
```
Square brackets, class name, method with `()`, arrow `->`. Applied in almost every method across every class.

---

## Parameters

**camelCase** throughout. No Hungarian prefixes.

Callbacks follow one of two patterns:
- `On` prefix: `OnButtonClick`, `OnFinishCallback`, `OnSelectedCallback`, `OnShowAction`, `OnHideAction`
- Verb-based: `aCoroutineOwner`, `aAction`, `aWaitTime` — only in `CoroutineQueue` (imported style), `a` prefix for arguments

Standard parameter naming:
```csharp
void SaveGame(GameContext gameContext)
void LoadScene(GameScenes targetScene)
void SetData(List<StatAttribute> attributes)
void ShowCanvas(CanvasGroup canvasGroup, float duration = 0.5f, Action onShowAction = null, Action OnHideAction = null)
```
Note the inconsistency in that last signature: `onShowAction` (lowercase o) vs `OnHideAction` (uppercase O) in the same method.

---

## Local Variables

**camelCase** throughout. **Never use `var`** — always declare the explicit type.

```csharp
string gameName = "Game_" + _persistenceService.GetGamesCount();
GameContext gameContext = new GameContext(gameName, playerName);
TownData townData = _townData.Find(tv => tv.TownMenu == townMenu);
TownView townView = _townViews.Find(td => td.TownMenu == townData.TownMenu);
```

Loop control variables: `i`, `n`, `k` for index/count; descriptive names for semantic iteration:
```csharp
foreach (StatAttribute attribute in attributes) { ... }
foreach (PaperTreeNode currentNode in paperTreeSkillTree) { ... }
foreach (KeyValuePair<Stats, TrainingButton> trainigDictEntry in _trainingDictionary) { ... }
```
Note: `trainigDictEntry` is a typo (missing `n`) — present in the source.

Lambda parameters: short (`td`, `tv`, `ptn`, `ptb`, `gc`, `sbd`), almost always abbreviated initials of the type.

---

## Enums

### Enum type names
PascalCase: `GameScenes`, `Stats`, `ModifierType`, `TownMenu`, `SkillNode`, `Actions`, `Languages`, `ItemType`, `EnemyId`.

### Enum values
ALL_CAPS_SNAKE_CASE:
```csharp
GameScenes.INTRO, GameScenes.MAIN_MENU, GameScenes.TOWN
Stats.HEALTH, Stats.ENERGY_BASE, Stats.ENERGY_RECOVERY
TownMenu.TRAINING_HOUSE, TownMenu.PAPER_TREE, TownMenu.STONE_SMITHY
SkillNode.SKT_01, SkillNode.SKT_02  // abbreviated prefix + zero-padded index
ModifierType.TRAININGHOUSE_MOD, ModifierType.SCISSORBONFIRE_MOD
Actions.NONE, Actions.ROCK, Actions.PAPER
EnemyId.ENEMY_0, EnemyId.ENEMY_1
```

### `[Description]` on enum values
Used on `Stats` to provide short display labels (max 6 chars):
```csharp
[Description("Health")] HEALTH = 0,
[Description("Scisso")] SCISSOR = 4,   // truncated — intentional
[Description("En_Bse")] ENERGY_BASE = 7,
```
Accessed via `EnumExtensions.Name()`.

---

## Constants and Static Readonly

`GameConsts` mixes two styles:

**`const` strings** → ALL_CAPS_SNAKE_CASE:
```csharp
public const string INTRO_SCENE = "00_Intro";
public const string MAIN_MENU_SCENE = "01_MainMenu";
```

**`static readonly` collections** → mix of ALL_CAPS and PascalCase:
```csharp
public static readonly Dictionary<GameScenes, string> SceneNames   // PascalCase — lookup by enum
public static readonly Dictionary<Type, ModifierType> ATTRIBUTE_TYPE_VALUE  // ALL_CAPS — value table
public static readonly List<ScissorBonfireModLevel> SCISSOR_MODS            // ALL_CAPS — data table
public static List<int> LEVEL_PRICES_AUX                                    // ALL_CAPS, not readonly (!)
public static List<int> TRAINING_EXP_PER_LEVEL                              // ALL_CAPS, not readonly
```

`RPSEditorConst` uses ALL_CAPS `const`:
```csharp
public const string DATA = "DATA";
public const string DEBUG = "DEBUG";
```

**Pattern observed**: `SceneNames` / `SceneEnums` are PascalCase because they are bi-directional lookup tables (treated as API). Data tables with values (`SCISSOR_MODS`, `TRAINING_MOD_PRICES`) are ALL_CAPS.

---

## Notifiable Field (N-typed) Wrappers

Prefix `N` + type abbreviation. Only for wrappers of `NotificableField<T>`:

| Class | Wraps |
|---|---|
| `NInt` | `NotificableField<int>` |
| `NBool` | `NotificableField<bool>` |
| `NString` | `NotificableField<string>` |
| `NFloat` | `NotificableField<float>` |
| `NList<T>` | `NotificableField<List<T>>` |
| `NAttribute` | `NotificableField<StatAttribute>` |

These are used as **field types inside serialized data classes** (Player, GameContext, TownData, BaseModifier). They are never used as property return types.

---

## Region Blocks

Used consistently across all classes. Names are ALL_CAPS:

```csharp
#region UNITY_LIFECYCLE    // (also seen as UNITY_LIFECYCE — typo in ServiceSubscriber)
#region UNITY_LIFECYCE     // typo variant — only in ServiceSubscriber.cs
#region SETUP
#region CONTROL
#region PROPERTIES
#region FIELDS
#region CONSTRUCTORS
#region EVENTS
#region ROLLS              // Character/Enemy combat
#region STATS              // Character stat mutation
#region TESTING            // Enemy test constructor
#region DEBUG              // Inspector debug buttons in UI controllers
#region TOSTRING
```

`#region UNTY_LIFECYCLE` — typo (missing 'I') in `GameContextButton.cs`.

---

## File Names

One public class per file. File name exactly matches class name. No suffixes beyond what the class name already has:
- `TrainingHouseManager.cs` → `class TrainingHouseManager`
- `TownViewScrObj.cs` → `class TownViewScrObj`
- `NotificableField.cs` → `class NotificableField<T>`

Exception: `Player.cs` contains both `class Player` and `class PlayerOld`. `RPSDictionary.cs` contains eight one-liner dictionary class definitions.

---

## Folder Structure Conventions

### Scene-indexed prefixes
`00_Intro/`, `01_MainMenu/`, `02_Town/` — used under `Managers/`, `UIControllers/`, `Prefabs/UI/`.

### Underscore-prefixed folders = non-production
`_oldScriptables/` — legacy, do not use.
`_Tests/` — scratch tests, not real NUnit tests.
`_RPS/` — the entire project source folder (top-level asset namespace).
`_ThirdParty/` — third-party assets.

### PascalCase for content
`Managers/`, `Services/`, `UIControllers/`, `Util/`, `Data/`, `AppEvents/`, `ConstAndEnums/`, `Prefabs/`, `Scenes/`, `ScriptableObjects/`.

### Domain-named subfolders match class prefix
`TrainingHouse/TrainingHouseManager.cs`, `TrainingHouse/TrainingHouseUIController.cs`, `TrainingHouse/TrainingButton.cs`. The folder name matches the domain prefix used in class names.

### Modifier subfolders
`Modifiers/TrainingHouse/TrainingHouseModifier.cs` — subfolder per modifier type under `Data/Attributes/Modifiers/`.

### Inspector `[Header]` grouping
```csharp
[Header("UI")]         // serialized Unity references
[Header("DEBUG")]      // debug-only buttons/fields
[Header("Icons")]      // icon-related references
[Header("Stats Buttons")]
```
`[ReadOnly]` paired with `[SerializeField]` on fields that are set at runtime (not in inspector): `[SerializeField, ReadOnly] private UIService _uiService;`.

---

## Inconsistencies

These are real inconsistencies in the source, not recommendations.

| Location | Issue |
|---|---|
| `BaseModifier.TotaModifier` | Typo: `TotaModifier` (missing `l`) — used as the property name on the abstract class and all implementations |
| `CoroutineQueue.cs` | Uses `m_` prefix and no-prefix local fields, not `_camelCase` |
| `UnitySerializedDictionary.cs` | Private serialized fields use no underscore (`keyData`, `valueData`) |
| `TrainingButton.cs` | `[SerializeField] private Image icon` — no underscore |
| `MainMenuUIController.cs` | `void NewGameMenu()` and `void LoadGameMenu()` — no access modifier (implicitly `private`), unlike `private void ContinueGame()` in the same file |
| `ShowCanvas(...)` signature | Parameter `onShowAction` (lowercase) vs `OnHideAction` (uppercase) in the same method signature |
| `GameContextButton.cs` | Region name `#region UNTY_LIFECYCLE` — missing 'I' |
| `ServiceSubscriber.cs` | Region name `#region UNITY_LIFECYCE` — missing 'L' |
| `IntroManager.cs` | `using Kapibara.RPS;` inside `namespace Kapibara.RPS { }` — self-import |
| `RPSDictionary.cs` | `LanguageDictionary : UnitySerializedDictionary<Actions, Sprite>` — key type is `Actions` (a game mechanic enum), not a language enum |
| `LEVEL_PRICES_AUX` in `GameConsts` | Named with `_AUX` suffix and not `readonly` — mutable public static list |
| `ScissorsBonfireUIController` | Class and folder name uses `Scissors` (plural), but `ScissorBonfireManager`, `ScissorBonfireModifier`, `ScissorBonfireDictionary`, and `ScissorBonfireVariation` all use `Scissor` (singular) |
