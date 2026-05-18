# Architecture Audit — 2026-05-18

Full codebase audit across 4 agents: Managers, UIControllers, Data/Services, Combat+Naming.

---

## CRITICAL — Crashes / Data Loss

### 1. StatAttribute.AddModifier() silently bypasses disk save
**File:** `StatAttribute.cs` lines 84–104  
`_modifiers.Value.Remove()` and `.Add()` mutate the inner List in-place, never assigning to the `NotificableField<>` setter. Result: modifier changes (core progression) never fire `OnGameContextUpdated` → no disk write. Data can be lost between in-memory state and what's on disk.

### 2. PersistenceService.LoadGameList() leaves SuppressContextUpdates stuck = true
**File:** `PersistenceService.cs` lines 85–93  
No `try/catch` around the deserialize loop. If any save file throws during deserialization, the stack unwinds with `SuppressContextUpdates` still `true`. All subsequent `NotificableField` writes silently skip saves for the rest of the session with no visible error.

### 3. TRAINING_EXP_PER_LEVEL and LEVEL_PRICES_AUX are public mutable Lists
**File:** `GameConsts.cs` lines 124, 130  
Both lack `readonly`. Any accidental `.Clear()` or `.Add()` corrupts these game-wide constants for the entire session. `TownData.LevelProgress`, `BaseModifier.Level`, `BaseModifier.LevelProgress`, and `StablesManager` all index these lists without defensive copies.

### 4. Player(Player) copy constructor is an empty null-initializer
**File:** `Player.cs` line 302  
`public Player(Player player) { }` — body is empty. All fields remain default/null. The indexer `this[Stats]` accesses `_statAttributes[stat]` → NullReferenceException. Currently unused in production but is public.

### 5. CurrentHealth and CurrentEnergy fire disk writes on every combat tick
**File:** `Player.cs` lines 88–91, 173–176  
`_currentHealth` and `_currentEnergy` are `NInt` (NotificableField). The property has `[JsonIgnore]` so the value is not persisted — but `NotificableField.Value` fires `OnGameContextUpdated` anyway, triggering a full `File.WriteAllText` on every HP/energy change. On Android this causes ANR risk during fast-paced combat.

### 6. CreditsTimeCounterManager uses Update() with potential per-frame disk writes
**File:** `CreditsTimeCounterManager.cs` lines 19–39  
Uses `Update()` polling (explicit arch violation). Inside the loop it modifies `_creditTimeCounter` fields. If `CreditTimeCounter` fields are `NotificableField`-backed, this is a disk write at 60fps.  
Additionally: `StartTimeCounter()` and `SetData()` are called from `SetUp()` — business logic fires before `Initialize()`, breaking the scene lifecycle contract.

### 7. TreasureStepManager and NPCStepManager bypass BaseManager entirely
**Files:** `TreasureStepManager.cs`, `NPCStepManager.cs`  
Both are plain `MonoBehaviour` — no `SetUp()`, no `Subscribe()`/`UnSubscribe()` lifecycle. Service resolution happens in `Awake()`, subscriptions inside `Initialize()`. On scene exit, their AppEvents listeners are never unsubscribed → dangling delegates on reload.

### 8. No step-advance loop — combat pipeline incomplete
**File:** `StepManager.cs` / `AppEvents.cs`  
After `OnCombatFinished` fires, nothing increments `CombatContext.CurrentStepIndex` or re-calls `StepManager.Initialize()`. Every step unconditionally returns the player to Town. Multi-step levels cannot function.

---

## HIGH — Architecture Violations

### 9. TownManager calls Initialize() on sub-managers directly
**File:** `TownManager.cs` line 108  
`targetManager.Initialize()` is called inside `GoToTownMenu()`. Only GameManager may call `Initialize()`. TownManager creates a second orchestrator layer, making execution order unpredictable.  
Same issue in `StepManager.Initialize()` which calls `CombatManager.Initialize()`, `TreasureStepManager.Initialize()`, etc.

### 10. TravelManager holds a direct reference to CreditsTimeCounterManager
**File:** `TravelManager.cs` lines 24, 52–55, 60  
`_creditsManager` is retrieved from ManagerService in `SetUp()` and called directly (`_creditsManager.CreditsLeft`, `_creditsManager.UseCredit()`). Managers must not hold references to other managers — communication must go through `AppEvents`.  
Also: `SceneService` is accessed via `ServiceLocator.Instance.GetService<SceneService>()` inside a business method, not cached in `SetUp()`.

