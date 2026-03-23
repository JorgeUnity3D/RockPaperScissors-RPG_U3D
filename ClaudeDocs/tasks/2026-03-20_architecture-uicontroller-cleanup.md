# 2026-03-20 — Architecture: UIController Passive View Rule + Cleanup

## Accomplished

- Established and documented the UIController passive view rule: UIControllers never read AppContext/GameConsts or mutate business objects; they only render data passed in and fire AppEvents signals.
- Added rule to `CLAUDE.md` under "Key Patterns to Follow".
- Saved rule to persistent memory (`feedback_uicontroller_rule.md`).
- Audited all 32 UIController files — found 4 with real violations, 6 with AppEvents-direct (accepted pattern).
- Fixed Gotcha #15: `PaperTreeModifier.bool SkillTreeData` → `List<SkillNode> UnlockedNodes`. Added `RestoreNodeState()` to `PaperTreeUIController` to restore node unlock state on load.
- Fixed all 4 UIController violations:
  - `UnlockMenuUIController`: `SetData` now takes `bool canAfford` instead of reading `AppContext.Player.Gold`
  - `PaperTreeUIController`: `SetData` now takes `int playerGold`; `SetUpSkillTreeLines` uses it
  - `ScissorsBonfireUIController`: `SetData`/`UpdateLevelUpView` now take `int levelUpCost, bool canAfford, Dictionary<Stats, int> statVariations`; `ScissorBonfireManager` added `GetLevelUpData()` helper
  - `TrainingHouseUIController`: removed `#region DEBUG` (buttons that mutated modifiers directly), removed `GameConsts`/`AppContext` reads, added `_playerGold`/`_trainingCosts` private fields injected via `SetData`; debug buttons moved to `TrainingHouseManager` as Odin `[Button]` methods

## Files Modified

- `CLAUDE.md`
- `ClaudeDocs/agents/gotchas.md` — Gotcha #15 marked fixed
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/Data/Attributes/Modifiers/PaperTree/PaperTreeModifier.cs`
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/UIControllers/02_Town/PaperTree/PaperTreeUIController.cs`
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/Managers/02_Town/PaperTreeManager.cs`
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/UIControllers/02_Town/UnlockMenuUIController.cs`
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/Managers/02_Town/TownManager.cs`
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/UIControllers/02_Town/ScissorsBonfire/ScissorsBonfireUIController.cs`
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/Managers/02_Town/ScissorBonfireManager.cs`
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/UIControllers/02_Town/TrainingHouse/TrainingHouseUIController.cs`
- `RockPaperScissors-RPG_U3D/Assets/_RPS/Scripts/Managers/02_Town/TrainingHouseManager.cs`

## Bugs Fixed

- Gotcha #15 — PaperTreeModifier single bool (data model now correct for Phase 1)
- 4 UIController architecture violations (AppContext reads, GameConsts reads, modifier mutations)

## Pending / Next Steps

- Phase 1 item 2: clarify InitialEnergy vs BaseEnergy relationship for combat
- Phase 1 item 3: add Backpack/consumables structure to Player (3 slots)
- Phase 1 item 4: document HasNpc vs NpcUnlocked intent in TownData
- Note: `TrainingHouseUIController` still has `[Header("DEBUG")]` fields `_addExperienceButton` / `_addLevelButton` referenced in the Inspector — these are now dead (no code uses them). They should be removed from the prefab in Unity Editor and the `[Header("DEBUG")]` line removed from the script on next pass.
