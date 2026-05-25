using Kapibara.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager principal de la escena Town. Gestiona la navegación entre edificios, el desbloqueo de nuevos menús y la actualización de oro del jugador.
	/// </summary>
	public class TownManager : BaseManager
	{
		private TownViewScrObj _townViewScrObj;

		[Header("DEBUG")]
		[SerializeField, ReadOnly] private List<TownData> _townData;
		[SerializeField, ReadOnly] private List<TownView> _townViews;
		[SerializeField, ReadOnly] private UIService _uiService;
		[SerializeField, ReadOnly] private ManagerService _managerService;
		[SerializeField, ReadOnly] private CreditsTimeCounterManager _creditsTimeCounterManager;
		[SerializeField, ReadOnly] private TownUIController _townUIController;
		[SerializeField, ReadOnly] private PlayerUIController _playerUIController;
		[SerializeField, ReadOnly] private UnlockMenuUIController _unlockMenuUIController;
		[SerializeField, ReadOnly] private TravelUIController _travelUIController;
		[SerializeField, ReadOnly] private HouseUIController _houseUIController;
		[SerializeField, ReadOnly] private InMenuUIController _inMenuUIController;

		private BaseUIElement _currentTownUIController;
		private TownMenu      _currentOpenMenu;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[TownManager] SetUp() -> ");
			_uiService = ServiceLocator.Instance.GetService<UIService>();
			_managerService = ServiceLocator.Instance.GetService<ManagerService>();
			_creditsTimeCounterManager = _managerService.GetManager<CreditsTimeCounterManager>();
			_playerUIController = _uiService.GetController<PlayerUIController>();
			_unlockMenuUIController = _uiService.GetController<UnlockMenuUIController>();
			_townUIController = _uiService.GetController<TownUIController>();
			_inMenuUIController = _uiService.GetController<InMenuUIController>();
			_townViewScrObj = ServiceLocator.Instance.GetService<StaticDataService>().TownViews;
			_townData = AppContext.TownData;
			_townViews = _townViewScrObj.Data;
		}

		protected override void Subscribe()
		{
			Debug.Log($"[TownManager] Subscribe() -> ");
			AppContext.Player.OnGoldValueChanged += UpdatePlayerGold;
			AppEvents.OnOpenTownMenu += OpenTownMenu;
			AppEvents.OnBackFromTownMenu += BackFromTownMenu;
			AppEvents.OnConfirmUnlock += UnlockTownMenu;
			AppEvents.OnBuildingExpUpdated += OnBuildingExpUpdated;
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[TownManager] UnSubscribe() -> ");
			AppContext.Player.OnGoldValueChanged -= UpdatePlayerGold;
			AppEvents.OnOpenTownMenu -= OpenTownMenu;
			AppEvents.OnBackFromTownMenu -= BackFromTownMenu;
			AppEvents.OnConfirmUnlock -= UnlockTownMenu;
			AppEvents.OnBuildingExpUpdated -= OnBuildingExpUpdated;
		}

		#endregion

		#region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[TownManager] Initialize() -> ");
			_townUIController.SetData(_townData, _townViewScrObj.Data);
			_playerUIController.UpdatePlayerGold(AppContext.Player.Gold);
			_creditsTimeCounterManager.Initialize();
		}

		private void OpenTownMenu(TownMenu townMenu)
		{
			Debug.Log($"[TownManager] OpenTownMenu() -> TownMenu: {townMenu}");
			TownData townData = _townData.Find(tv => tv.TownMenu == townMenu);
			if (townData == null)
			{
				Debug.Log($"[TownManager] OpenTownMenu() -> Error: No TownView with {townMenu} flag");
				return;
			}
			TownView townView = _townViews.Find(td => td.TownMenu == townData.TownMenu);
			if (townData.IsUnlocked)
			{
				GoToTownMenu(townData, townView);
			}
			else
			{
				GoToUnlockMenu(townData, townView);
			}
		}

		private void GoToTownMenu(TownData townData, TownView townView)
		{
			Debug.Log($"[TownManager] GoToTownMenu() -> TownView: {townData.TownMenu}");

			_currentOpenMenu = townData.TownMenu;

			if (townView.HasNpc && !townData.NpcUnlocked)
			{
				_currentTownUIController = null;
				_inMenuUIController.ShowCanvas();
				_inMenuUIController.SetData(townData, townView);
				return;
			}

			TownMenu townMenu = townData.TownMenu;
			BaseManager targetManager = _managerService.GetManager(townMenu);
			_currentTownUIController = _uiService.GetController(townMenu);

			if (_currentTownUIController == null || targetManager == null)
			{
				string uiController = _currentTownUIController == null ? "UIController" : "";
				string manager = targetManager == null ? "Manager" : "";
				Debug.Log($"[TownManager] GoToTownMenu() -> Couldn't find {uiController} {manager} for {townData.TownMenu}");
				return;
			}

			_currentTownUIController.ShowCanvas();
			if (targetManager is ITownBuilding building)
				building.OnMenuOpen();
			_inMenuUIController.ShowCanvas();
			_inMenuUIController.SetData(townData, townView);
		}

		private void GoToUnlockMenu(TownData townData, TownView townView)
		{
			Debug.Log($"[TownManager] GoToUnlockMenu() -> TownView: {townData.TownMenu} - Cost: {townData.Cost}");
			_unlockMenuUIController.ShowCanvas();
			_unlockMenuUIController.SetData(townData, townView, AppContext.Player.Gold >= townData.Cost);
		}

		private void BackFromTownMenu()
		{
			Debug.Log($"[TownManager] BackFromTownMenu() -> ");
			_currentTownUIController?.HideCanvas();
			_inMenuUIController.HideCanvas();
			_currentOpenMenu = default;
		}

		private void OnBuildingExpUpdated(TownMenu townMenu)
		{
			if (townMenu != _currentOpenMenu) return;
			TownData townData = _townData.Find(td => td.TownMenu == townMenu);
			TownView townView = _townViews.Find(tv => tv.TownMenu == townMenu);
			if (townData != null && townView != null)
				_inMenuUIController.SetData(townData, townView);
		}

		private void UnlockTownMenu(TownData townData)
		{
			Debug.Log($"[TownManager] UnlockTownMenu() -> TownView: {townData.TownMenu} - Cost: {townData.Cost}");
			AppContext.Player.Gold = Mathf.Max(0, AppContext.Player.Gold - townData.Cost);
			townData.IsUnlocked = true;
			TownView townView = _townViews.Find(td => td.TownMenu == townData.TownMenu);
			_townUIController.UpdateTownButton(townData, townView);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		private void UpdatePlayerGold(int currentGold)
		{
			_playerUIController.UpdatePlayerGold(currentGold);
		}

		#endregion

		#region DEBUG

		[FoldoutGroup("DEBUG"), Button("Buy Building")]
		private void Debug_BuyBuilding(TownMenu townMenu)
		{
			TownData townData = _townData.Find(td => td.TownMenu == townMenu);
			if (townData == null) return;
			townData.IsUnlocked = true;
			TownView townView = _townViews.Find(tv => tv.TownMenu == townMenu);
			_townUIController.UpdateTownButton(townData, townView);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		[FoldoutGroup("DEBUG"), Button("Rescue NPC")]
		private void Debug_RescueNpc(TownMenu townMenu)
		{
			TownData townData = _townData.Find(td => td.TownMenu == townMenu);
			if (townData == null) return;
			townData.NpcUnlocked = true;
			TownView townView = _townViews.Find(tv => tv.TownMenu == townMenu);
			_townUIController.UpdateTownButton(townData, townView);
			AppEvents.OnGameContextUpdated?.Invoke();
		}

		#endregion
	}
}