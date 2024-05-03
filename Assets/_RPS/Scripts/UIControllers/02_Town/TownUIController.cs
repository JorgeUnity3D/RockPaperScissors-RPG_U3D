using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class TownUIController : BaseUIController
	{
		[SerializeField] private TownDictionary _townDictionary;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion
		
        #region SETUP

		public override void SetUp()
		{
			Debug.Log($"[TownUIController] SetUp() -> ");
			foreach (KeyValuePair<TownMenu, UIButton> entry in _townDictionary)
			{
				TownMenu townMenu = entry.Key;
				UIButton townButton = entry.Value;
				townButton.AddListener(() =>
				{
					OpenTownMenu(townMenu);
				}, true);
			}
		}

		public void SetData(List<TownData> townViews, List<TownView> townDatas)
		{
			Debug.Log($"[TownUIController] SetData() -> ");
			for (int i = 0; i < townViews.Count; i++)
			{
				TownData townData = townViews[i];
				TownView townView = townDatas.Find(td => td.townMenu == townData.TownMenu);
				UpdateTownButton(townData, townView);
			}
		}

		public void UpdateTownButton(TownData townData, TownView townView)
		{
			Debug.Log($"[TownUIController] UpdateTownButton() -> townView: {townData.TownMenu}");
			UIButton townButton = _townDictionary[townData.TownMenu];
			townButton.GetComponent<Image>().sprite = townData.IsUnlocked ? townView.buildingIcon : townView.notBuiltIcon;
		}
		
        #endregion

        #region CONTROL

		private void OpenTownMenu(TownMenu townMenu)
		{
			AppEvents.OnOpenTownMenu?.Invoke(townMenu);
		}
		
        #endregion
	}
}