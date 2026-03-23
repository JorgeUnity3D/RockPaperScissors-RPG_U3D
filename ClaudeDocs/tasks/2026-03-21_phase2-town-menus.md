# Phase 2 — Wire the Four Blocked Town Menus

> Started: 2026-03-21

---

## Objetivo

Implementar los 4 menús de ciudad que estaban bloqueados: Stables, Stone Smithy, Theater, Library.

---

## 1. Stables ✅ DONE

### Qué se hizo
- Clarificado que los "caballos" = créditos gestionados por `CreditsTimeCounterManager`.
- `StablesManager` es la UI de monetización/IAP, no la gestión de créditos.
- `WatchAd_Button` → `AppEvents.OnEarnCredit` (nuevo evento) + `TownData(STABLES).Experience++`
- `BuyGame_Button` → placeholder TODO (Phase 7)
- EXP capado al techo del nivel actual; level-up diferido a Phase 6.

### Archivos modificados
- `AppEvents.cs` — añadido `OnEarnCredit`
- `CreditsTimeCounterManager.cs` — suscribe a `OnEarnCredit`
- `StablesUIController.cs` — `SetData(UnityAction, UnityAction)`; wira botones; sin progress bar (lo muestra `InMenuUIController`)
- `StablesManager.cs` — `SetUp` + `Initialize` + `WatchAd` + `BuyGame` + `AddStablesExp`
- `roadmap.md` — actualizado con arquitectura Credits/Stables + nota InMenuUIController reactivo

### Gotchas / follow-up
- `InMenuUIController` (Slider de nivel) no se actualiza reactivamente al ganar EXP en runtime. Documentado en roadmap → Phase 6: añadir `AppEvents.OnBuildingExpUpdated` + `InMenuUIController.RefreshLevel(float)`.
- Wiring en prefab `Stables_UIController`: hecho por el dev.

---

## 2. Stone Smithy ✅ DONE

### Qué se hizo
- 3 botones renombrados en prefab: `Buy→Upgrade` (`UpgradeAttackRune_Button`, `UpgradeHealthRune_Button`, `UpgradeEnergyRune_Button`)
- `GameConsts.STONE_SMITHY_COST_PER_LEVEL = 10` añadido
- `Item.cs` refactorizado: `[Serializable]`, eliminados `currentExp`/`button`, `int amount` → `List<int> amountsPerLevel`, `[JsonIgnore] Sprite icon`, añadido `Clone()`
- `StoneSmithyData.cs` (nuevo) + `StoneSmithyScrObj.cs` (nuevo) — ScriptableObject con los 3 items de config
- `Player.cs` — backpack renombrado a `_attackItemLevel/HealItemLevel/EnergyItemLevel (NInt)`; propiedades `AttackItemLevel/HealItemLevel/EnergyItemLevel`
- `StoneSmithyUIController`: `SetData(3×Item, 3×UnityAction)` + `RefreshAttackLevel/HealLevel/EnergyLevel(int)` + `SetupButton` via `GetComponent<Button>()`
- `StoneSmithyManager`: clona items del SO en `Initialize()`, inyecta nivel desde Player, `TrySpendGold` + `Upgrade*`, fórmula `(nivelActual+1)×STONE_SMITHY_COST_PER_LEVEL`

### Arquitectura
SO = toda la config del item (nombre, tipo, icono, `amountsPerLevel`). Player = solo el nivel (NInt). El mismo patrón aplica en combate (Phase 5): SO como referencia + nivel del contexto.

### Archivos modificados
- `StoneSmithy_UIController.prefab` — botones renombrados
- `GameConsts.cs` — `STONE_SMITHY_COST_PER_LEVEL`
- `Item.cs` — refactor completo
- `StoneSmithyData.cs` (nuevo), `StoneSmithyScrObj.cs` (nuevo)
- `Player.cs` — campos backpack renombrados
- `StoneSmithyUIController.cs` — implementación completa
- `StoneSmithyManager.cs` — implementación completa

### Gotchas / follow-up
- `StoneSmithyButton.selectionOverlay` no se usa (siempre `false`). Revisitar si se añade flujo select+confirm.
- Wiring Inspector pendiente: crear asset SO desde `RPSRPG/StoneSmithyData`; asignar `_stoneSmithyScrObj` en prefab de `StoneSmithyManager`; asignar 3 `StoneSmithyButton` en `StoneSmithy_UIController`.

---

## 3. Theater ✅ DONE

### Qué se hizo

**Datos:**
- `ComicVignetteSO` — ScriptableObject por viñeta (sprite, dialogText, VignetteAnimation)
- `ComicPageSO` — ScriptableObject por página (ComicPageLayout, lista de viñetas)
- `ComicStorySO` — ScriptableObject por historia (título, thumbnail, lista de páginas)
- `TheaterScrObj` — SO maestro con la lista de historias del juego

**Enums** añadidos a `RPSEnums.cs`:
- `VignetteAnimation` (FadeIn, SlideFromLeft, SlideFromRight, SlideFromBottom, ZoomIn)
- `ComicPageLayout` (One_Full=0 … Four_Grid=5 — el índice mapea al array `_layoutPrefabs` del controlador)

**Utilidad:**
- `UITween.cs` (namespace `Kapibara.Util`) — wrapper estático DOTween: FadeIn, FadeOut, SlideFrom, ZoomIn

**Player.cs:**
- Campo `_unlockedStoriesCount (NInt)`, propiedad `UnlockedStoriesCount`, valor inicial 1
- Añadido `int unlockedStoriesCount = 1` al JsonConstructor (saves antiguas por defecto a 1)

