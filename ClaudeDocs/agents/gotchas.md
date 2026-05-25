# Gotchas

---

## 1. NotificableField does NOT auto-save — saves are explicit

`NotificableField<T>.Value` setter only fires `OnValueChanged` (a local `UnityAction<T>` for UI listeners). It does NOT call `AppEvents.OnGameContextUpdated`. Disk writes only happen when a manager explicitly invokes `AppEvents.OnGameContextUpdated?.Invoke()` (e.g., `CombatStepManager.EndCombat`, `TrainingHouseManager` after unlock). This means assigning `Player.Gold -= 10` alone writes nothing to disk — only the explicit save trigger does. Hot paths like per-round combat stat updates are safe.

---

## 3. `_rectTransforms` is always null in BaseUIElement

`RefreshLayoutGroupsImmediateAndRecursive()` is called on every `ShowCanvas()`, but `_rectTransforms` is a private field that is never assigned anywhere in code — no `GetComponentsInChildren`, no setter. The method silently returns `if (_rectTransforms == null)`. Layout groups are never rebuilt. If a controller depends on this for proper layout, it will silently fail.

---

## 4. `RNGGenerator.Roll1D` re-seeds on every call with the current second — ✅ FIXED 2026-03-20

`InitState` calls removed from `Roll1D`, `RandomBetween(float, float)`, and `RandomEnumValue<T>`. Seed is now set once in `GameManager.SetUp()` using `TickCount ^ Guid.NewGuid().GetHashCode()` for high-entropy, unpredictable seeding. `RandomBetween(int, int)` was already correct (re-seed had been commented out).

---

## 5. `BaseModifierConverter` doesn't handle `LibraryModifier` — ✅ FIXED 2026-05-24

`LIBRARY_MOD` case added to `BaseModifierConverter.ReadJson`. Deserializes as `LibraryModifier` correctly. Old saves without a LibraryModifier load fine; saves with one no longer throw.

---

## 6. `ManagerService.GetManager(TownMenu)` is partially wired

The switch returns `null` for `LIBRARY`, `STABLES`, `STONE_SMITHY`, and `THEATER` (commented out). `TownManager.GoToTownMenu()` checks for `null` and bails with a Debug.Log — so these locations silently refuse to open even when `IsUnlocked = true`. The same applies to the UI side: `UIService.GetController(TownMenu)` is fully wired, so the manager null-check is the gate.

---

## 7. `SingletonMonoBehaviour` does NOT call `DontDestroyOnLoad`

`SingletonMonoBehaviour<T>` only enforces uniqueness (destroys duplicates). It does not call `DontDestroyOnLoad`. The `ServiceLocator` uses this as its base — it persists only because the `GameCore` prefab that contains it presumably has a `DontDestroyOnLoad` component or is in the first scene and never unloaded. If `ServiceLocator` ever appears in a non-persistent scene without a `DontDestroyOnLoad` component on the same object, it will be destroyed on scene load.

---

## 8. `GameConsts.SCISSOR_MODS` covers only 10 level-ups — content gap, not a crash

`ScissorBonfireManager.ConfirmLevelUp()` guards against out-of-bounds with `if (levelIndex >= GameConsts.SCISSOR_MODS.Count) { LogWarning; return; }`, so no crash. However, `SCISSOR_MODS` only has data for levels 1–9 (10 entries, indices 0–9). Players who reach level 10 will see a warning and the level-up button will silently do nothing. Add more entries to `GameConsts.SCISSOR_MODS` and `GameConsts.LEVEL_PRICES_AUX` once higher-level stat variations are designed.

---

## 9. `TrainingHouseModifier` `IsTraining` flag is stored inside the modifier (and serialized)

`IsTraining` is on `TrainingHouseModifier` (a `NBool` field, part of the data model). This is transient UI state living in persistent data — the last-selected training stat will be serialized to the save file on the next explicit save. This is intentional: the save acts as a "resume training" feature between sessions.

---

## 10. `NAttribute` constructor auto-adds two modifiers; the Player constructor adds a third

When `NAttribute(Stats, int)` is constructed, it calls:
```csharp
Value.AddModifier(new TrainingHouseModifier(stat));
Value.AddModifier(new PaperTreeModifier(stat));
```
Then in `Player(string playerName)` constructor, several stats also call:
```csharp
_maxHealth.Value.AddModifier(new ScissorBonfireModifier(Stats.HEALTH));
```
`AddModifier` replaces existing modifiers of the same type (by type comparison). So the `NAttribute` constructor's modifiers can be silently replaced if the Player constructor runs after — which it does, since `NAttribute` is constructed first as part of field initialization. This is intentional but fragile: if you add a modifier via `NAttribute` that the Player constructor also adds, only the Player's version survives.

---

## 11. `AppEvents` fields are null by default — some call sites don't use `?.Invoke()` — ✅ FIXED (already clean as of 2026-03-20)

Audit performed 2026-03-20: all 18 files using `AppEvents` were checked. Every active `.Invoke()` call already uses `?.Invoke()`. The only bare `.Invoke()` found was in a commented-out line in `CreditsTimeCounterManager.cs` — no live risk. No changes needed.

---

## 12. `TownData.LevelProgress` has a hardcoded experience table — ✅ FIXED 2026-03-20

`LevelProgress` used a local `new List<int> { 0, 10, 20, ... 90 }` created inline every time the property was accessed, duplicating `GameConsts.TRAINING_EXP_PER_LEVEL`. Fixed by replacing the inline list with a direct read from `GameConsts.TRAINING_EXP_PER_LEVEL`, matching the pattern in `BaseModifier.LevelProgress`.

---

## 13. Save files have no extension and no file format version — ✅ FIXED 2026-03-20

`LoadGameList` and `GetGamesCount` now use `Directory.GetFiles(_saveDirectory, "Game_*")` — stray files are ignored. `GameContext` now has a `_version` int field (not a `NotificableField`) with `CURRENT_VERSION = 1`. The `[JsonConstructor]` accepts `int version = 0` so old saves load with version 0 without breaking. New saves written from this point forward will include `"Version": 1` in the JSON.

---

## 14. `MapLevel._levelEnemies` references legacy `EnemyDataObject`

`MapLevel` holds `List<EnemyDataObject>` — the old ScriptableObject type from `_oldScriptables`. Travel/combat has not been updated to use the new model. `EnemyDataObject` and the `Enemy` class are separate; neither connects to the new `Player` class. The entire combat pipeline is a stub.

---

## 16. `ComicPlayerUIController._layoutPrefabs` dependía del orden del array — ✅ FIXED 2026-03-21

`GameObject[] _layoutPrefabs` usaba el valor int del enum `ComicPageLayout` como índice. Un array mal ordenado en Inspector silenciosamente instanciaba el layout incorrecto sin error.
Reemplazado por `List<ComicLayoutEntry>` donde cada entrada declara explícitamente su `ComicPageLayout`. `ShowPage()` busca por enum con un loop — el orden del Inspector no importa.

---

## 15. `PaperTreeModifier.SkillTreeData` is a single `bool` — ✅ FIXED 2026-03-20

`bool SkillTreeData` replaced with `List<SkillNode> UnlockedNodes`. `PaperTreeUIController.RestoreNodeState()` now restores `PaperTreeNode._isUnlocked` from the list on `SetData()`. Old saves with `"SkillTreeData": false` load cleanly — Newtonsoft ignores the unknown key and `unlockedNodes` defaults to empty list.
