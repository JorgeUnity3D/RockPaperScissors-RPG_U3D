# Dependencies

## Unity Version
`6000.3.8f1` (Unity 6, revision 1c7db571dde0)

---

## Unity Package Manager (manifest.json)

| Package | Version | Notes |
|---|---|---|
| com.unity.feature.2d | 2.0.2 | Meta-package pulling 2D modules |
| com.unity.ugui | 2.0.0 | uGUI (Canvas, Button, Image, etc.) |
| com.unity.inputsystem | 1.18.0 | New Input System |
| com.unity.ide.rider | 3.0.39 | Rider IDE integration |
| com.unity.ide.visualstudio | 2.0.27 | VS integration |
| com.unity.test-framework | 1.6.0 | Unity Test Runner (NUnit) |
| com.unity.modules.animation | 1.0.0 | Animator/mecanim |
| com.unity.modules.audio | 1.0.0 | AudioSource/AudioClip |
| com.unity.modules.physics2d | 1.0.0 | 2D physics |
| com.unity.modules.ui | 1.0.0 | Core UI module |
| com.unity.modules.uielements | 1.0.0 | UIToolkit |
| com.unity.modules.video | 1.0.0 | VideoPlayer |
| *(other standard modules)* | 1.0.0 | physics, ai, tilemap, etc. — not actively used |

---

## Assets/Plugins/ (DLL assets, not UPM)

### DOTween (Demigiant)
- **Path**: `Assets/Plugins/Demigiant/DOTween/`
- **Version**: Not specified in readme; readme copyright 2014-2018. Likely DOTween v1.x (free tier).
- **Usage**: `BaseUIElement` uses `canvasGroup.DOFade()` and `canvasGroup.DOKill()` for show/hide animations. Import: `using DG.Tweening`.

### Odin Inspector (Sirenix)
- **Path**: `Assets/Plugins/Sirenix/Odin Inspector/`
- **Version**: Not pinned in any local file (no changelog present).
- **Usage**: Pervasive. Used for editor layout (`[ReadOnly]`, `[SerializeField]`, `[HorizontalGroup]`, `[VerticalGroup]`, `[LabelWidth]`, `[PreviewField]`, `[InlineEditor]`). `ServiceLocator` inherits from `SerializedMonoBehaviour` via `SingletonMonoBehaviour`. `ServiceDictionary` and `ManagerDictionary` likely extend `SerializedDictionary` from Odin. `DataEditorWindow` uses Odin editor utilities.
- **Note**: `Character.cs` uses Odin layout attributes heavily — these are editor-only display, not runtime-affecting.

---

## Assets/_ThirdParty/ (imported asset packages)

### TextMesh Pro
- **Path**: `Assets/_ThirdParty/TextMesh Pro/`
- **Version**: Bundled with Unity 6 (TMP is now included by default; local copy suggests it was imported manually).
- **Usage**: Text rendering throughout UI. Not directly referenced in C# imports visible in _RPS scripts (used via prefabs/scene references).

### Doozy UI Manager
- **Path**: `Assets/_ThirdParty/Doozy/`
- **Version**: Not pinned in local files.
- **Usage**: Presence confirmed by folder. Not directly imported in any _RPS C# script via `using` statements visible in the codebase. May be used in prefabs/scenes for UI animation or signal routing.

### JsonDotNet (Newtonsoft.Json for Unity)
- **Path**: `Assets/_ThirdParty/JsonDotNet/`
- **Source zip**: `JsonDotNet201Source.zip` → Newtonsoft.Json 2.0.1 (Unity port).
- **Usage**: Critical — used in `PersistenceService`, `GameContext`, `Player`, `StatAttribute`, `BaseModifier` subclasses, and `BaseModifierConverter`. All save/load serialization goes through `JsonConvert.SerializeObject` / `JsonConvert.DeserializeObject`.

### ConsolePro
- **Path**: `Assets/_ThirdParty/ConsolePro/`
- **Version**: Unknown.
- **Usage**: Enhanced Unity console for debugging. No C# import visible in _RPS scripts; editor-only tool.

---

## Implicit Dependencies / Assumptions
- `System.IO` — used directly in `PersistenceService` for file operations.
- `UnityEngine.SceneManagement` — used in `GameManager` and `SceneService`.
- `System.Drawing.Color` — imported in `Character.cs` (unused — likely leftover).
