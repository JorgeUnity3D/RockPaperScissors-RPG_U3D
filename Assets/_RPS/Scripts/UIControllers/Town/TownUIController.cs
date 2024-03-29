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

		public void SetData(List<TownView> townViews, List<TownData> townDatas)
		{
			Debug.Log($"[TownUIController] SetData() -> ");
			for (int i = 0; i < townViews.Count; i++)
			{
				TownView townView = townViews[i];
				TownData townData = townDatas.Find(td => td.townMenu == townView.TownMenu);
				UpdateTownButton(townView, townData);
			}
		}

		public void UpdateTownButton(TownView townView, TownData townData)
		{
			Debug.Log($"[TownUIController] UpdateTownButton() -> townView: {townView.TownMenu}");
			UIButton townButton = _townDictionary[townView.TownMenu];
			townButton.GetComponent<Image>().sprite = townView.IsUnlocked ? townData.buildingIcon : townData.notBuiltIcon;
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