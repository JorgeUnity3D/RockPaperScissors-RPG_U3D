# Phase 8 — LoggerService, DebugService, Roadmap Cleanup

**Date:** 2026-05-26 (session 5)

## What was done

### Roadmap cleanup
- Town Locations table: Stone Smithy, Library, Paper Tree actualizados de ❌/⚠️ a ✅ con descripción real.
- Section 2 (Partially Implemented): Paper Tree actualizado de ⚠️ a ✅, nota sobre List<SkillNode> y cross-stat fix.
- Phase 2: Stone Smithy task marcado ✅ DONE.
- Añadida sección Phase 8 al final del roadmap con todos los items de esta sesión.

### LoggerService (nuevo)
- Extiende `ServiceSubscriber<LoggerService>` — execution order -9989.
- En `Awake`: crea el archivo `RPSLogs/rpslog-yyyymmdd-HHmm.txt` y suscribe a `Application.logMessageReceived`.
- Path del archivo: `{projectRoot}/RPSLogs/` en editor, `{persistentDataPath}/RPSLogs/` en build.
- Header decorado (box de ═) con fecha, plataforma y versión de Unity.
- Formato por línea: `[HH:mm:ss.fff] [LOG  ] message` / `[WARN ]` / `[ERROR]` / `[EXCP ]` / `[ASRT ]`.
- Stack trace incluida (con prefijo `→`) solo para Error, Exception y Assert.
- Footer con timestamp al cerrar (OnApplicationQuit + OnDestroy). Guard `_footerWritten` para evitar doble escritura.
- `AutoFlush = true` en el StreamWriter — sin riesgo de perder logs en crash.

### DebugService (nuevo)
- Extiende `ServiceSubscriber<DebugService>` — persiste entre escenas en GameCore.
- `[SerializeField, ReadOnly] DebugUIController _debugUIController` — hijo en prefab, asignado en Inspector.
- `SetUp`: cachea `PaperTreeScrObj` y `StoneSmithyScrObj` de `StaticDataService`.
- Guard `IsReady(caller)` — todos los métodos comprueban que hay contexto de juego antes de actuar.
- `ShowDebugUI()` / `HideDebugUI()` / `ToggleDebugUI()` para control de UI.
- Operaciones por categoría (`[FoldoutGroup]` de Odin para uso en editor):
  - **Economy**: `AddGold(amount)`, `SetGold(amount)`
  - **Player Level**: `ForceLevelUp()`, `ResetPlayerLevel()` — idéntico a los [Button] de ScissorBonfireManager
  - **Items**: `MaxAllItems()`, `ResetAllItems()`
  - **Paper Tree**: `UnlockAllPaperTreeNodes()`, `ResetAllPaperTreeNodes()`
  - **Stories**: `UnlockAllStories()` (usa `Player.UnlockStory()`), `LockAllStories()`
  - **Library**: `CompleteAllLibraryQuests()` (aplica `RewardStat`+`RewardAmount` via `LibraryModifier.Modifier +=`), `ResetLibraryQuests()`
  - **Town**: `UnlockAllBuildings()`, `RescueAllNPCs()`, `ResetAllBuildings()`

### DebugUIController (nuevo — IMGUI, sin prefab)
- Extiende `MonoBehaviour` directamente (no UIController — no necesita Canvas ni prefab).
- `OnGUI()` dibuja siempre un botón `[DBG]` en esquina superior derecha.
- Al pulsarlo, abre `GUILayout.Window` arrastrable con scroll interno.
- Obtiene referencia a `DebugService` vía `GetComponentInParent<DebugService>()` en Awake.
- Métodos públicos: `Show()`, `Hide()`, `Toggle()` (usables desde gestos externos o DebugService).
- Header de la ventana: escena activa y oro actual del jugador.
- Secciones: Economy (con campo de texto para importe), Player Level, Items, Paper Tree, Stories, Library, Town.
- Sin prefab — basta con añadir el componente al GameObject de DebugService en GameCore.

## Files changed
- `ClaudeDocs/agents/roadmap.md` — stale entries corregidos + sección Phase 8 añadida
- `Scripts/Services/Logger/LoggerService.cs` (nuevo)
- `Scripts/Services/Debug/DebugService.cs` (nuevo)
- `Scripts/UIControllers/Debug/DebugUIController.cs` (nuevo)

## Follow-up needed
- Añadir `DebugService` y `LoggerService` al prefab GameCore en el Inspector de Unity.
- Crear `DebugUIController` prefab como hijo de DebugService en GameCore.
- Asignar `_debugUIController` en el Inspector de DebugService.
- Conectar un gesto (p.ej. triple tap en esquina) a `DebugService.ToggleDebugUI()` para uso en builds.
- Considerar gate `#if DEVELOPMENT_BUILD || UNITY_EDITOR` para DebugService en el futuro.