**AppEvents.cs:**
- `OnStorySelected (UnityAction<int>)` — TheaterUIController → TheaterManager
- `OnComicClosed (UnityAction)` — ComicPlayerUIController → TheaterManager

**UIControllers:**
- `ComicPlayerUIController` — reescrito como UIController (era MonoBehaviour con código Doozy comentado). Overlay sobre todo. Instancia prefab de layout, encuentra slots por nombre "Slot_N", anima con UITween.
- `TheaterUIController` — galería de botones de historia. SetData(stories, unlockedCount) → instancia StoryButtons.
- `StoryButton` — componente helper del botón (Button + Image thumbnail + TMP_Text título + lockOverlay)

**TheaterManager:**
- `SetUp()` — obtiene TheaterUIController y ComicPlayerUIController vía UIService
- `Subscribe/UnSubscribe` — OnStorySelected + OnComicClosed
- `Initialize()` — llama TheaterUIController.SetData(stories, Player.UnlockedStoriesCount)
- `PlayStory(int index)` — valida rango y desbloqueo, llama ComicPlayerUIController.SetData()

### Arquitectura
`TheaterScrObj` (SO) = config completa de historias. `Player.UnlockedStoriesCount` (NInt) = progreso. `TheaterManager` orquesta. `ComicPlayerUIController` es overlay independiente gestionado por el manager (no por TownManager).

### Prefabs — wiring pendiente por el dev
- `TheaterScrObj` asset: crear desde `RPSRPG/Theater/TheaterData`, asignar en `TheaterManager._theaterScrObj`
- `_layoutPrefabs[6]` en `ComicPlayerUIController`: crear 6 prefabs con hijos `Slot_0`, `Slot_1`... (Image + CanvasGroup cada uno)
- `_storyButtonPrefab` en `TheaterUIController`: prefab con `StoryButton` (Button + Image + TMP_Text + lockOverlay GameObject)
- Crear assets `ComicVignetteSO`, `ComicPageSO`, `ComicStorySO` para las historias reales

### Gotchas / follow-up
- `SlideFromBottom` usa `SlideDirection.Down` en UITween (el offset negativo en Y mueve el slot hacia abajo, luego sube al origen — efecto "entra desde abajo").
- `Player.UnlockedStoriesCount` dispara un save write en cada incremento (NotificableField). Es correcto — solo ocurre al derrotar un jefe.
- `ManagerService` ya tenía THEATER cableado — no requirió cambios.
- Dialog text de viñetas no renderizado en esta fase: campo existe en `ComicVignetteSO` pero no se muestra en los slots. Añadir TMP_Text a los prefabs de slot y leer `Vignettes[i].DialogText` en `ShowPage()` (Phase 7 polish).

---

## 5. Mejoras Theater (2026-03-21, sesión tarde) ✅ DONE

### Qué se hizo

**GameManager cleanup:**
- Eliminados `_runTest`, `_runCleanTest` y `RunTest()` — flujo de test hardcodeado innecesario.
- Eliminado `using UnityEngine.Serialization` huérfano.

**ComicPageLayout — nuevo layout:**
- Añadido `Six_Grid = 6` al enum (`RPSEnums.cs`) — 2 filas × 3 columnas, Slot_0…Slot_5.

**Editor tool — ComicLayoutCreator:**
- `Scripts/Util/Editor/ComicLayoutCreator.cs` (nuevo) — menú `Kapibara/UI/Create Comic Layouts`.
- Crea los 7 GameObjects de layout en escena con anclas exactas por slot. El dev los arrastra y hace prefab.

**GameConsts — constantes de tweening:**
- `COMIC_VIGNETTE_DURATION` y `COMIC_SLIDE_DISTANCE` movidas de `ComicPlayerUIController` a `GameConsts.cs` (región `#region COMIC_PLAYER`).

**ComicLayoutEntry — wrapper Layout + Prefab:**
- `Scripts/UIControllers/02_Town/Theater/ComicLayoutEntry.cs` (nuevo) — clase `[Serializable]` con `ComicPageLayout Layout` + `GameObject Prefab`.
- `ComicPlayerUIController` cambia de `GameObject[] _layoutPrefabs` (dependiente de orden) a `List<ComicLayoutEntry> _layouts` (busca por enum).

**Player — UnlockedStoryIds:**
- `NInt _unlockedStoriesCount` reemplazado por `List<int> _unlockedStoryIds`.
- Expone `UnlockedStoryIds`, `IsStoryUnlocked(int)`, `UnlockStory(int)` (este último dispara save manualmente).
- Saves antiguos cargan sin romper — JsonConstructor defaultea a `{ 0 }` si el campo es null.
- `TheaterManager` y `TheaterUIController` actualizados para usar IDs en lugar de count.

### Archivos modificados
- `GameManager.cs`
- `RPSEnums.cs`
- `ComicLayoutCreator.cs` (nuevo)
- `GameConsts.cs`
- `ComicLayoutEntry.cs` (nuevo)
- `ComicPlayerUIController.cs`
- `Player.cs`
- `TheaterManager.cs`
- `TheaterUIController.cs`

### Wiring pendiente en Inspector (Theater)
- `ComicPlayer_UIController` prefab: borrar `_resetButton` (obsoleto), asignar `_closeButton`, `_pageContainer`, y la lista `_layouts` con los 7 entries.
- Crear los 7 prefabs de layout desde los GameObjects generados por `ComicLayoutCreator`.

---

## 4. Library — PENDIENTE
