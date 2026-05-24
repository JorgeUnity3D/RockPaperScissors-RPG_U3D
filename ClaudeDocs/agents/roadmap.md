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
| Buildings purchasable with gold | ✅ Completo — `TownData.IsUnlocked`, gold deducted, 4-state building visual (unbuilt/built/built-no-NPC/built+NPC) implementado en Phase 6 |
| `TownData.HasNpc` / `NpcUnlocked` fields | ✅ `NpcUnlocked = true` seteado por `GameManager.OnStepFinished` al resolver un `MapStepType.NPC_RESCUE` |
| 10-level map with adjacency unlocks | ❌ `MapLevel._isAvailable` es read-only property; nunca se setea a `true` desde `GameManager` ni ningún otro script — unlock-on-clear no implementado |
| Level clear → unlock adjacent level | ❌ Not implemented |
| Level clear → unlock town NPC | ✅ Implementado via `MapStepType.NPC_RESCUE` en `GameManager.OnStepFinished` |
| Boss Final (after level 10) | ❌ No existe código para un boss separado post-nivel 10 |
| Tutorial combat | ❌ Not implemented |
| CreditsTimeCounter / Caballos | ✅ Completo — `CreditsTimeCounterManager` suscribe a `AppEvents.OnTravelRequested`; llama `UseCredit()` y solo entonces dispara `OnTravelConfirmed`; timer offline funciona con Unix timestamps; `StablesManager` llama `EarnCredit()` al ver anuncio |

---

## 2. Partially Implemented — Needs Completion

### Training House ⚠️ Parcial
- **What works:** stat unlock (gold deducted, `IsUnlocked=true`), stat selection (`IsTraining=true` on one modifier), EXP gain per combat round acumulado en `CombatStepManager` y aplicado al salir.
- **What's missing:** Auto-select-next — `TrainingHouseManager` no suscribe a `AppEvents.OnTrainingLevelUpdated`; cuando un stat se completa (llega a nivel máximo) el jugador tiene que seleccionar manualmente el siguiente.
- **Files:** `TrainingHouseManager.cs`, `TrainingHouseModifier.cs`, `TrainingHouseUIController.cs`

### Scissor Bonfire ⚠️ Parcial
- **What works:** Gold deduction, player level increment, `SCISSOR_MODS` delta aplicado a todos los `ScissorBonfireModifier`; crash en nivel 10 guardado.
- **What's missing:** `SCISSOR_MODS` tiene 10 entradas (índices 0–9) — el guard detiene los level-ups en nivel 10 (necesita más datos de diseño). Building-level-up separado del jugador (cada 10 niveles de jugador) nunca trackeado.
- **Files:** `ScissorBonfireManager.cs`, `GameConsts.cs`

### Paper Tree ✅ DONE (Phase 6)
- `PaperTreeManager.OnPaperTreeNodeSelected` valida, deduce oro, actualiza `UnlockedNodes` + `Modifier`, acumula EXP. `PaperTreeButton` dispara el evento. `PaperTreeModifier` usa `List<string> UnlockedNodes` (no el bool antiguo).

### Travel ✅ DONE (Phase 3/4)
- `TravelUIController` renderiza niveles; `CreditsTimeCounterManager` intercepta `OnTravelRequested`, llama `UseCredit()`, dispara `OnTravelConfirmed`; `TravelManager` carga la escena de combate.
- **Pendiente:** unlock-on-clear de niveles adyacentes (ver sección Progresión).

### Theater ✅ DONE — wiring Inspector pendiente (dev)
- `TheaterManager`, `TheaterUIController`, `ComicPlayerUIController` con 7 layouts implementados. `Player.UnlockedStoryIds` con `IsStoryUnlocked(int)` y `UnlockStory(int)`. Boss defeat dispara Historia via `GameManager`.
- **Pendiente dev:** asignar en Inspector `_closeButton`, `_pageContainer` y los 7 `ComicLayoutEntry` en el prefab `ComicPlayer_UIController`.

### Library ✅ DONE (Phase 6)
- `LibraryManager` extiende `BaseManager`, wired en `ManagerService`. Quest state en `GameContext.LibraryQuests`. Kill tracking en `GameManager.ProcessLibraryKills(string enemyId)`. Bonus de stat aplicado en completion via `LibraryModifier.Modifier +=`.

### House ✅ DONE
- `HouseUIController.SetData()` itera todos los `Stats` enum e instancia un `HouseStat` por stat. Los 4 cost stats (`RockCost` etc.) no se muestran — son `NInt` fuera del sistema de atributos; omisión aceptada.

