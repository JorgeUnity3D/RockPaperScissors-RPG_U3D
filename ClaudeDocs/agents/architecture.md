# Architecture

Unity 6000.3.8f1. Namespace: `Kapibara.RPS` (game), `Kapibara.UI` (UI base), `Kapibara.Util.*` (utilities).

---

## Execution Order

```
ServiceLocator   -9999   (singleton, persists across scenes)
ServiceSubscriber -9989  (all services: PersistenceService, SceneService, UIService, ManagerService)
BaseManager      default (managers awake after services are registered)
```

---

## Layer Overview

```
AppContext (static accessor)
    └─ GameContext (serializable save state)
        ├─ Player (domain model, NotificableFields)
        │   └─ StatAttribute[] (each stat has a list of BaseModifiers)
        └─ TownData[] (per-location state)

AppEvents (static UnityActions — global event bus)

ServiceLocator (singleton, Dictionary<Type, Component>)
    ├─ PersistenceService (JSON save/load via Newtonsoft)
    ├─ SceneService (async scene load)
    ├─ UIService (GetComponentsInChildren<UIController> on awake)
    └─ ManagerService (GetComponentsInChildren<BaseManager> on awake)

GameManager (persists across scenes via DontDestroyOnLoad)
    └─ listens to SceneManager.sceneLoaded → calls ManagerService.GetManager<X>().Initialize()

BaseManager (per-scene, awake calls SetUp() + Subscribe())
    ├─ IntroManager
    ├─ MainMenuManager
    └─ TownManager + child managers (TrainingHouse, ScissorBonfire, PaperTree, ...)

BaseUIElement → UIController (per-canvas-group, show/hide via DOTween fade)
    └─ retrieved by UIService.GetController<T>()
```

---

## Core Patterns

### ServiceLocator
`ServiceLocator` is a `SingletonMonoBehaviour<ServiceLocator>` at execution order -9999. Services self-register via `ServiceSubscriber<T>`, which calls `SubscribeService(this)` in Awake and `UnsubscribeService<T>()` in OnDestroy. Key: only one instance per type is allowed; duplicate registration is silently ignored.

### Manager Lifecycle
`BaseManager.Awake()` → `SetUp()` + `Subscribe()`. `SetUp()` fetches services and controllers from ServiceLocator. `Subscribe()` registers to `AppEvents` or player events. `Initialize()` is called externally (by GameManager via ManagerService after scene load). `OnDestroy` calls `UnSubscribe()`.

### NotificableField System
All serialized data fields on Player, GameContext, TownData use `NotificableField<T>` wrappers (NInt, NBool, NString, NAttribute, NList). Every value change fires both `OnValueChanged` (local) and `AppEvents.OnGameContextUpdated` (global). `OnGameContextUpdated` triggers auto-save via `PersistenceService.UpdateSaveGame`. This means **any property assignment on Player or TownData triggers a disk write**.

### Stat/Modifier Stack
`StatAttribute` holds a base `AttributeValue` and a `List<BaseModifier>`. `TotalValue` = `AttributeValue + sum(modifier.TotaModifier)`. Each stat starts with three modifiers by default (set in `NAttribute` constructor and `Player` constructor):
- `TrainingHouseModifier` — additive, only active when `IsUnlocked`
- `PaperTreeModifier` — additive flat, controlled by skill tree
- `ScissorBonfireModifier` — additive, driven by level-up table in `GameConsts.SCISSOR_MODS`

`BaseModifierConverter` (custom Newtonsoft `JsonConverter`) deserializes abstract `BaseModifier` by reading the `ModifierType` discriminator field.

### AppContext
Static class; a thin accessor over `GameContext`. All managers read from `AppContext.Player` and `AppContext.TownData`. It has no lifecycle. It is set by `GameManager` before any scene loads town content.

### Event Bus (AppEvents)
All events are static `UnityAction` or `UnityAction<T>` fields — not properties, not a dedicated event class. Subscribing to a null action fires no error; invoking a null action requires `?.Invoke()` (used inconsistently — some callers use `?.`, some don't; see Gotchas).

### Scene Flow
```
Intro → MainMenu → Town (current endpoint; Map/Combat scenes exist in constants but are unimplemented)
```
`GameManager` listens to `SceneManager.sceneLoaded` and dispatches to the appropriate root manager via `ManagerService`. Scenes are loaded asynchronously via `SceneService.CRLoadSceneAsync` (activation held until progress >= 0.9).

### UI Show/Hide
`BaseUIElement.ShowCanvas()` / `HideCanvas()` uses DOTween `DOFade` on a `CanvasGroup`. On show: enables `Canvas`, sets `interactable=true`, `blocksRaycasts=true`. On hide: disables `Canvas` after fade. Duration defaults to 0.5s. A `RefreshLayoutGroupsImmediateAndRecursive()` call is made on show (iterates `_rectTransforms`, which is never populated in code — it's always null; see Gotchas).

### Persistence
Save files are raw JSON at `Application.persistentDataPath/<subfolder>/<GameName>`. No file extension. `GetGamesCount()` filters out files containing "Test" in the path. `UpdateSaveGame` re-serializes the entire context and overwrites the file on every `NotificableField` change.

---

## Town Location State Machine
Each town location is represented by a `TownData` entry (persisted). When a button is pressed, `AppEvents.OnOpenTownMenu` fires. `TownManager` checks `TownData.IsUnlocked`: if false, shows unlock UI; if true, calls `ManagerService.GetManager(menu).Initialize()` and `UIService.GetController(menu).ShowCanvas()`. Both `GetManager(TownMenu)` and `GetController(TownMenu)` use explicit switch expressions — not all menus are wired (Library, Stables, StoneSmithy, Theater are commented out in ManagerService).

---

## Character/Combat Model (Legacy)
`Character` (abstract) → `PlayerOld` / `Enemy`. Contains the full combat resolution logic: `TotalDamage()`, `DamageRoll()`, `CritRoll()`, `ThornsRoll()`, `CompareActionsAndSetMultiplier()`, `ActionRoll()` (enemy only). This is the **old model** — `Player` (the active data class) does not extend `Character`. Combat scenes are not yet implemented in managers or scenes.
