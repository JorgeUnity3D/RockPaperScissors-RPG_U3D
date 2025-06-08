using Kapibara.UI;
using Kapibara.Util.Extensions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class TravelUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private LevelButtonsDictionary _levelButtons;
		[SerializeField] private Image _levelPortraitImage;
		[SerializeField] private TextMeshProUGUI _levelNameText;
		[SerializeField] private Button _travelButton;
		[SerializeField] private TextMeshProUGUI _travelLabelText;		
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private List<MapLevel> _levels;
		
		private UnityAction<MapLevel> OnTravelConfirmed;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion
		
		#region SETUP

		public override void SetUp()
		{
			HideCanvas(0);
		}

		internal void SetData(List<StatAttribute> attributes, MapLevelScrObj mapLevelScrObj, UnityAction<MapLevel> OnTravelConfirmed)
		{
			Debug.Log($"[TravelUIController] SetData() -> ");
			_levels = mapLevelScrObj.Data;
			this.OnTravelConfirmed = OnTravelConfirmed;
			SetUpMapUI();
		}

		#endregion

		#region CONTROL

		private void SetUpMapUI()
		{
			Debug.Log($"[TravelUIController] SetUpMapUI() -> ");
			foreach (MapLevel level in _levels)
			{
				Button levelButton = _levelButtons[level.Level];
				if (levelButton == null)
				{
					Debug.Log($"[TravelUIController] SetUpMapUI() -> No Button found for level {level.Level}.{level.LevelName}");
					return;
				}
				levelButton.AddListener(() => SetUpLevelPreview(level), true);
			}			
		}

		private void SetUpLevelPreview(MapLevel level)
		{
			Debug.Log($"[TravelUIController] SetUpLevelPreview() -> {level.Level}.{level.LevelName}");
			_levelPortraitImage.sprite = level.LevelPortrait;
			_levelNameText.text = level.LevelName;
			_travelButton.AddListener(() => ConfirmTravel(level), true);
			_travelLabelText.text = level.Level.ToString("D1");
		}

		private void ConfirmTravel(MapLevel level)
		{
			Debug.Log($"[TravelUIController] ConfirmTravel() -> {level.Level}.{level.LevelName}");
			OnTravelConfirmed(level);
		}

		#endregion
	}
}