### 11. ScissorBonfireManager — multiple NotificableField writes in a ForEach loop
**File:** `ScissorBonfireManager.cs` lines 67–79  
`ConfirmLevelUp()` runs a `ForEach` over all attributes and sets `ScissorBonfireModifier.Level` and `ScissorBonfireModifier.Modifier` on each. If these properties are `NotificableField`-backed, this fires multiple disk writes inside the loop. Plus `Player.Gold` and `Player.Level++` before the loop = 2 + (N × 2) disk writes per level-up.

### 12. PaperTreeUIController mutates domain object
**File:** `PaperTreeUIController.cs` line 79  
`RestoreNodeState()` writes `node.IsUnlocked = modifier.UnlockedNodes.Contains(node.NodeID)` directly on `PaperTreeNode` domain objects. A UIController is permanently modifying data model state.

### 13. CreditsTimeCounterUIController uses Update() polling
**File:** `CreditsTimeCounterUIController.cs` lines 31–37  
`Update()` reads live domain object fields every frame. The commented code shows the correct event-driven approach was planned (`AppEvents.OnCreditsUpdated`) but reverted.

### 14. TravelUIController / StablesUIController / StoneSmithyUIController — direct callbacks instead of AppEvents
**Files:** `TravelUIController.cs`, `StablesUIController.cs`, `StoneSmithyUIController.cs`  
All three wire manager-supplied `UnityAction` delegates and call them directly, bypassing the `AppEvents` bus entirely. Creates tight controller-to-manager coupling.

### 15. Training EXP not persisted — bypasses NotificableField save chain
**File:** `CombatManager.cs` (`ApplyPendingTrainingExp()`)  
`_activeTrainingModifier.Experience += _pendingTrainingExp` mutates the modifier directly. `AppEvents.OnTrainingExpUpdated` fires, but nothing in its listener chain triggers `OnGameContextUpdated`. Training EXP earned in combat may not be saved.

### 16. MainMenuManager — multiple pattern violations
**File:** `MainMenuManager.cs`  
- `AppEvents.OnBackToMainMenu += Initialize` → `Initialize()` can be called from two paths (GameManager + event), subscriptions stack on reload.  
- `_loadGameUIController.Initialize()` → manager calling `Initialize()` on a UIController (wrong method name, wrong ownership).  
- `_persistenceService.LoadGameList(_loadGameUIController.InstanceGames)` → Service holds a UIController delegate (Service → UIController coupling).

### 17. TrainingHouseUIController has affordability logic
**File:** `TrainingHouseUIController.cs` lines 130–132  
Stores `_playerGold` and `_trainingCosts`, then computes `_playerGold >= trainingCost` inside `UpdateLockedView()`. Business logic executing in the view layer.

---

## MEDIUM — Code Quality / Minor Violations

### 18. TrainingHouseManager caches a snapshot of AppContext.Attributes
**File:** `TrainingHouseManager.cs` line 22  
`AppContext.Attributes` returns `new List<StatAttribute>()` on every access. The cached list can become stale. Also, `SelectTrainingStat()` iterates and writes `IsTraining` — check if `TrainingHouseModifier.IsTraining` is a `NotificableField` (if so, disk writes inside the loop).

### 19. Multiple managers cache `_player = AppContext.Player`
**Files:** `ScissorBonfireManager`, `PaperTreeManager`, `HouseManager`, `TravelManager`, `LibraryManager`, `StoneSmithyManager`, `TreasureStepManager`, `NPCStepManager`  
Managers should read `AppContext.Player` on demand, not mirror it into a private field. The field `_player` also shadows what is effectively a reference to the single Player object — not wrong in practice, but breaks traceability and violates the pattern.

### 20. GameManager caches `_gameContext = AppContext.GameContext` unnecessarily
**File:** `GameManager.cs` line 114  
The field is never used again. Dead code that obscures intent.

### 21. RPSEnums.cs — three enums break SCREAMING_SNAKE_CASE convention
**File:** `RPSEnums.cs`  
`MapStepType` (Combat, Boss, Treasure, NpcRescue), `VignetteAnimation` (FadeIn, SlideFromLeft…), and `ComicPageLayout` (One_Full, Two_Horizontal…) all use PascalCase. `MapStepType` is used in `StepManager`'s switch — a future fix requires migrating all switch cases and serialized values.

### 22. `var` used in several files
- `ServiceLocator.cs` — `var type = typeof(T)`
- `UIService.cs` — `var` in `GetControllers()`
- `RNGGenerator.cs` line 48 — `var values = Enum.GetValues(typeof(T))`
- `HouseStat.cs` line 63 — `var modParent = textMesh.transform.parent`

### 23. StoneSmithyButton has public fields
**File:** `StoneSmithyButton.cs` lines 11–13  
`icon`, `selectionOverlay`, and `levelText` are `public` — should be `[SerializeField] private`.

