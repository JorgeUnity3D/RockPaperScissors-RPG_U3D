namespace Kapibara.RPS {
    public enum Actions {
        NONE,
        ROCK,
        PAPER,
        SCISSOR,
        DEFENSE,
        ENERGY
    }

    public enum Languages {
        COMMON,
        ORC,
        ELF,
        DWARF,
        DEV
    }

    public enum ItemType {
        HEALTH_POTION,
        SHURIKEN,
        TORCH
    }

	public enum MapStepType
	{
		COMBAT,
		BOSS,
		TREASURE,
		NPC_RESCUE,
		SURPRISE_BOX
	}

	public enum EnemyId
	{
		ENEMY_0,
		ENEMY_1,
		ENEMY_2,
		ENEMY_3,
		ENEMY_4,
		ENEMY_5,
		ENEMY_6,
		ENEMY_7,
	}

	/// <summary>Animación de entrada de una viñeta al revelarse en el lector de comic.</summary>
	public enum VignetteAnimation
	{
		FADE_IN,
		SLIDE_FROM_LEFT,
		SLIDE_FROM_RIGHT,
		SLIDE_FROM_BOTTOM,
		ZOOM_IN
	}

	/// <summary>
	/// Layout de una página de comic. Determina el prefab de layout instanciado por ComicPlayerUIController.
	/// El índice enum debe coincidir con la posición en el array _layoutPrefabs del controlador (7 elementos, índices 0-6).
	/// </summary>
	public enum ComicPageLayout
	{
		ONE_FULL                = 0,
		TWO_HORIZONTAL          = 1,
		TWO_VERTICAL            = 2,
		THREE_TOP_ONE_BOTTOM_TWO = 3,
		THREE_TOP_TWO_BOTTOM_ONE = 4,
		FOUR_GRID               = 5,
		SIX_GRID                = 6  // 2 filas x 3 columnas, Slot_0...Slot_5
	}
}