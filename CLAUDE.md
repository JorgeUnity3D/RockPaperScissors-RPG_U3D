# RockPaperScissors-RPG — Claude Context

> Unity 6000.3.8f1 · Android · Turn-based RPG · Solo dev

## Project Overview

Rock-Paper-Scissors RPG for Android. Turn-based combat where the RPS mechanic drives battles. Features: multiple levels & enemies, progression system (skills/stats training), achievements, and inventory/ability management.

**Solo dev project.** Prioritize clarity, maintainability, and clean architecture over cleverness.

Namespaces: `Kapibara.RPS` (game), `Kapibara.UI` (UI base), `Kapibara.Util.*` (utilities).

---

## Directory Layout

```
./                                        ← project root (Claude CWD)
├── CLAUDE.md
├── Builds/                               ← compiled APKs, do not touch
├── Docs/                                 ← human-readable documentation
├── ClaudeDocs/agents/                    ← AI-optimized docs (primary reference)
│   ├── architecture.md                   ← systems, patterns, execution order
│   ├── dependencies.md                   ← all packages and plugins with versions
│   ├── file-index.md                     ← every meaningful file in _RPS/
│   ├── naming-conventions.md             ← inferred naming rules from codebase
│   └── gotchas.md                        ← known bugs, footguns, fragile decisions
├── ClaudeDocs/investigations/            ← research by investigator agents
├── ClaudeDocs/tasks/                     ← completed task summaries
└── RockPaperScissors-RPG_U3D/Assets/
    ├── _RPS/                             ← MAIN GAME CODE (primary work area)
    │   ├── Scripts/AppEvents/            ← global event bus
    │   ├── Scripts/ConstAndEnums/        ← GameConsts, GameEnums
    │   ├── Scripts/Data/                 ← domain models, Player, modifiers, context
    │   ├── Scripts/Managers/             ← BaseManager + per-scene managers
    │   ├── Scripts/Services/             ← ServiceLocator, Persistence, Scene, UI, Manager
    │   ├── Scripts/UIControllers/        ← BaseUIElement, UIController, per-scene controllers
    │   └── Scripts/Util/                 ← NotificableFields, Singleton, Extensions, RNG
    ├── _Art/                             ← sprites, textures, animations
    ├── Resources/                        ← runtime-loaded assets
    ├── Plugins/                          ← DOTween, Odin Inspector (read-only)
    └── _ThirdParty/                      ← TMP, Doozy, JsonDotNet, ConsolePro (read-only)
```

**NEVER edit `_ThirdParty/` or `Plugins/`.** Treat as read-only.

---

## Documentation First

Before touching code on any non-trivial task:
1. Read `ClaudeDocs/agents/architecture.md` — understand the system you're touching
2. Read `ClaudeDocs/agents/gotchas.md` — check for known issues in that area
3. Read `ClaudeDocs/agents/naming-conventions.md` — match existing conventions exactly
4. Check `ClaudeDocs/agents/dependencies.md` — verify any library you plan to use exists

If docs feel outdated after exploring code, flag it before proceeding.

---

## Architecture (summary — read architecture.md for full detail)

```
ServiceLocator (-9999) → ServiceSubscriber (-9989) → BaseManager (default)

AppContext (static) → GameContext (save state) → Player + TownData[]
AppEvents (static UnityActions) — global event bus

ServiceLocator
  ├── PersistenceService   (JSON save/load via Newtonsoft)
  ├── SceneService         (async scene loading)
  ├── UIService            (GetController<T>)
  └── ManagerService       (GetManager<T>)

BaseManager → SetUp() + Subscribe() on Awake, Initialize() called by GameManager
BaseUIElement → UIController → show/hide via DOTween fade on CanvasGroup
```

Scene flow: `Intro → MainMenu → Town` (Map/Combat unimplemented).

---

## Unity-Specific Rules

### Engine & Platform
- Unity **6000.3.8f1**. Use Unity 6 APIs only — no legacy pre-2021 patterns.
- Target: **Android only**. No PC/iOS code unless wrapped in `#if`.
- Modern Android target (API 28+). Confirm exact value in `ProjectSettings/ProjectSettings.asset`.

