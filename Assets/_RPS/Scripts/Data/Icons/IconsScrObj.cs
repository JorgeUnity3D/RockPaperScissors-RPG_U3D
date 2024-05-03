using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "Icons", menuName = "RPSRPG/Icons")]
	public class IconsScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private IconsDictionary _data;
		public IconsDictionary Data => _data;
	}
}