### CreditsTimeCounter ✅ DONE
- Timer con Unix timestamps offline. `OnTravelRequested` → `UseCredit()` → `OnTravelConfirmed`. `StablesManager.WatchAd` → `EarnCredit()`. Timer coroutine recarga créditos automáticamente.

### InMenuUIController ✅ DONE (Phase 6)
- `TownManager` suscribe a `AppEvents.OnBuildingExpUpdated(TownMenu)` y llama `_inMenuUIController.SetData()` si el edificio abierto coincide. Reactivo en runtime.

---

## 3. Bugs to Fix Before Building Further

Listed in priority order.

### P0 — Blocks data integrity

**Bug #3 — `_rectTransforms` is always null in `BaseUIElement`** ✅ FIXED 2026-03-21
- Removed cached list; `RefreshLayoutGroupsImmediateAndRecursive()` now calls `GetComponentsInChildren<RectTransform>()` directly on each show.

**Bug #9 — `TrainingHouseModifier.IsTraining` is transient state stored in persistent data** ✅ NOT A BUG
- `NotificableField` solo dispara `OnValueChanged` para UI observers. El disco se escribe únicamente cuando un manager invoca `AppEvents.OnGameContextUpdated` explícitamente. No hay escrituras excesivas.

**Bug #10 — `NAttribute` constructor adds modifiers that Player constructor may silently replace**
- `NAttribute(Stats, int)` adds `TrainingHouseModifier` + `PaperTreeModifier`. Player constructor then calls `AddModifier(new ScissorBonfireModifier(...))`. `AddModifier` replaces by type — if NAttribute ever adds ScissorBonfireModifier too, only the Player version survives. Currently safe, but fragile if NAttribute is changed.
- **File:** `NAttribute.cs`, `Player.cs`

### P1 — Blocks combat implementation

**Bug #2 — Two Player classes, no bridge** ✅ FIXED (Phase 4)
- `PlayerOld` eliminated. Combat uses `CombatContext` built from `AppContext.Player` directly.

**Bug #14 — `MapLevel._levelEnemies` references legacy `EnemyDataObject`** ✅ FIXED (Phase 3)
- Reemplazado por sistema `MapStep` con `EnemyScrObj`. `EnemyDataObject` obsoleto.

### P2 — Blocks specific location menus

**Bug #6 — `ManagerService.GetManager(TownMenu)` returns null for 4 locations** ✅ FIXED
- `ManagerService` ahora wirea los 9 edificios via switch expression. Verificado en código.

**Bug — `LibraryManager` is not a `BaseManager`** ✅ FIXED (Phase 6)
- `LibraryManager` extiende `BaseManager`, namespace `Kapibara.RPS`, `SetUp`/`Subscribe`/`Initialize` implementados.

**Bug — `StablesManager.cs` and `StoneSmithyManager.cs` do not exist on disk** ✅ FIXED
- Ambos archivos existen en `Scripts/Managers/02_Town/`. Verificado.

### P3 — Silent failures, fragile state

**Bug #7 — `SingletonMonoBehaviour` does not call `DontDestroyOnLoad`**
- `ServiceLocator` persiste gracias a `GameCore` prefab en escena persistente. Riesgo teórico; no urgente.
- **File:** `SingletonMonoBehaviour.cs`

**Bug #10 — `NAttribute` constructor adds modifiers that Player constructor may silently replace**
- Actualmente seguro, pero frágil si `NAttribute` añade algún día `ScissorBonfireModifier`.
- **File:** `NAttribute.cs`, `Player.cs`

**Bug #15 — `PaperTreeModifier.SkillTreeData` is a single `bool`** ✅ FIXED (Phase 6)
- Reemplazado por `List<string> UnlockedNodes` en `PaperTreeModifier`. Serializer actualizado.

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

**Phase 3 Status — ✅ COMPLETE (2026-05-10 / 2026-05-19)**
- ✅ Task 1 — step-based `MapStep` system replaces legacy `EnemyDataObject`. `EnemyScrObj` holds HP, probabilities, gold, languages. All 7 biomes fully populated in `MapLevels.asset`.
- ✅ Task 2 — `TravelManager.OnTravelConfirmed()` deducts horse use (via `CreditsTimeCounterManager`), stores selected level in `CombatContext`, loads combat scene.
- ⚠️ Task 3 — `TravelUIController` renders all levels; `IsAvailable` display is wired; unlock-on-clear (set adjacent `IsAvailable=true`) deferred to Phase 6 since it requires a combat-result event.

---

