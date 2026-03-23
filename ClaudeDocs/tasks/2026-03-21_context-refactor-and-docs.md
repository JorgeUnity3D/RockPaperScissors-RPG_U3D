# 2026-03-21 — TownContext Refactor + XML Documentation Pass

## Accomplished

- Clarificado que `_rockCost/_paperCost/_scissorCost/_defenseCost` en `Player` son costes de energía de acción de combate, no costes de oro.
- Creado `TownContext` como clase serializable wrapper dentro de `GameContext`, reemplazando el campo plano `NList<TownData> _townData`.
- Actualizado `GameContext` `[JsonConstructor]` con backward compat: acepta `townContext` (saves nuevos) y `townData` (saves viejos) — fallback a `new TownContext(townData)`.
- Actualizado `AppContext` para exponer propiedad `TownContext`; mantenido shortcut `TownData` para que todos los managers compilen sin cambios.
- Fixed Bug #3: eliminado campo `_rectTransforms` (nunca asignado); `RefreshLayoutGroupsImmediateAndRecursive()` ahora llama `GetComponentsInChildren<RectTransform>()` directamente.
- Añadido "Debug Menu (in-build testing panel)" a Phase 7 del roadmap.
- Añadida regla "Never use `var`" a `CLAUDE.md`, `naming-conventions.md` y memoria.
- Añadidos `/// <summary>` XML a 52 archivos C#: Data/Context, Data/Town, Data/Attributes/Modifiers, Util/NotificableFields, Managers, UIControllers, AppEvents, ConstAndEnums, Services.

## Files Created

- `Data/Context/TownContext/TownContext.cs`

## Files Modified

- `Data/Context/GameContext/GameContext.cs` — `_townData` → `_townContext`, constructors actualizados
- `Data/Context/AppContext/AppContext.cs` — añadida propiedad `TownContext`, actualizado shortcut `TownData`
- `UIControllers/UIHelpers/Base/BaseUIElement.cs` — eliminado `_rectTransforms`, fix Bug #3
- `ClaudeDocs/agents/roadmap.md` — añadido item Phase 7 debug menu
- `ClaudeDocs/agents/naming-conventions.md` — añadida regla no-var
- `CLAUDE.md` — añadida regla no-var
- 52 archivos con XML doc comments (ver output del agente para lista completa)

## Bugs Fixed

- Bug #3 — `_rectTransforms` siempre null en `BaseUIElement`.
