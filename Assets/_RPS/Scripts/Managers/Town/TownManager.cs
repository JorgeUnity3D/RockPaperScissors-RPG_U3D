using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class TownManager : BaseManager
	{
		[SerializeField] private TownViewDataObject _townViewDataObject;
		[SerializeField, ReadOnly] private List<TownView> _townViews;
		[SerializeField, ReadOnly] private List<TownData> _townDatas;
		
		[SerializeField, ReadOnly] private UIService _uiService;
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
			_playerUIController = _uiService.GetController<PlayerUIController>();
			_unlockMenuUIController = _uiService.GetController<UnlockMenuUIController>();
			_townUIController = _uiService.GetController<TownUIController>();
			_houseUIController = _uiService.GetController<HouseUIController>();
			_inMenuUIController = _uiService.GetController<InMenuUIController>();
			_townViews = AppContext.TownViews;
			_townDatas = _townViewDataObject.Data;
		}

		protected override void Subscribe()
		{
            Debug.Log($"[TownManager] Subscribe() -> ");
            AppEvents.OnOpenTownMenu += OpenTownMenu;
            AppEvents.OnBackFromTownMenu += BackFromTownMenu;
            AppEvents.OnConfirmUnlock += UnlockTownMenu;
		}
		
		protected override void UnSubscribe()
		{
            Debug.Log($"[TownManager] UnSubscribe() -> ");
            AppEvents.OnOpenTownMenu -= OpenTownMenu;
            AppEvents.OnBackFromTownMenu -= BackFromTownMenu;
		}

        #endregion

        #region CONTROL

		public override void Initialize()
		{
			Debug.Log($"[TownManager] Initialize() -> ");
			_townUIController.SetData(_townViews, _townViewDataObject.Data);
			_playerUIController.UpdatePlayerGold(AppContext.Player.currentGold);
		}
		
		private void OpenTownMenu(TownMenu townMenu)
		{
			Debug.Log($"[TownManager] OpenTownMenu() -> TownMenu: {townMenu}");
			TownView townView = _townViews.Find(tv => tv.townMenu == townMenu);
			if (townView == null)
			{
				Debug.Log($"[TownManager] OpenTownMenu() -> Error: No TownView with {townMenu} flag");
				return;
			}
			TownData townData = _townDatas.Find(td => td.townMenu == townView.townMenu);
			if (townView.isUnlocked)
			{
				GoToTownMenu(townView, townData);
			}
			else
			{
				GoToUnlockMenu(townView, townData);
			}
		}
		
		private void GoToTownMenu(TownView townView, TownData townData)
		{
			Debug.Log($"[TownManager] GoToTownMenu() -> TownView: {townView.townMenu}");
			switch (townView.townMenu)
			{
				case TownMenu.LIBRARY:
					_currentTownUIController = _uiService.GetController<LibraryUIController>();
					break;
				case TownMenu.PAPER_TREE:
					_currentTownUIController = _uiService.GetController<PaperTreeUIController>();
					break;
				case TownMenu.SCISSORS:
					_currentTownUIController = _uiService.GetController<ScissorsBonfireUIController>();
					break;
				case TownMenu.STABLES:
					_currentTownUIController = _uiService.GetController<StablesUIController>();
					break;
				case TownMenu.STONE_SMITHY:
					_currentTownUIController = _uiService.GetController<StoneSmithyUIController>();
					break;
				case TownMenu.THEATER:
					_currentTownUIController = _uiService.GetController<TheaterUIController>();
					break;
				case TownMenu.TRAINING_HOUSE:
					_currentTownUIController = _uiService.GetController<TrainingHouseUIController>();
					break;
				case TownMenu.TRAVEL:
					_currentTownUIController = _uiService.GetController<TravelUIController>();
					break;
				case TownMenu.HOUSE:
					_currentTownUIController = _uiService.GetController<HouseUIController>();
					break;
			}
			_currentTownUIController.ShowCanvas();
			_inMenuUIController.ShowCanvas();
			_inMenuUIController.SetData(townView, townData);
		}
		
		private void GoToUnlockMenu(TownView townView, TownData townData)
		{
			Debug.Log($"[TownManager] GoToUnlockMenu() -> TownView: {townView.townMenu} - Cost: {townView.cost}");
			_unlockMenuUIController.ShowCanvas();
			_unlockMenuUIController.SetData(townView, townData);
		}
		
		private void BackFromTownMenu()
		{
			Debug.Log($"[TownManager] BackFromTownMenu() -> ");
			_currentTownUIController.HideCanvas();
			_inMenuUIController.HideCanvas();
		}

		private void UnlockTownMenu(TownView townView)
		{
			Debug.Log($"[TownManager] UnlockTownMenu() -> TownView: {townView.townMenu} - Cost: {townView.cost}");
			AppContext.Player.currentGold = Mathf.Max(0, AppContext.Player.currentGold - townView.cost);
			townView.isUnlocked = true;
			TownData townData = _townDatas.Find(td => td.townMenu == townView.townMenu);
			_townUIController.UpdateTownButton(townView, townData);
			_playerUIController.UpdatePlayerGold(AppContext.Player.currentGold);
			Debug.Log($"[TownManager] UnlockTownMenu() -> Invoking: AppEvents.OnGameContextUpdated");
			AppEvents.OnGameContextUpdated?.Invoke();
		}

        #endregion
	}
}