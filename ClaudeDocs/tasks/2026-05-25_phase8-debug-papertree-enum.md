# Phase 8 — Debug Buttons, PaperTree Refactor, SkillNode Enum

**Date:** 2026-05-25 (session 3)

## What was done

### LibraryManager — debug buttons
- Removed redundant `Simulate NPC Rescued` and `Force Materialize` buttons.
- Added ValueDropdown quest selector (`_debugQuestIndex`) with quest label + progress.
- Added `Add Progress` (increments CurrentKills by 1, capped at TargetKills).
- Added `Complete Quest` (sets IsCompleted, adds reward to modifier).
- Added `Reset Quests` (clears LibraryQuests list, fires OnMenuOpen + OnGameContextUpdated).

### TheaterManager + TheaterUIController — bug fix + debug buttons
- **Bug fix:** `unlockedStoryIds.Contains(i)` → `unlockedStoryIds.Contains(stories[i].StoryId)` (both UIController and Manager). Index vs StoryId mismatch caused wrong stories to show as unlocked.
- Added ValueDropdown story selector with Unlock Story and Lock All Stories debug buttons.
- Lock All Stories resets to only story 0 unlocked.

### ScissorBonfireManager — price bug fix + debug buttons
- **Bug fix:** `GetLevelUpData` used `LEVEL_PRICES_AUX[level - 1]` while `ConfirmLevelUp` used `LEVEL_PRICES_AUX[level]` — off-by-one mismatch. Fixed to use `[level]` consistently.
- Added `Force Level Up` (bypasses gold, applies SCISSOR_MODS, guards max level).
- Added `Reset Level` (sets level to 1, zeroes all ScissorBonfireModifier.Modifier).

**Commit:** 3887eea — pushed to origin/develop.

### StoneSmithyManager — debug buttons
- Added `Force Upgrade Attack`, `Force Upgrade Heal`, `Force Upgrade Energy` (bypass gold, respect max level cap).
- Added `Reset All Items` (resets all StoneSmithyModifier levels and modifiers to zero).

### PaperTree full refactor
- **Removed `IsUnlocked` from `PaperTreeNode`** — was mutable state on a shared ScriptableObject; unlocked status now computed from `modifier.UnlockedNodes` at render time.
- **Fixed `_nextNodes`/`_previousNodes` accumulation bug** — `SetUp()` was called each menu-open without clearing lists first, causing exponential growth. Moved to `PaperTreeScrObj.OnEnable()` with `ClearLinks()` before rebuild.
- **Eliminated `RestoreNodeState()`** from PaperTreeManager — no longer needed.
- **Moved `IconsScrObj` from PaperTreeUIController SerializeField to `StaticDataService`** — fixes architecture violation (UIController had direct SO reference).
  - Added `[SerializeField] private IconsScrObj _statIcons;` and `public IconsScrObj StatIcons` to StaticDataService.
  - Manager fetches `_icons` in `SetUp()`, passes to `SetData()`.
- **`PaperTreeButton.SetUp()`** now receives `bool isUnlocked` to set sprite; no longer reads state from node.
- **`PaperTreeUIController.SetData()`** signature extended with `IconsDictionary icons` and `int playerGold`.
- **`CanUnlock()`** changed from property to method: `public bool CanUnlock(List<SkillNode> unlockedNodes)`.

### PaperTreeEditorWindow (new file)
- Full 3-panel Editor Window following MapLevelEditorWindow pattern.
- Header: ObjectField for PaperTreeScrObj.
- Tab bar: ROCK, PAPER, SCISSOR, DEFENSE, ENERGY REC.
- Left panel: node list with NodeID + modifier + cost, +/− buttons.
- Right panel: node detail — Identity (NodeID, Stat), Values (Modifier, Cost), Connections (NextNodesIDs).
- `DrawEmbedded(float w, float h)` for RPSDatabaseWindow embedding.
- `[MenuItem("Kapibara/Paper Tree Editor")]`.
- Added to RPSDatabaseWindow as `SubView.PaperTree` with a header button.

### PaperTreeManager — debug buttons
- Global node ValueDropdown with `[tree] NodeID +modifier` labels and ✓ for unlocked.
- `Unlock Node` — bypasses gold/prereqs, fires SetData + OnGameContextUpdated.
- `Reset All Nodes` — clears all UnlockedNodes and zeroes Modifier for all 5 stats.

### SkillNode enum — Option A
- Changed from 12 opaque entries (`SKT_01`…`SKT_12`, implicit 0-11) to 35 tree-coded entries.
- Hundreds digit = tree (1=Rock, 2=Paper, 3=Scissor, 4=Defense, 5=Energy), unit = node index.
- `R1=101…R7=107, P1=201…P7=207, S1=301…S7=307, D1=401…D7=407, E1=501…E7=507`.
- **Note:** existing saves with old SKT_XX values will not deserialize correctly. User accepted this (save format irrelevant for current dev state).
- `PaperTreeEditorWindow.AddNode()` sets smart default NodeID: `(_selectedTree + 1) * 100 + (newIndex + 1)`.

## Files changed
- `Scripts/Managers/02_Town/LibraryManager.cs`
- `Scripts/UIControllers/02_Town/Theater/TheaterUIController.cs`
- `Scripts/Managers/02_Town/TheaterManager.cs`
- `Scripts/Managers/02_Town/ScissorBonfireManager.cs`
- `Scripts/Managers/02_Town/StoneSmithyManager.cs`
- `Scripts/Services/StaticData/StaticDataService.cs`
- `Scripts/Data/Town/PaperTree/PaperTreeNode.cs`
- `Scripts/Data/Town/PaperTree/PaperTreeScrObj.cs`
- `Scripts/Managers/02_Town/PaperTreeManager.cs`
- `Scripts/UIControllers/02_Town/PaperTree/PaperTreeButton.cs`
- `Scripts/UIControllers/02_Town/PaperTree/PaperTreeUIController.cs`
- `Scripts/Util/Editor/PaperTreeEditorWindow.cs` (new)
- `Scripts/Util/Editor/RPSDatabaseWindow.cs`
- `Scripts/ConstAndEnums/GameEnums.cs` (SkillNode enum)

## Follow-up needed
- Reassign NodeIDs in all PaperTreeScrObj tree assets in the Unity Inspector (old int values 0-11 no longer map to valid enum entries).
- Wire `StaticDataService._statIcons` in GameCore prefab Inspector if not already done.
