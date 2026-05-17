# Roadmap

> Audited 2026-03-20. Full codebase read + complete PDF read (RPSTheDocV2.pdf, 26 pages).
> Previous roadmap dated 2026-03-19 — several entries were outdated or inaccurate. This replaces it entirely.

---

## 1. Design Doc vs What Is Actually Implemented

### Screen Flow

| Doc Screen | Status | Notes |
|---|---|---|
| Logo screen (Screen 1) | ✅ Implemented | `IntroManager` + `IntroUIController` |
| Start Menu — START/OPTIONS/CREDITS/EXIT (Screen 2) | ⚠️ Partial | `MainMenuUIController` has Continue/New Game/Load Game buttons plus an `_optionsButton` field wired in inspector; OPTIONS button has no listener hooked in code (`SetUp()` omits it); no CREDITS or EXIT buttons in code |
| Settings window / Ventana de Ajustes (Screen 3) | ❌ Missing | No settings controller; no sound/language/delete-save UI |
| Historia — 12 story cutscenes (Screen 4) | ❌ Missing | `ComicPlayerUIController` exists; not triggered on new game or on boss defeat; initial Historia (story #1) is not wired from any manager |
| Town Map / Mapa Pueblo (Screen 5) | ⚠️ Partial | `TownUIController` renders location buttons and handles unlock flow; missing: 4-state building visuals (doc specifies unbuilt / built / built+NPC-rescued), animated NPCs, animated gold bar, travel-use barra, backpack icon |
| Combat scene (Screen 6) | ❌ Missing | No combat scene, no `CombatManager`, no combat UI |

---

### Town Locations

`UIService.GetController(TownMenu)` is fully wired for all 9 locations.
`ManagerService.GetManager(TownMenu)` is wired for: PAPER_TREE, SCISSORS, TRAINING_HOUSE, TRAVEL, HOUSE. It returns `null` for LIBRARY, STABLES, STONE_SMITHY, THEATER.

| Location | Manager | UI Controller | GetManager wired | Mechanic Complete |
|---|---|---|---|---|
| Travel / Viajar | `TravelManager` | `TravelUIController` | ✅ | ⚠️ Level buttons + preview panel render; `TravelToLevel()` is an empty stub — does not load combat scene or deduct travel uses |
| Stables / Establos | `StablesManager` | `StablesUIController` | ✅ | ❌ Skeleton only; `StablesUIController` has no fields or logic |
| Training House | `TrainingHouseManager` | `TrainingHouseUIController` | ✅ | ⚠️ Unlock + selection work; EXP gain from combat is missing; auto-select-next-on-exit not implemented |
| Stone Smithy | `StoneSmithyManager` | `StoneSmithyUIController` | ❌ null | ❌ No `StoneSmithyManager.cs` file exists on disk; `StoneSmithyButton` exists |
| Scissor Bonfire | `ScissorBonfireManager` | `ScissorsBonfireUIController` | ✅ | ⚠️ Level-up gold deduction + stat delta works; level-10 crash is FIXED (early-return guard added); building level-up every 10 player levels not tracked |
| Paper Tree | `PaperTreeManager` | `PaperTreeUIController` | ✅ | ⚠️ 5-tab graph renders with colored lines; node buttons are set up via `SelectPaperTreeButton()` callback, but the callback body is empty — node purchase not wired; `PaperTreeModifier.SkillTreeData` is still a single bool (Gotcha #15 unresolved) |
| Theater | `TheaterManager` | `TheaterUIController` | ✅ | ⚠️ Manager + UI implementados; `ComicPlayerUIController` reescrito con `List<ComicLayoutEntry>` y 7 layouts (incluyendo `Six_Grid`); wiring de prefabs en Inspector pendiente (dev) |
| Library | `LibraryManager` | `LibraryUIController` | ❌ null | ❌ `LibraryManager.cs` is a bare `MonoBehaviour` stub — not even `BaseManager`; no namespace, no `SetUp`, no `Initialize`; `LibraryModifier` is now correctly wired in `BaseModifierConverter` (Bug #5 FIXED) |
| House (stats display) | `HouseManager` | `HouseUIController` | ✅ | ✅ Iterates all 11 `Stats` enum values via `Enum.GetValues`, instantiates `HouseStat` prefab per stat, displays `TotalValue` + icon |

---

### Player Stats

The design doc defines **16 player stats** (LVL + 15 attributes). `Stats` enum has 11 values. The 4 "cost" stats (Coste Piedra, Coste Papel, Coste Tijera, Coste Defensa) are stored as plain `NInt` fields on `Player`, not as `NAttribute`/`Stats` enum entries — they are not part of the modifier stack and are not shown in `HouseUIController`.

| Doc Stat | In Code | Form |
|---|---|---|
| LVL | ✅ `Player.Level` | `NInt` |
| HP (base 10) | ✅ `Player.MaxHealth` | `NAttribute` (Stats.HEALTH) |
| Mentalidad (base 0) | ✅ `Player.Mentality` | `NAttribute` (Stats.MENTALITY) |
| Piedra (base 3) | ✅ `Player.Rock` | `NAttribute` (Stats.ROCK) |
| Coste Piedra (base 10) | ✅ `Player.RockCost` | `NInt` — not a stat, not in modifier stack |
| Papel (base 3) | ✅ `Player.Paper` | `NAttribute` (Stats.PAPER) |
| Coste Papel (base 10) | ✅ `Player.PaperCost` | `NInt` — same |
| Tijera (base 3) | ✅ `Player.Scissor` | `NAttribute` (Stats.SCISSOR) |
| Coste Tijera (base 10) | ✅ `Player.ScissorCost` | `NInt` — same |
| Defensa (base 3 in doc, 0 in code) | ⚠️ `Player.Defense` | `NAttribute` (Stats.DEFENSE) — initial value is 0 in code, doc says 3 |
| Coste Defensa (base 10) | ✅ `Player.DefenseCost` | `NInt` — same |
| Espinas (base 0) | ✅ `Player.Thorns` | `NAttribute` (Stats.THORNS) |
| Energía Inicial (base 10 in doc, 0 in code) | ⚠️ `Player.InitialEnergy` | `NInt` — initial value is 0 in code, doc says 10 |
| Energía Base | ✅ `Player.BaseEnergy` | `NAttribute` (Stats.ENERGY_BASE) |
| Recuperación Energía (base 5 in doc, 0 in code) | ⚠️ `Player.EnergyRecovery` | `NAttribute` (Stats.ENERGY_RECOVERY) — initial value is 0, doc says 5 |
| Crítico (base 0) | ✅ `Player.Crit` | `NAttribute` (Stats.CRIT) |
| Superpoder (base 0) | ✅ `Player.Superpower` | `NAttribute` (Stats.SUPERPOWER) |

**Note on `InitialEnergy`:** `_initialEnergy` is an `NInt`, not an `NAttribute`, and is separate from `_baseEnergy`. Its relationship to the combat energy system is not defined in code yet. The doc treats Energía Inicial as a stat separate from Recuperación Energía.

**Note on cost stats:** The `[Stats]` indexer on `Player` only returns `StatAttribute` entries (the 11 enum values). The 4 cost values are accessible via `Player.RockCost`, `Player.PaperCost`, etc., but are not shown in `HouseUIController` and do not participate in the modifier system.

---

### Combat System

The entire combat pipeline is unimplemented at the manager/scene level. The legacy `Character` / `PlayerOld` / `Enemy` classes contain roll logic that does not connect to the active `Player` data model.

| Doc Feature | Code Status |
|---|---|
| Combat scene | ❌ No scene; `GameManager.InitializeScene()` has an empty `case GameScenes.COMBAT:` |
| Player action selection (Rock/Paper/Scissors/Defense/Energy) | ❌ No UI, no input |
| NPC action (probability roll via Gambits) | ⚠️ `Enemy.ActionRoll()` exists in legacy `Enemy.cs` but uses `PlayerOld`, not `Player` |
| Mentality roll (PJ reads NPC mind) | ⚠️ `MentalityRollAgainst()` in `Character.cs` (legacy); disconnected |
| Damage modifier table (8×8 from doc) | ❌ `CompareActionsAndSetMultiplier()` in `Character.cs` (legacy, not the 8×8 matrix) |
| Variability roll 1D(Level×0.16+4) | ⚠️ `RNGGenerator.Roll1D` exists; re-seed bug FIXED (Gotcha #4 resolved) |
| Critical hit roll | ⚠️ `CritRoll()` in `Character.cs` (legacy) |
| Thorns roll | ⚠️ `ThornsRoll()` in `Character.cs` (legacy) |
| Super attacks (energy = 100) | ❌ Not implemented |
| Backpack consumables in combat | ❌ `Item.cs` is a data class only; no backpack in `Player`; Stone Smithy not implemented |
| NPC Gambits (Primary / Secondary / Tertiary) | ❌ No gambit system; `Enemy` has flat probabilities only |
| Language roll (NPC mind-read shows enemy-language symbol) | ❌ `Languages` enum + `LanguageWords.cs` exist; not connected |
| 10-round structure with Ronda 5 surprise box + Ronda 10 boss | ❌ Not implemented |
| Training EXP gain during combat | ❌ `TrainingHouseModifier.IsTraining` flag exists; no combat hook |
| Gold reward (Tirada de Recompensa) | ❌ `Enemy` has `GoldMin`/`GoldMax` fields; no reward resolution |
| Post-combat: boss kill → unlock NPC + Historia | ❌ No unlock event pipeline |
| 4 NPC sprite variants (color 1/2 × low/high level) | ❌ Not implemented |

---

### Progression / Unlock System

| Doc Feature | Status |
|---|---|
| Buildings purchasable with gold | ⚠️ `TownData.IsUnlocked` works; gold deducted on confirm; visual update on `TownUIController` fires; 4-state building visual (unbuilt / built / built-no-NPC / built+NPC) not implemented |
| `TownData.HasNpc` / `NpcUnlocked` fields | ✅ Fields exist in `TownData`; not yet set by any game event |
| 10-level map with adjacency unlocks | ❌ `MapLevel._isAvailable` flag exists; no adjacency logic; no unlock-on-clear |
| Level clear → unlock adjacent level | ❌ Not implemented |
| Level clear → unlock town NPC | ❌ Not implemented |
| Boss Final (after level 10) | ❌ Not implemented |
| Tutorial combat | ❌ Not implemented |
| CreditsTimeCounter / Caballos | ✅ `CreditsTimeCounterManager` gestiona créditos (= caballos) con recarga por tiempo; `EarnCredit()` y `UseCredit()` existen; `CreditsTimeCounterUIController` los muestra vía `Update()` polling; `StablesManager` llama `EarnCredit()` al ver anuncio (Phase 2 ✅); `TravelManager` debe llamar `UseCredit()` al viajar (Phase 3 ❌) |

---

## 2. Partially Implemented — Needs Completion

### Training House
- **What works:** stat unlock (deducts gold, sets `IsUnlocked=true` on `TrainingHouseModifier`), stat selection (`IsTraining=true` on one modifier at a time).
- **What's missing:** EXP gain per round during combat; auto-select-next on exit when stat is completed; the UI level progress bar is display only.
- **Files:** `TrainingHouseManager.cs`, `TrainingHouseModifier.cs`, `TrainingHouseUIController.cs`, `TrainingButton.cs`

### Scissor Bonfire
- **What works:** gold deduction, player level increment, `SCISSOR_MODS` delta applied to all `ScissorBonfireModifier` instances; crash at level 10 is now guarded (returns with a `Debug.LogWarning` instead of throwing).
- **What's missing:** `SCISSOR_MODS` only has 10 entries (indices 0–9); the guard means level-ups stop at level 10. More entries need to be designed and added. Building-level-up every 10 player levels not tracked.
- **Files:** `ScissorBonfireManager.cs`, `GameConsts.cs`

### Paper Tree
- **What works:** 5-tab graph renders node buttons and colored connector lines; line colors reflect node state (unlocked / can-afford / cannot-afford / locked); `SetUpSkillNodes()` calls `paperTreeButton.SetUp()` on each node.
- **What's missing:** `SelectPaperTreeButton()` is an empty callback — clicking a node does nothing; node purchase (gold deduction, modifier update) not wired; `PaperTreeModifier.SkillTreeData` is still a single `bool` with no per-node tracking (Gotcha #15 unresolved).
- **Files:** `PaperTreeManager.cs`, `PaperTreeUIController.cs`, `PaperTreeModifier.cs`, `PaperTreeButton.cs`, `PaperTreeNode.cs`

### Travel
- **What works:** `TravelUIController` renders level buttons from `MapLevelScrObj`; clicking a level shows portrait + name + travel button; `TravelManager` wires the callback chain correctly.
- **What's missing:** `TravelManager.TravelToLevel()` is an empty stub — no scene load, no travel-use deduction, no encounter launch; `MapLevel._levelEnemies` uses legacy `EnemyDataObject` (Gotcha #14); no adjacency/unlock display.
- **Files:** `TravelManager.cs`, `TravelUIController.cs`, `MapLevel.cs`

### Theater ✅ Implementado (wiring Inspector pendiente)
- **What works:** `TheaterManager` completo (`SetUp`, `Subscribe`, `Initialize`, `PlayStory`); `TheaterUIController.SetData(stories, unlockedStoryIds)` instancia `StoryButton` por historia; `ComicPlayerUIController` reescrito — usa `List<ComicLayoutEntry>` (busca prefab por enum, no por índice), 7 layouts incluyendo `Six_Grid`. `Player.UnlockedStoryIds` es `List<int>` con `IsStoryUnlocked(int)` y `UnlockStory(int)`.
- **What's missing:** Wiring Inspector: `ComicPlayer_UIController` prefab necesita `_closeButton`, `_pageContainer` y los 7 `ComicLayoutEntry`. Prefabs de layout a crear con `Kapibara/UI/Create Comic Layouts`.
- **Files:** `TheaterManager.cs`, `TheaterUIController.cs`, `ComicPlayerUIController.cs`, `ComicLayoutEntry.cs`, `Player.cs`

### Library
- **What works:** `LibraryUIController` exists; `LibraryModifier` now correctly deserializes (Bug #5 FIXED in `BaseModifierConverter`).
- **What's missing:** `LibraryManager.cs` is a bare `MonoBehaviour` stub with `Start()` and `Update()` only — not even `BaseManager`; not wired in `ManagerService`; `LibraryQuest` has no public kill-count API; no quest tracking, no page progression, no stat bonus on completion.
- **Files:** `LibraryManager.cs`, `LibraryUIController.cs`, `LibraryQuest.cs`, `LibraryModifier.cs`

### House (Stats Display)
- **What works:** Fully functional as a display panel. `HouseUIController.SetData()` iterates `Enum.GetValues(typeof(Stats))` and instantiates a `HouseStat` row per stat; `RefreshData()` updates all rows. All 11 `Stats` enum values are shown.
- **What's missing:** The 4 cost stats (`RockCost`, `PaperCost`, `ScissorCost`, `DefenseCost`) are not displayed — they are `NInt` fields outside the enum/attribute system.
- **Files:** `HouseManager.cs`, `HouseUIController.cs`, `HouseStat.cs`

### CreditsTimeCounter
- **What works:** Timer ticks via `Update()`; credits increment when timer expires; `EarnCredit()` and `UseCredit()` public methods exist and are correct; `CreditsTimeCounterUIController` polls `CreditTimeCounter` via `Update()` and renders the credit icons directly (no AppEvents needed). `CreditsTimeCounterManager` suscribe a `AppEvents.OnEarnCredit` (añadido en Phase 2).
- **What's missing:** `UseCredit()` is not yet called from `TravelManager` (Phase 3).
- **Architecture note:** Credits = caballos/horses. `CreditsTimeCounterManager` owns the credit count and time recharge. `StablesManager` is the IAP/monetisation UI: `WatchAd_Button` → `AppEvents.OnEarnCredit` + stables EXP; `BuyGame_Button` → IAP placeholder (Phase 7). `TravelManager` calls `UseCredit()` on travel (Phase 3).
- **Files:** `CreditsTimeCounterManager.cs`, `CreditsTimeCounterUIController.cs`, `CreditTimeCounter.cs`, `CreditsTimeCounterScrObj.cs`

### InMenuUIController (HUD de edificio)
- **What works:** `TownManager` llama `SetData(TownData, TownView)` al abrir cualquier edificio; muestra nombre, nivel, `LevelProgress` (Slider), NPC info y botón Back.
- **What's missing:** `SetData` se llama una sola vez al entrar. Si un manager modifica `TownData.Experience` en runtime (ej. `StablesManager.WatchAd`), el Slider no se actualiza reactivamente — el jugador ve el valor actualizado sólo la próxima vez que abre el edificio. Solución pendiente: añadir `AppEvents.OnBuildingExpUpdated(TownMenu)` → `TownManager` suscribe → llama `_inMenuUIController.RefreshLevel(float)` (nuevo método a añadir). Aplica a cualquier edificio con EXP en runtime.
- **Files:** `InMenuUIController.cs`, `TownManager.cs`

---

## 3. Bugs to Fix Before Building Further

Listed in priority order.

### P0 — Blocks data integrity

**Bug #3 — `_rectTransforms` is always null in `BaseUIElement`** ✅ FIXED 2026-03-21
- Removed cached list; `RefreshLayoutGroupsImmediateAndRecursive()` now calls `GetComponentsInChildren<RectTransform>()` directly on each show.

**Bug #9 — `TrainingHouseModifier.IsTraining` is transient state stored in persistent data**
- Toggling which stat is selected for training triggers a full disk write. This is UI state living in the save file. When combat EXP is wired, the flag will be correct but the save writes will be excessive.
- Fix: consider a non-serialized bool or a separate in-memory tracking structure for "currently selected" state.
- **File:** `TrainingHouseModifier.cs`

**Bug #10 — `NAttribute` constructor adds modifiers that Player constructor may silently replace**
- `NAttribute(Stats, int)` adds `TrainingHouseModifier` + `PaperTreeModifier`. Player constructor then calls `AddModifier(new ScissorBonfireModifier(...))`. `AddModifier` replaces by type — if NAttribute ever adds ScissorBonfireModifier too, only the Player version survives. Currently safe, but fragile if NAttribute is changed.
- **File:** `NAttribute.cs`, `Player.cs`

### P1 — Blocks combat implementation

**Bug #2 — Two Player classes, no bridge** ✅ FIXED (Phase 4)
- `PlayerOld` eliminated. Combat uses `CombatContext` built from `AppContext.Player` directly.

**Bug #14 — `MapLevel._levelEnemies` references legacy `EnemyDataObject`**
- `EnemyDataObject` is in `_oldScriptables/` and is not connected to the active `Enemy` or `Player` data models. The travel/combat pipeline cannot use `MapLevel` enemy lists as-is.
- **File:** `MapLevel.cs`, `EnemyDataObject.cs`

### P2 — Blocks specific location menus


**Bug #6 — `ManagerService.GetManager(TownMenu)` returns null for 4 locations**
- LIBRARY, STABLES, STONE_SMITHY, THEATER are commented out. Even when `IsUnlocked=true`, `TownManager.GoToTownMenu()` exits early on null manager.
- Fix: uncomment/add the missing cases once those managers are ready.
- **File:** `ManagerService.cs`

**Bug (new) — `LibraryManager` is not a `BaseManager`**
- `LibraryManager.cs` is a bare `MonoBehaviour` with only `Start()` and `Update()`. It has no namespace, no `SetUp()`, no `Initialize()`. Even if wired in `ManagerService`, calling `Initialize()` on it would fail or do nothing.
- Fix: rewrite `LibraryManager` to extend `BaseManager` in `Kapibara.RPS` namespace, following the pattern of `HouseManager` or `PaperTreeManager`.
- **File:** `LibraryManager.cs`

**Bug (new) — `StablesManager.cs` and `StoneSmithyManager.cs` do not exist on disk**
- Files are referenced by name in documentation and `ManagerService` (commented out), but no `.cs` files were found. The `StablesUIController` and `StoneSmithyUIController` + `StoneSmithyButton` exist.
- Fix: create these manager classes before wiring them in `ManagerService`.

### P3 — Silent failures, fragile state

**Bug #7 — `SingletonMonoBehaviour` does not call `DontDestroyOnLoad`**
- `ServiceLocator` persists only because the `GameCore` prefab presumably handles it externally. If `ServiceLocator` appears in a non-persistent scene without an explicit `DontDestroyOnLoad` component, it will be destroyed on scene load.
- **File:** `SingletonMonoBehaviour.cs`

**Bug #15 — `PaperTreeModifier.SkillTreeData` is a single `bool`**
- The tree has 12 `SkillNode` enum values across 5 tabs. Only one bool is stored per modifier. Per-node purchased state is not tracked anywhere on the save model.
- Fix: replace `bool SkillTreeData` with `List<SkillNode> UnlockedNodes` (or `HashSet<SkillNode>`) on `PaperTreeModifier`, and update the serializer.
- **File:** `PaperTreeModifier.cs`, `BaseModifierConverter.cs`

**Bug (new) — `TravelManager.cs` imports `UnityEditor` namespace** ✅ FIXED 2026-03-21
- Directive removed (done during XML doc pass).

**Bug (new) — `CreditsTimeCounterManager.cs` imports `UnityEditor` namespace** ✅ FIXED 2026-03-21
- Directive removed (done during XML doc pass).

**Bug (new) — Initial stat values differ between doc and code** ✅ FIXED (date unknown)
- Values are correct in current code: Defense=3, InitialEnergy=10, EnergyRecovery=5.

### P0 bugs resolved since last roadmap

- **Bug #4 — RNG re-seed FIXED** (Gotcha #4 marked ✅): `InitState` removed from `Roll1D`, `RandomBetween(float,float)`, `RandomEnumValue`. Seed set once in `GameManager.SetUp()` via `TickCount ^ Guid.GetHashCode()`.
- **Bug #5 — LibraryModifier deserializer FIXED** (Gotcha #5 no longer active): `BaseModifierConverter.ReadJson` now has `case ModifierType.LIBRARY_MOD: return JsonConvert.DeserializeObject<LibraryModifier>(...)`. Saves with LibraryModifier will load correctly.
- **Bug #8 — SCISSOR_MODS crash at level 10 FIXED** (Gotcha #8 no longer active): `ScissorBonfireManager.ConfirmLevelUp()` now checks `levelIndex >= GameConsts.SCISSOR_MODS.Count` and returns with a warning instead of throwing. The underlying data gap (no entries beyond level 10) remains and needs more entries when ready.
- **Bug #11 — AppEvents null safety FIXED** (Gotcha #11 marked ✅): Audit confirmed all 18 files using AppEvents already use `?.Invoke()`. No live risk.
- **Bug #12 — Duplicate LevelProgress exp table FIXED** (Gotcha #12 marked ✅): `TownData.LevelProgress` now reads from `GameConsts.TRAINING_EXP_PER_LEVEL`.
- **Bug #13 — Save file version + filter FIXED** (Gotcha #13 marked ✅): `GameContext` has `_version` field (new saves write `"Version": 1`); `LoadGameList` and `GetGamesCount` use `Directory.GetFiles(_saveDirectory, "Game_*")` pattern.

---

## 4. Suggested Order of Work to Reach a Playable Build

"Playable" = new game → town → travel to level 1 → fight one enemy → return with gold → upgrade something → repeat.

---

### Phase 0 — Stabilize What Exists (no new features)

1. **Remove editor `using` directives from `TravelManager.cs` and `CreditsTimeCounterManager.cs`.** Android builds will fail otherwise. One-liner each.
2. **Correct initial stat values in `Player.cs` constructor** to match design doc: `_defense` base = 3, `_initialEnergy` = 10, `_energyRecovery` base = 5.
3. **Fix Bug #3** (`_rectTransforms` null in `BaseUIElement`): decide if layout rebuild is actually needed; if yes, assign in `SetUp()`; if no, remove the dead call.
4. **Rewrite `LibraryManager.cs`** to extend `BaseManager` in namespace `Kapibara.RPS`. Even an empty-but-correct skeleton unblocks wiring it in `ManagerService`.
5. **Create `StablesManager.cs` and `StoneSmithyManager.cs`** skeletons extending `BaseManager`. Empty is fine for now; they just need to exist and compile.
6. **Wire Bug #6**: uncomment `LIBRARY`, `STABLES`, `STONE_SMITHY`, `THEATER` cases in `ManagerService.GetManager(TownMenu)`.

---

### Phase 1 — Finalize the Data Model

Before combat can be built, all data structures must be stable.

1. **Resolve Bug #15** (PaperTreeModifier single bool): replace `bool SkillTreeData` with `List<SkillNode> UnlockedNodes`; update `BaseModifierConverter.ReadJson` to deserialize the list; update `PaperTreeUIController` to read from it.
2. **Decide on `InitialEnergy` vs `BaseEnergy`**: clarify whether `_initialEnergy (NInt)` is the starting energy per combat run and `_baseEnergy (NAttribute)` is the max, or merge them. Doc treats Energía Inicial and Recuperación Energía as separate stats.
3. **Add `Backpack` / consumables to `Player`**: the doc specifies 3 consumable slots (damage-item, heal-item, energy-item). Stone Smithy upgrades these. Design the data structure now (e.g., 3 `NInt` fields for item levels) so Stone Smithy can be implemented in Phase 2.
4. **Add `HasNPC`-based state logic**: `TownData` already has `HasNpc` and `NpcUnlocked` fields. Define which field means what (e.g., `HasNpc=true` after building purchased, `NpcUnlocked=true` after boss defeated), and document the intended usage in `TownData`.

---

### Phase 2 — Wire the Four Blocked Town Menus

1. **Stables (`StablesManager.Initialize()`)** ✅ DONE 2026-03-21: IAP/monetisation UI sobre el sistema de créditos de `CreditsTimeCounterManager`. `WatchAd_Button` → `EarnCredit()` + `TownData(STABLES).Experience++` + actualiza progress bar. `BuyGame_Button` → placeholder TODO. No se añaden campos nuevos al modelo de datos. Level-up del edificio diferido a Phase 6.
2. **Stone Smithy (`StoneSmithyManager.Initialize()`)**: show 3 consumable slots from `Player.Backpack`; wire `StoneSmithyButton` to upgrade cost/level formula from doc (level×N gold).
3. **Theater (`TheaterManager.Initialize()`)** ✅ DONE 2026-03-21: Manager + UI implementados. Wiring prefabs en Inspector pendiente (dev). Ver sección Theater arriba.
4. **Library (`LibraryManager.Initialize()`)**: load `LibraryQuest` list; render quests as locked/unstarted for now. Kill tracking requires combat; wire the display and leave the counter at zero until Phase 4.

---

### Phase 3 — Map & Travel

1. **Resolve `MapLevel._levelEnemies` legacy type** (Gotcha #14): replace `List<EnemyDataObject>` with a plain C# class (e.g., `NPCConfig`) that holds the stats the combat system will need (HP, action probabilities, gold min/max, gambit list, languages). This decision gates all of combat.
2. **Implement `TravelManager.TravelToLevel()`**: deduct one horse use (gate behind count > 0), set selected level in `AppContext` or a `CombatContext`, load `GameScenes.MAP` or `GameScenes.COMBAT` via `SceneService`.
3. **Add adjacency unlock display in `TravelUIController`**: grey out locked levels; unlock based on `MapLevel.IsAvailable`; set `IsAvailable=true` on adjacent levels when a level is cleared (requires combat first, but the display logic can be wired now).

**Phase 3 Status (data complete as of 2026-05-10):**
- ✅ Task 1 — step-based `MapStep` system replaces legacy `EnemyDataObject`. `EnemyScrObj` holds HP, probabilities, gold, languages. All 7 biomes fully populated in `MapLevels.asset`.
- ❌ Task 2 — `TravelManager.TravelToLevel()` is still a stub. **This is the Phase 4 entry point.** Implement first: deduct horse use, store selected level's step list in `CombatContext`, load combat scene.
- ⚠️ Task 3 — `TravelUIController` renders all levels; `IsAvailable` display is wired; unlock-on-clear (set adjacent `IsAvailable=true`) deferred to Phase 6 since it requires a combat-result event.

---

### Phase 4 — Combat Core (MVP)

This is the largest chunk. Build the minimum version that produces a win/lose outcome.

1. **Create the Combat scene** and a `CombatManager : BaseManager`. Wire it in `GameManager.InitializeScene()`.
2. **Bridge Player → combat**: write a `CombatContext` (plain C# class, not persistent) that copies stats from `AppContext.Player` at fight start. Do not extend `PlayerOld`. Discard `PlayerOld` for new combat.
3. **Implement NPC data**: populate `NPCConfig` per the design doc — HP, 5 action probabilities (must sum to 100), gold min/max, gambit list placeholder, language(s). Store in `MapLevel` or a new `NPCScrObj`.
4. **Implement action selection UI**: player picks Rock/Paper/Scissors/Defense/Energy from the bottom bar. NPC choice is simultaneous (hidden until resolution).
5. **Implement NPC action selection** via weighted probability roll (`1D100` mapped to ranges). Skip Gambits for MVP.
6. **Implement damage resolution** per the 8×8 matrix from doc section 8: compare actions, look up multiplier, roll variability (`1D(Level×0.16+4)` via `RNGGenerator.Roll1D`), apply multiplier, roll critical (`1D100 vs Crit attribute`).
7. **Implement Thorns roll** per doc formula for Attack-vs-Defense matchup.
8. **Implement round loop**: 10 rounds; end on HP ≤ 0; award gold from `NPCConfig.GoldMin`/`GoldMax`.
9. **Implement training EXP award**: on round end, if player chose the stat matching `TrainingHouseModifier.IsTraining`, apply EXP per the doc's conditions table (Sin Daño x1.2 → +1, Sin Daño ≥2 → +2, Crítico → +1 additional).
10. **Wire combat entry/exit**: `TravelManager.TravelToLevel()` → load combat scene → return to town on win or loss.

---

### Phase 5 — Combat Features (Post-MVP)

Once basic combat works:

1. **Energy system**: add current-energy tracking to `CombatContext`; wire Energy action to charge per `EnergyRecovery`; gate Super attacks at energy = 100.
2. **Mentality roll** (PJ reads NPC mind): implement variability + mentality formula; show/hide thought bubble and NPC language symbol.
3. **Backpack consumables**: expose 3 item slots from `Player.Backpack` in combat UI; items are single-use per run (recharged by Smithy on next run).
4. **NPC Gambits**: implement Primary → Secondary → Tertiary gambit evaluation on `NPCConfig`; Primary overrides mentality; Tertiary only fires when mentality roll succeeds.
5. **Round 5 Caja Sorpresa**: random HP/energy event (Vida++/+, Energía++/+/--/-).
6. **Round 10 Boss**: spawn `_isSpecialLevel` NPC; award ESCENA + gold on win; trigger Historia cutscene via `ComicPlayerUIController`.
7. **Language system**: roll `1DN` (N = number of languages) when mentality succeeds; display NPC's chosen action in that NPC's symbol set.
8. **Gold reward multipliers**: implement the reward table from doc section 8 (Superpoder kill → x2 reward, Extra cases).

---

### Phase 6 — Progression Loop Completion

1. **4-state building visuals** in `TownUIController`: distinguish unbuilt / purchased-no-NPC / purchased-with-NPC from `TownData.IsUnlocked` + `TownData.NpcUnlocked`. Animate NPCs on the map.
2. **NPC rescue pipeline**: on boss defeat, set `TownData.NpcUnlocked = true` for the corresponding location (Nivel 1 → Training House NPC, etc., per the unlock table in doc section 10). This enables the building's full mechanic.
3. **Paper Tree node purchasing**: implement `SelectPaperTreeButton()` in `PaperTreeUIController` — deduct gold, add `SkillNode` to `PaperTreeModifier.UnlockedNodes`, refresh the stat's `PaperTreeModifier.Modifier`, re-render the tree.
4. **Library quest tracking**: hook combat's NPC-defeat event into `LibraryQuest` kill count; award stat bonus on quest card completion; unlock next page when all cards on a page are done.
5. **Theater story progression**: llamar `AppContext.Player.UnlockStory(storyId)` al derrotar un jefe; el ID corresponde al índice de la historia en `TheaterScrObj.Stories`.
6. **Stables building level-up + InMenuUIController reactivo**: `WatchAd` ya incrementa `TownData(STABLES).Experience`. Implementar level-up real (cada 10 EXP → `Level++`) y conectar `InMenuUIController` para que el Slider se actualice en tiempo real vía `AppEvents.OnBuildingExpUpdated(TownMenu)` → `TownManager` → `InMenuUIController.RefreshLevel(float)`. Ver nota en sección "InMenuUIController" de este doc.

---

### Phase 7 — Polish & Missing Screens

1. **Settings screen**: sound toggle, language selection (`Languages` enum exists), delete save, quit button.
2. **Credits screen**: wired from Start Menu OPTIONS or CREDITS button.
3. **Historia on new game**: trigger `ComicPlayerUIController` (story #1) before first entry to town from `MainMenuManager` or `GameManager`.
4. **Tutorial combat**: high-stat demo player, guided UI overlays per doc.
5. **Town visual polish**: animated NPCs (idle), animated gold bar increment/decrement, travel-use horse bar, backpack icon on map.
6. **`_optionsButton` in `MainMenuUIController`**: hook it to show the Settings screen.
7. **Debug menu (in-build testing panel)**: overlay screen accessible via a hidden gesture or dev flag; lets the tester manually trigger actions (add gold, set stat level, add EXP, skip to round N, unlock building, etc.) without needing Odin Inspector. Replaces the debug `[Button]` methods that currently only work in-editor. Scope: `DebugMenuManager` + `DebugMenuUIController`; gate behind `#if DEVELOPMENT_BUILD || UNITY_EDITOR` or a `GameConsts.DEBUG_ENABLED` bool.

---

## Summary Table

| Phase | Effort | Playable Gate |
|---|---|---|
| 0 — Stabilize | Small (build fixes, skeletons) | Prevents Android build failures |
| 1 — Data model | Small–Medium | Required for correct combat math |
| 2 — Wire 4 menus | Medium | Stables = required for travel uses |
| 3 — Map/Travel | Medium | Required before combat |
| 4 — Combat MVP | Large | **First complete loop** |
| 5 — Combat features | Large | Full design-spec combat |
| 6 — Progression loop | Medium | Meaningful long-term progression |
| 7 — Polish | Variable | Ship quality |
