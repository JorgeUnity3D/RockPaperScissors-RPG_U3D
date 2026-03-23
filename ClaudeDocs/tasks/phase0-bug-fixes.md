# Phase 0 — Stabilize What Exists

> Status: ✅ COMPLETE (2026-03-21)

---

## 2026-03-20 (session 1)

### Accomplished
- Fixed Bug #8 (Gotcha #8): `SCISSOR_MODS` IndexOutOfRangeException at player level 10.

### Files Modified
- `Managers/02_Town/ScissorBonfireManager.cs`

### What Changed
`ConfirmLevelUp()` now captures `levelIndex = AppContext.Player.Level` before `Level++`. Both `LEVEL_PRICES_AUX` and `SCISSOR_MODS` are indexed with `levelIndex` (always in range 0–9). Guard added: if `levelIndex >= SCISSOR_MODS.Count`, returns early with `Debug.LogWarning`.

### Bugs Fixed
- Gotcha #8 — SCISSOR_MODS IndexOutOfRange at level 10.

---

## 2026-03-20 (session 2)

### Accomplished
- Fixed Bug #5 (Gotcha #5): `LibraryModifier` missing from `BaseModifierConverter.ReadJson()` → crash on save load.

### Files Modified
- `Util/Serializer/BaseModifierConverter.cs`

### What Changed
Added `case ModifierType.LIBRARY_MOD` to the switch in `ReadJson()`, deserializing to `LibraryModifier` via `SpecifiedSubclassConversion`.

### Bugs Fixed
- Gotcha #5 — LibraryModifier missing from BaseModifierConverter.

---

## 2026-03-20 (session 3)

### Accomplished
- Fixed Bug #4 (Gotcha #4): `RNGGenerator` re-seeding on every call with Unix seconds caused identical rolls within the same second.

### Files Modified
- `Util/RNGGenerator.cs`
- `Managers/GameManager.cs`

### What Changed
Removed `UnityEngine.Random.InitState(int.Parse(GetTimestamp()))` from `Roll1D`, `RandomBetween(float, float)`, and `RandomEnumValue<T>`. Added single seed call in `GameManager.SetUp()`: `UnityEngine.Random.InitState(System.Environment.TickCount ^ System.Guid.NewGuid().GetHashCode())`.

### Bugs Fixed
- Gotcha #4 — RNGGenerator re-seeds every call.

---

## 2026-03-20 (session 4)

### Accomplished
- Fixed Bug #12 (Gotcha #12): `TownData.LevelProgress` had a hardcoded inline exp table duplicating `GameConsts.TRAINING_EXP_PER_LEVEL`.

### Files Modified
- `Data/Town/TownData.cs`

### What Changed
Replaced the inline `List<int>` in `TownData.LevelProgress` with a direct read from `GameConsts.TRAINING_EXP_PER_LEVEL`, matching the existing pattern in `BaseModifier.LevelProgress`.

### Bugs Fixed
- Gotcha #12 — Duplicate LevelProgress exp table.

---

## 2026-03-20 (session 5)

### Accomplished
- Audited Bug #11 (Gotcha #11): todas las invocaciones activas de `AppEvents` ya usaban `?.Invoke()`. Gotcha marcado como limpio.
- Fixed Bug #13 (Gotcha #13): save versioning y filtro de archivos en `PersistenceService`.

### Files Modified
- `Data/Context/GameContext/GameContext.cs`
- `Services/Persistence/PersistenceService.cs`

### What Changed
`GameContext`: añadido campo `_version` (int), propiedad `Version`, con `int version = 0` en `[JsonConstructor]` para backward compat.
`PersistenceService`: `LoadGameList` y `GetGamesCount` ahora filtran con `"Game_*"` en `GetFiles`.

### Bugs Fixed
- Gotcha #11 — AppEvents null safety (ya estaba limpio).
- Gotcha #13 — Save versioning y filtro de archivos.

---

## 2026-03-20 (session 6)

### Accomplished
- Roadmap completamente re-auditado (PDF 26 páginas + codebase completo). `ClaudeDocs/agents/roadmap.md` reescrito desde cero.
- Eliminado `using static UnityEditor.Experimental.GraphView.GraphView` de `TravelManager.cs` y `CreditsTimeCounterManager.cs`.
- Corregidos valores iniciales de stats en `Player.cs` constructor: Defense 0→3, InitialEnergy 0→10, EnergyRecovery 0→5.
- `GameContext._version`: eliminada constante `CURRENT_VERSION`, reemplazada por literal `1` inline.

### Files Modified
- `ClaudeDocs/agents/roadmap.md` — reescrito completo
- `Managers/02_Town/TravelManager.cs`
- `Managers/02_Town/CreditsTimeCounterManager.cs`
- `Data/Context/Player/Player.cs`
- `Data/Context/GameContext/GameContext.cs`

### Bugs Fixed
- Nuevo — `using UnityEditor` en dos managers (build blocker Android).
- Nuevo — Stats con valores iniciales incorrectos vs design doc.

---

## 2026-03-21

### Accomplished
- Reescrito `LibraryManager.cs` de MonoBehaviour pelado a `BaseManager` correcto en namespace `Kapibara.RPS`.
- Creados `StablesManager.cs` y `StoneSmithyManager.cs` como esqueletos `BaseManager`.
- Descomentados los 4 cases en `ManagerService.GetManager(TownMenu)`: LIBRARY, STABLES, STONE_SMITHY, THEATER.
- Añadidos LibraryManager, StablesManager, StoneSmithyManager, TheaterManager al prefab `ManagerService.prefab`.

### Files Created
- `Managers/02_Town/StablesManager.cs`
- `Managers/02_Town/StoneSmithyManager.cs`

### Files Modified
- `Managers/02_Town/LibraryManager.cs` — reescritura completa a BaseManager
- `Services/ManagerLocator/ManagerService.cs` — descomentados 4 switch cases
- `Prefabs/Managers/02_Town/ManagerService.prefab` — añadidos 4 GameObjects hijo

### Bugs Fixed
- Bug #6 — ManagerService retornaba null para LIBRARY, STABLES, STONE_SMITHY, THEATER.
- Bug #3 — `_rectTransforms` siempre null en `BaseUIElement` (fixed en sesión de refactor del mismo día, ver `2026-03-21_context-refactor-and-docs.md`).
