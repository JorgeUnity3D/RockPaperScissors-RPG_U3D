using System.Collections.Generic;
using Kapibara.Util.Extensions;
using Kapibara.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista principal de la ciudad. Muestra los botones de cada edificio con su sprite de bloqueado/desbloqueado y emite OnOpenTownMenu al pulsarlos.
	/// </summary>
	public class TownUIController : UIController
	{
		[Header("UI")]
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
			foreach (KeyValuePair<TownMenu, Button> entry in _townDictionary)
			{
				TownMenu townMenu = entry.Key;
				Button townButton = entry.Value;
				townButton.AddListener(() =>
				{
					OpenTownMenu(townMenu);
				}, true);
			}
		}

		/// <summary>Inicializa todos los botones de edificios con su estado actual de desbloqueo.</summary>
		public void SetData(List<TownData> townViews, List<TownView> townDatas)
		{
			Debug.Log($"[TownUIController] SetData() -> ");
			for (int i = 0; i < townViews.Count; i++)
			{
				TownData townData = townViews[i];
				TownView townView = townDatas.Find(td => td.TownMenu == townData.TownMenu);
				UpdateTownButton(townData, townView);
			}
		}

		/// <summary>Actualiza el sprite de un botón concreto según su estado de desbloqueo.</summary>
		public void UpdateTownButton(TownData townData, TownView townView)
		{
			Debug.Log($"[TownUIController] UpdateTownButton() -> townView: {townData.TownMenu}");
			Button townButton = _townDictionary[townData.TownMenu];
			townButton.GetComponent<Image>().sprite  = townData.IsUnlocked ? townView.BuildingIcon : townView.NotBuiltIcon;
			townButton.interactable = !townData.IsUnlocked || !townData.HasNpc || townData.NpcUnlocked;
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