### Phase 4 — Combat Core (MVP)

> ✅ COMPLETE — 2026-05-19

Full combat loop playable: Town → Travel (credits deducted) → Combat scene → StepManager dispatches steps → CombatManager runs fight → result screen → return to Town with gold + training EXP persisted.

1. ✅ Combat scene + `CombatStepManager : BaseManager` wired in `GameManager.InitializeScene()`.
2. ✅ `CombatContext` (plain C#, not persistent) bridges `AppContext.Player` stats into combat. No `PlayerOld`.
3. ✅ `EnemyScrObj` holds HP, 5 action probabilities, gold min/max, languages. Populated for all levels.
4. ✅ Action selection UI — player picks Rock/Paper/Scissors/Defense/Energy from bottom bar.
5. ✅ NPC action via weighted probability roll (`1D100` mapped to ranges).
6. ✅ Damage resolution — 8×8 matrix, variability roll, critical roll vs `Crit` attribute.
7. ✅ Thorns roll on Attack-vs-Defense matchup.
8. ✅ Round loop — 10 rounds max; ends on HP ≤ 0; gold awarded from `EnemyScrObj`.
9. ✅ Training EXP accumulated per round, applied at combat end, saved via `OnGameContextUpdated`.
10. ✅ `GameManager.OnCombatFinished()` — advances to next step or returns to Town on final step / defeat.

Architecture audit fixes (Blocks A–E) also complete as of 2026-05-19. See `ClaudeDocs/tasks/2026-05-18_fix-plan.md`.

---

### Phase 5 — Combat Features (Post-MVP)

> ✅ COMPLETE — 2026-05-24

Once basic combat works:

1. **Energy system**: ✅ DONE 2026-05-20 — model cleanup: BaseEnergy removed, CurrentEnergy persists between steps, InitialEnergy resets on travel, Enemy uses data.InitialEnergy.
2. **Mentality roll + language system**: ✅ DONE (Phase 4) — `MentalityRollAgainst()`, thought bubble, `LanguageRoll()`, `GetActionIcon()` all implemented in `CombatStepManager`.
3. **Player action selection — 2-step confirmation**: ✅ DONE (2026-05-20) — `_pendingAction` state in `PlayerHUDUIController`; first tap shows thought bubble with common language icon; second tap confirms and fires `OnCombatActionSelected`.
4. **Enemy thought bubble on mentality roll**: ✅ DONE (2026-05-21) — Win → `GetActionIconEnemy(ThinkingAction)`; Lose → `GetActionIconCommon(Actions.NONE)` (= question mark, stored in all Language SOs under `Actions.NONE`). `Language.GetActionIcon` hardened with null guard + warning.
5. **Common language for action bubbles**: ✅ DONE (2026-05-20) — `PlayerHUDUIController.SetCommonLanguage(Language)` wired; action bubbles use common language via `GetActionIconCommon()`; thought bubbles use enemy language (resolved in `CombatStepManager`).
6. **Backpack consumables**: ✅ DONE (2026-05-20) — `OpenBackpack_Button` en combat panel abre `Backpack_Actions` group; Shuriken/Potion/Torch single-use por combate; efecto resuelto en `CombatStepManager.OnBackpackItemUsed()`; niveles desde `Player.XxxItemLevel`; valores desde `StoneSmithyScrObj.amountsPerLevel`.
7. **NPC Gambits**: ✅ DONE (2026-05-24) — `EvaluateGambits(PRIMARY)` en `BeginRound()` salta mentality si activa; SECONDARY sustituye `ActionRoll()` si activa; TERTIARY se evalúa en `OnPlayerActionSelected` solo si `_enemyReadsMind`.
8. **Round 5 Caja Sorpresa**: ✅ DONE (2026-05-24) — `SurpriseBoxStepManager.Initialize()` selecciona efecto aleatorio HP/energía y lo muestra via `SurpriseBoxHUDUIController`. `OnSurpriseBoxCollected` avanza el step.
9. **Round 10 Boss**: ✅ DONE (2026-05-24) — `StepManager` rutea `MapStepType.BOSS` a `_combatManager`. `GameManager.OnStepFinished` detecta victoria de boss, llama `UnlockStory()` y `comicPlayer.SetData(bossStory)`.
10. **Gold reward multipliers**: ✅ DONE (2026-05-21) — Superpoder kill → x2 gold. `_playerSuperKill` tracked in `CombatStepManager`; set when `playerIsSuper && _enemy.CurrentHealth <= 0` in Case 1 and Case 3a; applied in `EndCombat`.
11. **PauseMenuUIController**: ✅ DONE (2026-05-24) — Overlay con settings/resume/options/exit. `PauseManager` suscribe a `OnCombatExited` → `OnStepFinished(false)`.

---

### Phase 6 — Progression Loop Completion

1. ✅ **4-state building visuals** (2026-05-23): `HasNpc` movido de `TownData` a `TownView` (dato estático de diseño). `TownUIController`: botones siempre interactables, routing delegado a `TownManager`. Estado 3 (construido, NPC no rescatado): `InMenuUIController` muestra `_blockedPanel` + oculta `_levelBackgroundImage` y `_levelProgressHolder`; `TownManager` no abre el controller real del edificio. Estado 4 (NPC rescatado): flujo normal.
2. ✅ **NPC rescue pipeline** (ya implementado en Phase 5): `GameManager.OnStepFinished` maneja `MapStepType.NPC_RESCUE` → `townData.NpcUnlocked = true` para el `step.TargetBuilding`. Solo requiere que los `MapLevel` tengan steps `NPC_RESCUE` con `TargetBuilding` configurado en el SO.
3. ✅ **Paper Tree node purchasing** (2026-05-23): `OnPaperTreeNodeSelected` en `PaperTreeManager` valida, deduce oro, actualiza `UnlockedNodes` + `Modifier`, acumula EXP del edificio. `PaperTreeUIController.SelectPaperTreeButton` dispara el evento.
4. ✅ **Library quest tracking** (2026-05-24): Full redesign — quest state materialized into `GameContext.LibraryQuests` (`List<LibraryQuestProgress>`) when Library NPC rescued. Kill tracking moved to persistent `GameManager.ProcessLibraryKills(string enemyId)` — called from `OnStepFinished`, no cross-scene event dependency. `LibraryManager` only materializes + displays; `LibraryQuestCard` renders `LibraryQuestProgress`. Page progression: page N unlocks when all quests on page N-1 are completed. `AppEvents.OnEnemyDefeated` removed (was never consumed). `Player.LibraryKillCounts` removed. Stat bonus applied immediately on completion via `ApplyLibraryReward` → `LibraryModifier.Modifier +=`. EnemyData now has `string _id` slug (stable, localizable key) replacing old enum integer. 7 new `_02` enemies added (one per biome). `Enemies_01` atlas assigned to all `_02` and Boss enemies.
5. ✅ **Theater story progression** (ya implementado en Phase 5): `GameManager.OnStepFinished` → boss defeat (first time) → `AppContext.Player.UnlockStory(bossStory.StoryId)`. Requiere `BossStory` asignado en cada `MapLevel`.
6. ✅ **Stables level-up + InMenuUIController reactivo** (2026-05-23): `AddStablesExp` implementa level-up real con while loop + cap en nivel máximo. `AppEvents.OnBuildingExpUpdated(TownMenu)` añadido como evento genérico — cualquier manager lo dispara al modificar EXP/Level. `TownManager` suscribe, trackea `_currentOpenMenu`, y refresca `InMenuUIController.SetData()` solo si el edificio abierto coincide.

---

### Phase 7 — Polish & Missing Screens

1. **Settings screen**: sound toggle, language selection (`Languages` enum exists), delete save, quit button.
2. **Credits screen**: wired from Start Menu OPTIONS or CREDITS button.
3. **Historia on new game**: trigger `ComicPlayerUIController` (story #1) before first entry to town from `MainMenuManager` or `GameManager`.
4. **Tutorial combat**: high-stat demo player, guided UI overlays per doc.
5. **Town visual polish**: animated NPCs (idle), animated gold bar increment/decrement, travel-use horse bar, backpack icon on map.
6. **`_optionsButton` in `MainMenuUIController`**: hook it to show the Settings screen.
8. **Action bubble animation**: bubbles inflate proportionally to each side's effective power; larger bubble hits and destroys the smaller; tie = both pop. DOTween animation, no gameplay impact. See `ClaudeDocs/investigations/combat-design-clarifications.md` point 4.
9. **Debug menu (in-build testing panel)**: overlay screen accessible via a hidden gesture or dev flag; lets the tester manually trigger actions (add gold, set stat level, add EXP, skip to round N, unlock building, etc.) without needing Odin Inspector. Replaces the debug `[Button]` methods that currently only work in-editor. Scope: `DebugMenuManager` + `DebugMenuUIController`; gate behind `#if DEVELOPMENT_BUILD || UNITY_EDITOR` or a `GameConsts.DEBUG_ENABLED` bool.

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
