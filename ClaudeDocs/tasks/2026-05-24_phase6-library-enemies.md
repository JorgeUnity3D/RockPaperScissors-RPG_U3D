# Phase 6 — Library Quest System + Enemy Expansion (2026-05-24)

## What was done

### Library quest tracking — full redesign

**Problem:** Original design fired `AppEvents.OnEnemyDefeated` during combat. `LibraryManager` lives in the Town scene — no listener in combat → kills never tracked. Also, ScrObj-to-ScrObj references were spreading through the codebase.

**Solution:** Materialize quest state into `GameContext` once (when Library NPC is rescued). Kill tracking moved to `GameManager` (persistent, always alive).

- `GameContext`: added `_libraryQuests: List<LibraryQuestProgress>` — replaces `Player._libraryKillCounts: List<int>`
- `LibraryQuestProgress` (NEW `Scripts/Data/Town/Library/LibraryQuestProgress.cs`): serializable runtime quest state — stores `_enemyId` (string), `_enemyDisplayName`, `_currentKills`, `_targetKills`, `_rewardStat`, `_rewardAmount`, `_isCompleted`, `_pageIndex`. No ScrObj references at runtime.
- `LibraryQuestData`: changed `_targetEnemy` from enum `EnemyId` → `EnemyScrObj` reference (used only at materialization time)
- `Player.cs`: removed `_libraryKillCounts` + `LibraryKillCounts` entirely
- `AppEvents.cs`: removed `OnEnemyDefeated` (never had listeners; replaced inline)
- `GameManager.cs`: added `ProcessLibraryKills(string enemyId)`, `ApplyLibraryReward(LibraryQuestProgress)`, `AddLibraryExp(int)`. Called from `OnStepFinished` on enemy defeat.
- `LibraryManager.cs`: rewritten — only handles materialization (`MaterializeIfNeeded`) and display. No kill tracking.
- `LibraryUIController.SetData(List<LibraryQuestProgress>, int unlockedPageCount)`: new signature, instantiates `LibraryQuestCard` per visible quest
- `LibraryQuestCard.SetData(LibraryQuestProgress)`: new signature using progress object

**Backward compatibility:** `GameContext` JsonConstructor accepts `libraryQuests = null` → defaults to empty list. Old saves load without error.

---

### EnemyData._id — string slug

- `EnemyData.cs`: added `[SerializeField] private string _id` + `public string Id => _id` above `_name`
- All 14 existing enemy assets (`_00` and `_01` tiers) updated with unique string slugs:
  - Forest: `forest-goblin`, `forest-orc`
  - Desert: `desert-crawler`, `desert-golem`
  - Ocean: `ocean-serpent`, `ocean-shaman`
  - FrozenMountain: `frozen-imp`, `frozen-golem`
  - Cave: `cave-bat`, `cave-troll`
  - Volcano: `volcano-sprite`, `volcano-brute`
  - Void: `void-wraith`, `void-knight`
- All 7 Boss assets updated: `forest-boss`, `desert-boss`, `ocean-boss`, `frozen-boss`, `cave-boss`, `volcano-boss`, `void-boss`
- Boss portraits: migrated from old boss atlas (`03abc9512c2a87143b6653bb0c02902f`) → `Enemies_01` at each biome's index-1 position

---

### New _02 enemies (7 new ScrObj assets)

One new enemy per level, tier _02. All use `Enemies_01` atlas at the same index as their level's `_00` enemy. All have `_gambits: []` (to be filled in Inspector).

| Asset | ID | Name | Archetype | HP |
|---|---|---|---|---|
| `Enemy_Forest_02` | `forest-shaman` | Moss Shaman | paper + mentality | 11 |
| `Enemy_Desert_02` | `desert-dancer` | Dune Dancer | scissor + crit | 14 |
| `Enemy_Ocean_02` | `ocean-stalker` | Deep Stalker | rock + defense | 18 |
| `Enemy_FrozenMountain_02` | `frozen-wraith` | Blizzard Wraith | scissor + crit + mentality | 16 |
| `Enemy_Cave_02` | `cave-crawler` | Stone Crawler | paper + thorns | 18 |
| `Enemy_Volcano_02` | `volcano-witch` | Ash Witch | scissor + superpower | 16 |
| `Enemy_Void_02` | `void-entity` | Null Entity | energy + superpower | 25 |

All 7 added to `MapLevels.asset` `_possibleEnemies` for their respective levels.

---

### Sprite atlas migration

**Enemies_01** (GUID: `5d60c13e1f8d7879c90718ac7f894b3f`) — color-variant of Enemies_00. Same positional layout.

- `_02` enemies: `Enemies_01` at index matching their level's `_00` enemy (even indices: 0,4,2,6,8,10,12)
- Boss enemies: `Enemies_01` at index matching their level's `_01` enemy (odd indices: 1,5,3,7,9,11,13)

---

## Files changed

### New files
- `Scripts/Data/Town/Library/LibraryQuestProgress.cs` + `.meta`

### Modified scripts
- `Scripts/Data/Combat/EnemyData.cs` — added `_id` field + property
- `Scripts/Data/Town/Library/LibraryQuestData.cs` — EnemyScrObj ref instead of EnemyId enum
- `Scripts/Data/Context/GameContext/GameContext.cs` — added `_libraryQuests`
- `Scripts/Data/Context/Player/Player.cs` — removed `_libraryKillCounts`
- `Scripts/AppEvents/AppEvents.cs` — removed `OnEnemyDefeated`
- `Scripts/Managers/GameManager.cs` — added library kill tracking methods
- `Scripts/Managers/02_Town/LibraryManager.cs` — full rewrite
- `Scripts/UIControllers/02_Town/Library/LibraryUIController.cs` — new `SetData` signature
- `Scripts/UIControllers/02_Town/Library/LibraryQuestCard.cs` — new `SetData` signature

### New ScrObj assets
- `ScriptableObjects/Enemies/Enemy_Forest_02.asset` + `.meta`
- `ScriptableObjects/Enemies/Enemy_Desert_02.asset` + `.meta`
- `ScriptableObjects/Enemies/Enemy_Ocean_02.asset` + `.meta`
- `ScriptableObjects/Enemies/Enemy_FrozenMountain_02.asset` + `.meta`
- `ScriptableObjects/Enemies/Enemy_Cave_02.asset` + `.meta`
- `ScriptableObjects/Enemies/Enemy_Volcano_02.asset` + `.meta`
- `ScriptableObjects/Enemies/Enemy_Void_02.asset` + `.meta`

### Modified assets
- `ScriptableObjects/Levels/MapLevels.asset` — all 7 levels: added `_02` enemy to `_possibleEnemies`
- All 28 existing enemy assets — `_id` slug + portrait updates

## Follow-up
- `_02` enemies have `_gambits: []` — fill in Inspector when gambits are designed
- Library pages: content (quest definitions) still needs data entry in `LibraryScrObj` Inspector
- Gotcha #5 (`BaseModifierConverter` missing `LIBRARY_MOD`) already fixed in code — update gotchas.md
