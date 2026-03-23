# Phase 1 — Finalize the Data Model

---

## Items

### 1. Bug #15 — PaperTreeModifier single bool ✅ FIXED 2026-03-20
`bool SkillTreeData` reemplazado por `List<SkillNode> UnlockedNodes`. Ver `2026-03-20_architecture-uicontroller-cleanup.md`.

---

### 2. InitialEnergy vs BaseEnergy ⏸️ DEFER to Phase 4

**Decisión:** No definir ahora. Se resolverá con la implementación del combate.

**Contexto conocido:**
- `_currentEnergy (NInt)` — energía actual durante el combate, runtime.
- `_initialEnergy (NInt, base 10)` — energía con la que arranca cada combate. Equivale a `initialEnergy` del legacy `Character`/`Enemy`.
- `_baseEnergy (NAttribute, Stats.ENERGY_BASE)` — stat modificable (tiene ScissorBonfireModifier). Función exacta desconocida. Hipótesis: tick pasivo de energía por ronda (distinto de EnergyRecovery que es activo al elegir acción Energía). Se confirmará durante Phase 4.
- `_energyRecovery (NAttribute, Stats.ENERGY_RECOVERY, base 5)` — energía recuperada al elegir la acción Energía activamente.
- El techo del superataque es siempre 100 (hardcodeado en Enemy legacy, no es un stat).

---

### 3. Backpack / consumibles en Player ✅ DONE 2026-03-21

**Diseño del doc (sección 5.4 Herrería de la Piedra):**
- La mochila se desbloquea al construir la Herrería Y rescatar al aldeano.
- Contiene **3 objetos consumibles**, uno de cada tipo:
  - Objeto 1: daño instantáneo
  - Objeto 2: curación instantánea
  - Objeto 3: rellena energía instantáneamente
- Se recarga automáticamente tras cada RUN.
- Cada objeto tiene nivel (upgradeable en la Herrería). Nivel edificio sube 1 por cada 3 mejoras compradas (max nivel 10).
- Los objetos producen efecto inmediato en combate y no pueden usarse de nuevo en la misma RUN.

**Implementado en `Player.cs`:**
- 3 fields `NInt`: `_consumableItemDamage`, `_consumableItemHeal`, `_consumableItemEnergy` (0 = no desbloqueado).
- 3 properties públicas con XML doc: `ConsumableItemDamage`, `ConsumableItemHeal`, `ConsumableItemEnergy`.
- Constructor `Player(string)`: init a 0.
- `[JsonConstructor]`: 3 params opcionales con default 0 — backward compat con saves viejos.
- La disponibilidad en combate (usado/no-usado esta run) es estado de runtime, no persistido — se implementará en `CombatContext` en Phase 4.

---

### 4. HasNpc vs NpcUnlocked en TownData ✅ DONE (XML docs añadidos 2026-03-21)

**Definición acordada:**
- `HasNpc` — el menú de este edificio tiene un NPC asociado. Se activa cuando se construye el edificio. Controla si `InMenuUIController` muestra `_menuNPCHolder`.
- `NpcUnlocked` — el NPC de este edificio ha sido rescatado durante los combates. Se activa como resultado de un evento en el mapa de niveles.

**Contexto del sistema de niveles:**
Cada nivel es una secuencia de eventos (combates, tesoros, NPC a rescatar). Cuando el jugador completa el evento de rescate correspondiente a un edificio, se setea `TownData.NpcUnlocked = true`.
