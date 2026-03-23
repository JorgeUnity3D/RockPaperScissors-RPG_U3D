# Phase 2 — Wire the Four Blocked Town Menus

> Canonical detail in: `ClaudeDocs/tasks/2026-03-21_phase2-town-menus.md`

---

## 2026-03-21 — Session 1 (Stables + Stone Smithy)

**Done:** Stables ✅, Stone Smithy ✅

**Files created/modified:**
- `AppEvents.cs` — added `OnEarnCredit`
- `CreditsTimeCounterManager.cs` — subscribes `OnEarnCredit`
- `StablesUIController.cs`, `StablesManager.cs` — full implementation
- `GameConsts.cs` — `STONE_SMITHY_COST_PER_LEVEL`
- `Item.cs` — refactored (Serializable, `amountsPerLevel`, `Clone()`)
- `StoneSmithyData.cs`, `StoneSmithyScrObj.cs` — new
- `Player.cs` — backpack fields renamed to `_attackItemLevel/HealItemLevel/EnergyItemLevel`
- `StoneSmithyUIController.cs`, `StoneSmithyManager.cs` — full implementation
- `roadmap.md` — updated

**Pending wiring (dev):** StoneSmithyScrObj asset creation + inspector assignments

---

## 2026-03-21 — Session 2 (Theater)

**Done:** Theater ✅

**Files created:**
- `RPSEnums.cs` — added `VignetteAnimation`, `ComicPageLayout`
- `AppEvents.cs` — added `OnStorySelected`, `OnComicClosed`
- `Scripts/Data/Town/Theater/ComicVignetteSO.cs`
- `Scripts/Data/Town/Theater/ComicPageSO.cs`
- `Scripts/Data/Town/Theater/ComicStorySO.cs`
- `Scripts/Data/Town/Theater/TheaterScrObj.cs`
- `Scripts/Util/UITween.cs` — static DOTween wrapper (FadeIn, FadeOut, SlideFrom, ZoomIn) + `SlideDirection` enum; namespace `Kapibara.Util`
- `Scripts/UIControllers/02_Town/Theater/StoryButton.cs`

**Files rewritten/modified:**
- `ComicPlayerUIController.cs` — full rewrite from Doozy stub to `UIController`; layout-prefab system; slot-based vignette reveal with UITween animations
- `TheaterUIController.cs` — galería de botones de historia
- `TheaterManager.cs` — SetUp/Subscribe/Initialize/PlayStory
- `Player.cs` — `_unlockedStoriesCount (NInt)`, initial=1; JsonConstructor param `unlockedStoriesCount=1`

**Architecture note:**
- `TheaterScrObj` = config; `Player.UnlockedStoriesCount` = progreso (NInt, persisted). No new modifier type needed.
- `ManagerService` already had THEATER wired — no change required.
- Layout prefabs: each has children named `Slot_0`, `Slot_1`... with Image + CanvasGroup.

**Pending wiring (dev):**
- Create `TheaterScrObj` asset → assign to `TheaterManager._theaterScrObj`
- Create 6 layout prefabs → assign to `ComicPlayerUIController._layoutPrefabs[0..5]`
- Create `StoryButton` prefab → assign to `TheaterUIController._storyButtonPrefab`
- Set ComicPlayerUIController Canvas `sortingOrder` high (e.g. 100)
- Create `ComicVignetteSO/ComicPageSO/ComicStorySO` assets for actual story content

**Pending:** Library (Phase 2 item 4)

---

## 2026-03-22 — Session 3 (Theater ScrObj refactor + Library)

**Done:** Theater ScrObj refactor ✅, Library ✅ — Phase 2 complete (code)

### Theater ScrObj refactor
Naming fix (`SO` → `ScrObj`) + data-wrapper pattern applied to entire Theater hierarchy.

**Files deleted:** `ComicVignetteSO.cs`, `ComicPageSO.cs`, `ComicStorySO.cs` (+ .meta)

**Files created:**
- `Scripts/Data/Town/Theater/ComicVignetteData.cs` — `[Serializable]`, inline vignette data (no longer a ScrObj)
- `Scripts/Data/Town/Theater/ComicPageData.cs` — `[Serializable]`, `_layout + _vignettes`
- `Scripts/Data/Town/Theater/ComicPageScrObj.cs` — `SerializedScriptableObject`, `[HideLabel] ComicPageData _data`
- `Scripts/Data/Town/Theater/ComicStoryData.cs` — `[Serializable]`, `_title + _thumbnail + _pages (List<ComicPageScrObj>)`
- `Scripts/Data/Town/Theater/ComicStoryScrObj.cs` — `SerializedScriptableObject`, `[HideLabel] ComicStoryData _data`

**Files modified:**
- `TheaterScrObj.cs` — `[HideLabel, InlineEditor] List<ComicStoryScrObj> _data`; `Stories` → `Data`
- `TheaterManager.cs` — `List<ComicStorySO>` → `List<ComicStoryScrObj>`; `.Stories` → `.Data`
- `TheaterUIController.cs` — `SetData(List<ComicStoryScrObj> ...)`
- `StoryButton.cs` — `SetUp(ComicStoryScrObj ...)`: `.Thumbnail` → `.Data.Thumbnail`, `.Title` → `.Data.Title`
- `ComicPlayerUIController.cs` — `_currentStory: ComicStoryScrObj`; page access: `_currentStory.Data.Pages[i].Data`; count: `_currentStory.Data.Pages.Count`
- `Player.cs` — `_unlockedStoriesCount (NInt)` → `_unlockedStoryIds (List<int>)`; `IsStoryUnlocked(int)` + `UnlockStory(int)` methods

**Pending wiring (dev):** Delete old `*.asset` files; recreate from new menus `RPSRPG/Theater/ComicStory` / `ComicPage`; reassign in `TheaterData.asset`.

---

### Library
Full implementation, display-only (kill tracking wired in Phase 4).

**Files created:**
- `Scripts/Data/Town/Library/LibraryQuestData.cs` — `[Serializable]`; `EnemyId`, `TargetKills`, `RewardStat`, `RewardAmount`
- `Scripts/Data/Town/Library/LibraryPageData.cs` — `[Serializable]`; `List<LibraryQuestData>`
- `Scripts/Data/Town/Library/LibraryData.cs` — `[Serializable]`; `List<LibraryPageData>`
- `Scripts/Data/Town/Library/LibraryScrObj.cs` — `SerializedScriptableObject`; `[HideLabel] LibraryData _data`
- `Scripts/UIControllers/02_Town/Library/LibraryQuestCard.cs` — MonoBehaviour; `SetData(LibraryQuestData, int currentKills)`

**Files modified:**
- `LibraryManager.cs` — `SetUp()` caches UI + Player; `Initialize()` calls `_libraryUIController.SetData(...)`
- `LibraryUIController.cs` — `SetData(LibraryData, List<int> killCounts)`; instantiates `LibraryQuestCard` per quest (all pages flattened)
- `Player.cs` — `_libraryKillCounts (List<int>)`; `LibraryKillCounts` property; both constructors initialized to `new List<int>()`; JsonConstructor param `libraryKillCounts = null`

**Dead code:** `LibraryQuest.cs` — original stub, superseded by `LibraryQuestData.cs`. Can be deleted in editor.

**Pending wiring (dev):**
- Create `LibraryData.asset` → assign to `LibraryManager._libraryScrObj`
- Create `LibraryQuestCard` prefab (3 TMP texts) → assign to `LibraryUIController._questCardPrefab`
- Assign `_cardContainer` (scroll content Transform) in `LibraryUIController`
