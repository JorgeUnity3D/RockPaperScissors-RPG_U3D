# Theater ScrObj Refactor

> Date: 2026-03-22

---

## Qué se hizo

### 1. ComicVignetteSO eliminado — datos inline en ComicPageScrObj

`ComicVignetteSO` (ScriptableObject) reemplazado por `ComicVignetteData` (`[Serializable]`).
Los datos de viñeta ahora viven dentro del asset `ComicPageScrObj`, sin asset separado por viñeta.
`ComicPlayerUIController` no requirió cambios — `.Sprite` y `.Animation` existen en `ComicVignetteData`.

### 2. ComicPageSO → ComicPageScrObj + ComicPageData

- `ComicPageSO.cs` eliminado.
- `ComicPageScrObj.cs` (nuevo): `SerializedScriptableObject`, `[HideLabel] ComicPageData _data`, `Data` property.
- `ComicPageData.cs` (nuevo): `[Serializable]` con `_layout (ComicPageLayout)` + `_vignettes (List<ComicVignetteData>)`.

### 3. ComicStorySO → ComicStoryScrObj + ComicStoryData

- `ComicStorySO.cs` eliminado.
- `ComicStoryScrObj.cs` (nuevo): `SerializedScriptableObject`, `[HideLabel] ComicStoryData _data`, `Data` property.
- `ComicStoryData.cs` (nuevo): `[Serializable]` con `_title`, `_thumbnail`, `[InlineEditor] List<ComicPageScrObj> _pages`.

### 4. TheaterScrObj actualizado al patrón correcto

- Cambiado de `ScriptableObject` a `SerializedScriptableObject`.
- `List<ComicStorySO> _stories` + `Stories` → `[HideLabel, InlineEditor] List<ComicStoryScrObj> _data` + `Data`.
- Con `[InlineEditor]` en ambos niveles, desde `TheaterData.asset` se edita toda la jerarquía inline.

### 5. Consumidores actualizados

- `TheaterManager.cs` — `_theaterScrObj.Stories` → `_theaterScrObj.Data`
- `TheaterUIController.cs` — firma `SetData(List<ComicStoryScrObj> ...)`
- `StoryButton.cs` — `story.Title/Thumbnail` → `story.Data.Title/Thumbnail`
- `ComicPlayerUIController.cs` — tipo `ComicStoryScrObj`, acceso `_currentStory.Data.Pages[i].Data`, `_currentStory.Data.Pages.Count`

---

## Archivos creados

- `Scripts/Data/Town/Theater/ComicVignetteData.cs`
- `Scripts/Data/Town/Theater/ComicPageData.cs`
- `Scripts/Data/Town/Theater/ComicPageScrObj.cs`
- `Scripts/Data/Town/Theater/ComicStoryData.cs`
- `Scripts/Data/Town/Theater/ComicStoryScrObj.cs`

## Archivos modificados

- `Scripts/Data/Town/Theater/TheaterScrObj.cs`
- `Scripts/Managers/02_Town/TheaterManager.cs`
- `Scripts/UIControllers/02_Town/Theater/TheaterUIController.cs`
- `Scripts/UIControllers/02_Town/Theater/StoryButton.cs`
- `Scripts/UIControllers/02_Town/Theater/ComicPlayerUIController.cs`

## Archivos eliminados

- `Scripts/Data/Town/Theater/ComicVignetteSO.cs`
- `Scripts/Data/Town/Theater/ComicStorySO.cs`
- `Scripts/Data/Town/Theater/ComicPageSO.cs`

---

## Acciones manuales pendientes en Unity

- Borrar assets obsoletos: `ComicVignette*.asset` (Story1/), `ComicStory*.asset`, `ComicPage*.asset` — tienen script roto.
- Recrear desde menús: `RPSRPG/Theater/ComicStory`, `RPSRPG/Theater/ComicPage`.
- Asignar en `TheaterData.asset` → editable todo inline desde ese único asset.

---

## Próximo paso

Implementar **Library** para cerrar Phase 2: reescribir `LibraryManager.cs` como `BaseManager`, wiring en `ManagerService`, display-only de quests en `LibraryUIController`.
