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
        //NONE,
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
		Combat,
		Boss,
		Treasure,
		NpcRescue
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
		FadeIn,
		SlideFromLeft,
		SlideFromRight,
		SlideFromBottom,
		ZoomIn
	}

	/// <summary>
	/// Layout de una página de comic. Determina el prefab de layout instanciado por ComicPlayerUIController.
	/// El índice enum debe coincidir con la posición en el array _layoutPrefabs del controlador (7 elementos, índices 0-6).
	/// </summary>
	public enum ComicPageLayout
	{
		One_Full                = 0,
		Two_Horizontal          = 1,
		Two_Vertical            = 2,
		Three_TopOne_BottomTwo  = 3,
		Three_TopTwo_BottomOne  = 4,
		Four_Grid               = 5,
		Six_Grid                = 6  // 2 filas x 3 columnas, Slot_0...Slot_5
	}
}