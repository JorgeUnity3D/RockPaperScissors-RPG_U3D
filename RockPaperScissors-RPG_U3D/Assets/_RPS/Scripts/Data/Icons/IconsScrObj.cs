using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "StatIcons", menuName = "RPSRPG/Player/StatIcons")]
	public class IconsScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private IconsDictionary _data;
		public IconsDictionary Data
		{
			get => _data;
		}
	}
}