### 24. TravelUIController.OnTravelConfirmed field naming
**File:** `TravelUIController.cs` line 26  
`private UnityAction<MapLevel> OnTravelConfirmed` — private field missing `_` prefix. Also causes `this.OnTravelConfirmed = OnTravelConfirmed` parameter shadowing.

### 25. Player.Attributes allocates a new List on every access
**File:** `Player.cs` lines 263–282  
Every call to `AppContext.Attributes` or `_player.Attributes` creates a `new List<StatAttribute>`. Any caller iterating this in a loop creates a fresh list per iteration.

### 26. BaseModifier.GetModifier<T>() returns last match, not first
**File:** `StatAttribute.cs` lines 122–133  
Iterates full list overwriting `result` on each match — returns the last one, not the first. The docstring says "first modifier of type T."

### 27. Stats.SCISSOR Description attribute truncated
**File:** `GameEnums.cs` line 34  
`[Description("Scisso")]` — missing trailing 'r'. Displayed label will show "Scisso".

### 28. BaseModifier.Level setter crashes if level = 0
**File:** `BaseModifier.cs` lines 46–51  
`TRAINING_EXP_PER_LEVEL[_level.Value - 1]` → index -1 if level is 0. All constructors initialize level to 1, but any future reset/migration path would crash.

### 29. LibraryModifier and GameEnums.cs have stale "missing converter" warnings
Both the `LibraryModifier.cs` class doc and `GameEnums.cs` `LIBRARY_MOD` comment warn that the converter case is missing. The case was added — the warnings are now incorrect.

### 30. Dead AppEvent — OnCombatRoundResolved never fired
**File:** `AppEvents.cs`  
`OnCombatRoundResolved` is declared but `CombatManager.ResolveRound()` never invokes it. Round results are communicated by direct UIController method calls instead.

### 31. NPCStepManager holds unused `_player` reference
**File:** `NPCStepManager.cs`  
`_player` is fetched in `Awake()` and passed to `SetData()` as `_player.MaxHealth.TotalValue` twice (for both max and current HP — always shows full bar regardless of combat state). No other use. The Player reference should be removed; current HP needs a channel from CombatManager.

### 32. TownData.LevelProgress out-of-bounds at max level
**File:** `TownData.cs` lines 113–115  
`TRAINING_EXP_PER_LEVEL[Level]` without bounds check. If `Level` reaches 10 (the list has 10 entries, 0-9), throws `IndexOutOfRangeException`. No guard equivalent to the one in `BaseModifier`.

### 33. AppEvents.SuppressContextUpdates has no reset guarantee
**File:** `AppEvents.cs`  
If deserialization is interrupted, the flag stays `true` and all subsequent `NotificableField` writes silently skip saves forever until the next app restart.

### 34. ComicPlayerUIController uses runtime Find()
**File:** `ComicPlayerUIController.cs` line 109  
`transform.Find($"Slot_{i}")` — runtime `Find()` by name on an instantiated prefab. Violates the no-Find rule.

### 35. `#endregion EVENTS` misplaced inside OnLevelValueChanged remove accessor
**File:** `Player.cs` lines 448–451  
Structural corruption — the region directive is inside the `remove` body. Code compiles by coincidence; any refactor of that block risks a real compile error.

---

## Files confirmed clean

- `BaseManager.cs` — correct contract, no violations
- `TheaterManager.cs` — cleanest manager in the codebase (AppContext read on demand, no cached Player, AppEvents only)
- `SceneService.cs` — clean, no anti-patterns
- `ServiceSubscriber.cs` — clean base
- `TreasureUIController.cs` — clean passive view
- `NPCUIController.cs` — clean passive view
- `CombatResultUIController.cs` — clean passive view
- `Enemy.cs` — correct POCO model (minor: per-round List allocation)
- `IntroManager.cs` — clean (minor: redundant `using` directive)

---

## Priority Fix Order

| Priority | Issue | Risk |
|---|---|---|
| 1 | StatAttribute.AddModifier bypasses disk save | Silent data loss |
| 2 | PersistenceService SuppressContextUpdates leak | Silent save failure |
| 3 | CurrentHealth/CurrentEnergy disk writes per tick | ANR on Android |
| 4 | CreditsTimeCounterManager Update() + early SetUp() side-effects | Perf + lifecycle |
| 5 | TreasureStepManager/NPCStepManager → extend BaseManager | Arch invariant |
| 6 | No step-advance loop | Combat non-functional |
| 7 | GameConsts mutable lists → add readonly | Data corruption risk |
| 8 | Player copy constructor empty | Latent crash |
| 9 | TownManager/StepManager call Initialize() on other managers | Arch violation |
| 10 | TravelManager direct manager reference | Coupling |
