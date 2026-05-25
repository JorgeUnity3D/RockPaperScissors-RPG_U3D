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

	public enum GambitType
	{
		PRIMARY,    // antes de action roll Y mentality roll; obligatorio; cancela el resto
		SECONDARY,  // antes de action roll; puede ser sobreescrito por tertiary
		TERTIARY    // tras mentality roll; solo actúa cuando el enemigo lee la mente del jugador
	}

	public enum GambitCondition
	{
		ALWAYS,              // siempre se activa
		ENERGY_ZERO,         // CurrentEnergy == 0
		ENERGY_BELOW,        // CurrentEnergy < threshold
		HP_BELOW_PERCENT,    // CurrentHealth < MaxHealth * hpPercent
		ROUND_EQUALS,        // currentRound == threshold
		ROUND_GE,            // currentRound >= threshold
		PLAYER_ACTION_IS,    // playerAction == matchAction  (solo Tertiary)
		PLAYER_ACTION_IS_NOT // playerAction != matchAction  (solo Tertiary)
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

	/// <summary>Animación de salida de una viñeta cuando la página completa avanza a la siguiente.</summary>
	public enum VignetteExitAnimation
	{
		NONE,
		FADE_OUT,
		SLIDE_TO_LEFT,
		SLIDE_TO_RIGHT,
		SLIDE_TO_TOP,
		SLIDE_TO_BOTTOM,
		ZOOM_OUT
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