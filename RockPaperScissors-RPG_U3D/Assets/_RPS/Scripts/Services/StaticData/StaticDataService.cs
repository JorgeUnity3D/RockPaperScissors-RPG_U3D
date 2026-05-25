using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Repositorio central de ScriptableObjects estáticos del juego.
	/// Pertenece a GameCore (DontDestroyOnLoad) y se registra en el ServiceLocator.
	/// Cualquier manager lo obtiene con ServiceLocator.Instance.GetService<StaticDataService>().
	/// </summary>
	[DefaultExecutionOrder(-9989)]
	public class StaticDataService : ServiceSubscriber<StaticDataService>
	{
		[Header("Town")]
		[SerializeField] private TownViewScrObj           _townViews;
		[SerializeField] private LibraryScrObj            _libraryQuests;
		[SerializeField] private CreditsTimeCounterScrObj _creditsTimeCounter;
		[SerializeField] private TheaterScrObj            _theaterData;
		[SerializeField] private StoneSmithyScrObj        _stoneSmithyItems;
		[SerializeField] private PaperTreeScrObj          _paperTreeSkillTrees;

		[Header("Map")]
		[SerializeField] private MapLevelScrObj           _mapLevels;

		[Header("Player")]
		[SerializeField] private IconsScrObj              _statIcons;

		[Header("Combat")]
		[SerializeField] private LanguageScrObj           _commonLanguage;

		public TownViewScrObj           TownViews           => _townViews;
		public LibraryScrObj            LibraryQuests       => _libraryQuests;
		public CreditsTimeCounterScrObj CreditsTimeCounter  => _creditsTimeCounter;
		public TheaterScrObj            TheaterData         => _theaterData;
		public StoneSmithyScrObj        StoneSmithyItems    => _stoneSmithyItems;
		public PaperTreeScrObj          PaperTreeSkillTrees => _paperTreeSkillTrees;
		public MapLevelScrObj           MapLevels           => _mapLevels;
		public IconsScrObj              StatIcons           => _statIcons;
		public LanguageScrObj           CommonLanguage      => _commonLanguage;
	}
}
