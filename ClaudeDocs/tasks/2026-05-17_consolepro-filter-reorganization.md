# ConsolePro Filter Reorganization

**Date:** 2026-05-17

## What was done

Rewrote `Assets/_ThirdParty/ConsolePro/Settings/RPS-CustomFilters.cpf` from scratch.

The original config organized filters **by class type** (Managers / Services / UIControllers / 6 empty slots / Debug). This was replaced with a **by scene/system** layout that covers the full codebase:

| # | Filter | Contents |
|---|--------|----------|
| 1 | Core | GameManager, BaseManager, all Services, ServiceLocator |
| 2 | Menu | IntroManager, MainMenuManager, all Menu UIControllers |
| 3 | Town | All Town managers + all Town UIControllers |
| 4 | Combat | CombatManager, CombatUIController, CombatResultUIController, CombatResolver, Enemy |
| 5 | Progression | All modifiers (BaseModifier, TrainingHouseModifier, LibraryModifier, etc.), StatAttribute |
| 6 | Data | Player, AppContext, GameContext, CombatContext, TownContext |
| 7 | Editor | All editor windows (RPSDatabaseWindow, DataEditorWindow, etc.) |
| 8 | Serialization | BaseModifierConverter, BaseSpecifiedConcreteClassConverter |
| 9 | Errors | Fuzzy `"["` on Message column, Error+Warning only — visible across all filters |
| 10 | Debug | SceneDebugger, Test0 |

## Files changed

- `RockPaperScissors-RPG_U3D/Assets/_ThirdParty/ConsolePro/Settings/RPS-CustomFilters.cpf` — full rewrite

## Notes

- Stale entries removed: `GameStateManager` (class does not exist) and `BaseUIController` (real class is `UIController`)
- Errors filter uses `swallowStandard: false` + `standardCanSwallow: true` so errors remain visible in every context filter, not just the Errors tab
- All 10 filter slots now populated — no empty placeholders
