using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{

	[CreateAssetMenu(fileName = "MapLevels", menuName = "RPSRPG/MapLevels")]
	public class MapLevelScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private List<MapLevel> _data;
		public List<MapLevel> Data
		{
			get => _data;
		}
	}
}