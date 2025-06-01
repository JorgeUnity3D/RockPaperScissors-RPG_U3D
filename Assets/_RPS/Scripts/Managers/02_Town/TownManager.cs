using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kapibara.RPS
{
	public class TownManager : BaseManager
	{
		[SerializeField] private TownViewScrObj _townViewScrObj;
		[SerializeField, ReadOnly] private List<TownData> _townData;
		[SerializeField, ReadOnly] private List<TownView> _townViews;
		
		[SerializeField, ReadOnly] private UIService _uiService;
		[SerializeField, ReadOnly] private ManagerService _managerService;
		[SerializeField, ReadOnly] private TownUIController _townUIController;
		[SerializeField, ReadOnly] private PlayerUIController _playerUIController;
		[SerializeField, ReadOnly] private UnlockMenuUIController _unlockMenuUIController;
		[SerializeField, ReadOnly] private TravelUIController _travelUIController;
		[SerializeField, ReadOnly] private HouseUIController _houseUIController;
		[SerializeField, ReadOnly] private InMenuUIController _inMenuUIController;

		private BaseUIController _currentTownUIController;
		
		#region SETUP
		
		public override void SetUp()
		{
			Debug.Log($"[TownManager] SetUp() -> ");
			_uiService = ServiceLocator.Instance.GetService<UIService>();
			_managerService = ServiceLocator.Instance.GetService<ManagerService>();
			_playerUIController = _uiService.GetController<PlayerUIController>();
			_unlockMenuUIController = _uiService.GetController<UnlockMenuUIController>();
			_townUIController = _uiService.GetController<TownUIController>();
			_inMenuUIController = _uiService.GetController<InMenuUIController>();
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
		}
		
		protected override void UnSubscribe()
		{
            Debug.Log($"[TownManager] UnSubscribe() -> ");
            AppContext.Player.OnGoldValueChanged -= UpdatePlayerGold;
            AppEvents.OnOpenTownMenu -= OpenTownMenu;
            AppEvents.OnBackFromTownMenu -= BackFromTownMenu;
            AppEvents.OnConfirmUnlock -= UnlockTownMenu;
		}

        #endregion

        #region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[TownManager] Initialize() -> ");
			_townUIController.SetData(_townData, _townViewScrObj.Data);
			_playerUIController.UpdatePlayerGold(AppContext.Player.Gold);
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
			TownView townView = _townViews.Find(td => td.townMenu == townData.TownMenu);
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
			TownMenu townMenu = townData.TownMenu;
			BaseManager targetManager = _managerService.GetManager(townMenu);
			_currentTownUIController = _uiService.GetController(townMenu);

			if (_currentTownUIController == null || targetManager == null)
			{
				Debug.Log($"[TownManager] GoToTownMenu() -> Couldn't find Manager or UIController for {townData.TownMenu}");
				return;
			}

			_currentTownUIController.ShowCanvas();
			targetManager.Initialize();
			_inMenuUIController.ShowCanvas();
			_inMenuUIController.SetData(townData, townView);
		}
		
		private void GoToUnlockMenu(TownData townData, TownView townView)
		{
			Debug.Log($"[TownManager] GoToUnlockMenu() -> TownView: {townData.TownMenu} - Cost: {townData.Cost}");
			_unlockMenuUIController.ShowCanvas();
			_unlockMenuUIController.SetData(townData, townView);
		}
		
		private void BackFromTownMenu()
		{
			Debug.Log($"[TownManager] BackFromTownMenu() -> ");
			_currentTownUIController.HideCanvas();
			_inMenuUIController.HideCanvas();
		}

		private void UnlockTownMenu(TownData townData)
		{
			Debug.Log($"[TownManager] UnlockTownMenu() -> TownView: {townData.TownMenu} - Cost: {townData.Cost}");
			AppContext.Player.Gold = Mathf.Max(0, AppContext.Player.Gold - townData.Cost);
			townData.IsUnlocked = true;
			TownView townView = _townViews.Find(td => td.townMenu == townData.TownMenu);
			_townUIController.UpdateTownButton(townData, townView);
			
		}

		private void UpdatePlayerGold(int currentGold)
		{
			_playerUIController.UpdatePlayerGold(currentGold);
		}

        #endregion
	}
}