using System;
using System.Collections.Generic;
using Kapibara.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Kapibara.RPS
{
	public class HouseUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private TextMeshProUGUI _nameText;
		[SerializeField] private TextMeshProUGUI _levelText;
		[SerializeField] private GameObject _houseStatPrefab;
		[SerializeField] private List<Transform> _houseStatsHolder;
		[SerializeField] private IconsScrObj _iconsScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private IconsDictionary _icons;
		[SerializeField, ReadOnly] private Dictionary<Stats, HouseStat> _houseDictionary;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[HouseUIController] SetUp() -> ");
			HideCanvas(0);
			_icons = _iconsScrObj.Data;
			_houseDictionary = new Dictionary<Stats, HouseStat>();
		}

		public void SetData(Player player)
		{
			Debug.Log($"[HouseUIController] SetData() -> ");
			
			if (_houseDictionary.Count == 0)
			{
				int holderIndex = 0;
				int counter = 0;
				foreach (Stats stat in Enum.GetValues(typeof(Stats)))
				{
					HouseStat houseStat = Instantiate(_houseStatPrefab, _houseStatsHolder[holderIndex]).GetComponent<HouseStat>();
					_houseDictionary.Add(stat, houseStat);
					counter++;
					if (counter >= 3)
					{
						counter = 0;
						holderIndex++;
					}
				}
			}
			RefreshData(player);
		}

		public void RefreshData(Player player)
		{
			Debug.Log($"[HouseUIController] RefreshData() -> ");
			_nameText.text = player.Name;
			_levelText.text = player.Level.ToString();
			foreach (Stats stat in Enum.GetValues(typeof(Stats)))
			{
				_houseDictionary[stat].SetData(player[stat], _icons[stat]);
			}
		}

		#endregion

	}
}