### C# Conventions
- **Read existing scripts in the target folder before writing anything.** Match the style, structure, and patterns found there. Do not impose external conventions.
- Namespaces: `Kapibara.RPS`, `Kapibara.UI`, `Kapibara.Util.*` — use the one matching the system.
- `[SerializeField] private` over `public` for Inspector fields.
- **Never use `var`.** Always declare the explicit type.
- No `Find()`, `FindObjectOfType()`, `FindWithTag()` at runtime — use ServiceLocator, direct references, or ScriptableObjects.
- Avoid `Update()` polling. Use `AppEvents` subscriptions or coroutines.
- Async: coroutines by default. No UniTask in project.

### Key Patterns to Follow
- New managers: extend `BaseManager`, override `SetUp()`, `Subscribe()`, `UnSubscribe()`, `Initialize()`.
- New UI controllers: extend `UIController`, override `SetUp()`.
- New data fields on Player/TownData: use `NotificableField<T>` wrappers (NInt, NBool, NString, NFloat, NAttribute). Assignments fire `OnValueChanged` (local UI event only — NOT a disk write). Disk writes only happen on explicit `AppEvents.OnGameContextUpdated` invocations.
- New modifiers: extend `BaseModifier`, add a `ModifierType` entry, and add a case to `BaseModifierConverter.ReadJson()` or deserialization will throw.
- Static game data: ScriptableObjects in `_RPS/ScriptableObjects/`.

### UIController Rule (passive view)
**UIControllers never read or write game state.** They render data passed to them and fire `AppEvents` signals. That's it.
- ❌ No `AppContext` reads — if a button needs to know if the player can afford something, the Manager passes a pre-computed `bool`.
- ❌ No `GameConsts` access for calculations — cost/stat logic belongs in the Manager.
- ❌ No mutations of modifiers, Player fields, or any business object.
- ✅ `AppEvents.OnSomething?.Invoke()` is allowed — it's a signal, not logic.
- If you see `AppContext` in a UIController file, it's a violation.

### Performance (Android)
- No allocations in hot paths. Pool objects.
- No LINQ in performance-sensitive code.
- Confirm atlas setup before adding new sprites.

---

## Workflow

### Before writing code
- State your plan in 2-3 lines. If the change touches more than one system, wait for confirmation.
- If requirements are ambiguous, ask — don't assume.

### File operations
- New scripts go in the `_RPS/Scripts/` subfolder matching their system.
- One class per file. Filename = class name, exactly.
- After any change: list what changed and why.

### Task tracking
- After completing a task, write `ClaudeDocs/tasks/YYYY-MM-DD_task-name.md`.
- Include: what was done, files changed, gotchas or follow-up needed.

---

## Critical Gotchas (read gotchas.md for the full list)

- **NotificableField = disk write.** Every `.Value` assignment on Player/TownData triggers full JSON serialization to disk. Never assign in loops or per-frame code.
- **Two Player classes in Player.cs.** `Player` (active) and `PlayerOld : Character` (legacy combat). They are unconnected. Combat will need a bridge. Don't confuse them.
- **IndexOutOfRange at player level 10.** `SCISSOR_MODS` has 10 entries but is used 1-indexed. Level 10 throws. Max safe level is 9.
- **LibraryModifier missing from deserializer.** `BaseModifierConverter` has no case for `LIBRARY_MOD`. Any save with a LibraryModifier will throw on load.
- **`_rectTransforms` is always null.** `RefreshLayoutGroupsImmediateAndRecursive()` silently no-ops on every ShowCanvas(). Layout is never rebuilt.
- **RNGGenerator re-seeds every call with Unix seconds.** Multiple calls within the same second return identical values.
- **SingletonMonoBehaviour does NOT call DontDestroyOnLoad.** ServiceLocator persists only because GameCore prefab handles it externally.
- **AppEvents null safety inconsistent.** Some call sites use `?.Invoke()`, some don't. Always use `?.Invoke()` on any AppEvents invocation.

---

## Extended Docs

Full detail in `ClaudeDocs/agents/`:
- `architecture.md` — execution order, all patterns, scene flow
- `dependencies.md` — all packages/plugins with versions and usage
- `file-index.md` — every file in _RPS/ with one-line descriptions
- `naming-conventions.md` — inferred naming rules from codebase
- `gotchas.md` — 15 documented bugs and fragile decisions
