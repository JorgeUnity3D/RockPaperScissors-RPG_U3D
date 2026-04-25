# Phase 3 — Map & Travel: Plan detallado

> Redactado: 2026-04-25

---

## Contexto

Phase 2 completada en código (2026-03-22): Stables, Stone Smithy, Theater (+ ScrObj refactor), Library.
Phase 3 arranca aquí: conectar el sistema de viaje y preparar el modelo de datos para combate.

---

## El problema central: EnemyDataObject y el código legacy

`MapLevel._levelEnemies` es `List<EnemyDataObject>`. `EnemyDataObject` está en `_oldScriptables/`
y envuelve `Enemy`, que extiende la clase abstracta `Character`. El problema:

- `Character` mezcla **configuración estática** (stats, probabilidades) con **estado de runtime**
  (`currentHealth`, `currentAction`, `currentEnergy`) — no se puede usar como config de nivel
- `Enemy` usa `LanguageDataObject` (otro legacy SO wrapper) y `PlayerOld`
- `Character.cs` y `Enemy.cs` tienen la lógica de combate real (rolls, daño, multiplicadores)
  — **NO se borran**, Phase 4 los evaluará

---

## Pasos

### Paso 1 — Crear `EnemyData` + adaptar `EnemyDataObject` ✅
**Archivos:** `Scripts/Data/Combat/EnemyData.cs`, `Scripts/Data/Combat/EnemyDataObject.cs`

- `EnemyData` — `[Serializable]`, standalone (sin herencia de `Character`). Campos: identity, combat stats, energy, action costs, action probabilities, mentality mod, reward, languages (`List<Language>`).
- `EnemyDataObject` — movido de `_oldScriptables/` a `Scripts/Data/Combat/`. Adaptado al patrón del proyecto: `[SerializeField] private EnemyData _data` + `public EnemyData Data => _data`. Menu: `RPSRPG/Combat/Enemy`.
- `Character.cs`, `Enemy.cs`, `LanguageDataObject.cs` — no se tocan (legacy de referencia).
- `NPCData.cs` — eliminado (nombre incorrecto).

---

### Paso 2 — Actualizar `MapLevel` ✅ / ❌
**Archivo:** `Scripts/Data/Town/Travel/MapLevel.cs`

- Eliminar: `List<EnemyDataObject> _levelEnemies` y propiedad `LevelEnemies`
- Añadir:   `NPCData _npc` y propiedad `Npc`

---

### Paso 3 — Crear `CombatContext` ✅ / ❌
**Archivo:** `Scripts/Data/Context/CombatContext.cs`

Clase C# plain (no `[Serializable]`, no persiste a disco). Vive solo en memoria durante el combate.

```csharp
public class CombatContext
{
    public MapLevel SelectedLevel { get; set; }
}
```

Phase 4 añadirá más campos aquí (HP actual, energía, acción elegida…).

---

### Paso 4 — Actualizar `AppContext` ✅ / ❌
**Archivo:** `Scripts/Data/Context/AppContext/AppContext.cs`

Añadir:
```csharp
public static CombatContext CombatContext { get; set; }
```

Transient — no forma parte de `GameContext`, no se serializa.

---

### Paso 5 — `CreditsTimeCounterManager`: exponer `CreditsLeft` ✅ / ❌
**Archivo:** `Scripts/Managers/02_Town/CreditsTimeCounterManager.cs`

Añadir getter público:
```csharp
public int CreditsLeft => _creditTimeCounter.CreditsLeft;
```

`UseCredit()` ya es público — no cambia.

---

### Paso 6 — Implementar `TravelManager.TravelToLevel()` ✅ / ❌
**Archivo:** `Scripts/Managers/02_Town/TravelManager.cs`

```
1. Cachear CreditsTimeCounterManager en SetUp() via ManagerService.GetManager<T>()
2. Si CreditsLeft == 0 → log de aviso + return (UI "sin créditos" es Phase 7)
3. creditsManager.UseCredit()
4. AppContext.CombatContext = new CombatContext { SelectedLevel = level }
5. ServiceLocator.GetService<SceneService>().LoadScene(GameScenes.COMBAT)
```

---

### Paso 7 — `TravelUIController`: lock/unlock de botones ✅ / ❌
**Archivo:** `Scripts/UIControllers/02_Town/Travel/TravelUIController.cs`

En `SetUpMapUI()`, al iterar niveles:
```csharp
levelButton.interactable = level.IsAvailable;
```

---

### Paso 8 — Eliminar `EnemyDataObject` ✅ / ❌
**Archivo:** `Scripts/_oldScriptables/EnemyDataObject.cs` + `.meta`

Queda huérfano una vez que `MapLevel` no lo referencia.

**NO se toca:** `Enemy.cs`, `Character.cs`, `LanguageDataObject.cs`

---

## Resumen de ficheros

| Archivo | Acción |
|---|---|
| `Data/Town/Travel/NPCData.cs` | NUEVO |
| `Data/Context/CombatContext.cs` | NUEVO |
| `Data/Town/Travel/MapLevel.cs` | Modificar |
| `Data/Context/AppContext/AppContext.cs` | Modificar |
| `Managers/02_Town/TravelManager.cs` | Modificar |
| `Managers/02_Town/CreditsTimeCounterManager.cs` | Modificar |
| `UIControllers/02_Town/Travel/TravelUIController.cs` | Modificar |
| `_oldScriptables/EnemyDataObject.cs` | ELIMINAR |

---

## Pasos manuales en Unity Editor (post-código)

1. Borrar assets `EnemyDataObject` creados previamente (Project panel)
2. Rellenar campo `_npc (NPCData)` en cada `MapLevel` del `MapLevelScrObj.asset`
3. Marcar Level 1 con `_isAvailable = true`

---

## Estado

- [x] Paso 1 — EnemyData + EnemyDataObject
- [x] Paso 2 — MapLevel
- [x] Paso 3 — CombatContext
- [x] Paso 4 — AppContext
- [x] Paso 5 — CreditsTimeCounterManager
- [x] Paso 6 — TravelManager
- [x] Paso 7 — TravelUIController
- [x] Paso 8 — Eliminar EnemyDataObject (hecho al renombrar a EnemyScrObj)
