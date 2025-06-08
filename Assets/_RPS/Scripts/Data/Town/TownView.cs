using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class TownView
	{
		[SerializeField] private TownMenu _townMenu;
		[HorizontalGroup("Icons"), PreviewField, SerializeField] private Sprite _buildingIcon;
		[HorizontalGroup("Icons"), PreviewField, SerializeField] private Sprite _notBuiltIcon;
		[HorizontalGroup("Icons"), PreviewField, SerializeField] private Sprite _npcIcon;
		[HorizontalGroup("Icons"), PreviewField, SerializeField] private Sprite _npcNotFoundIcon;

		public TownMenu TownMenu
		{
			get => _townMenu;
		}

		public Sprite BuildingIcon
		{
			get => _buildingIcon;
		}

		public Sprite NotBuiltIcon
		{
			get => _notBuiltIcon;
		}

		public Sprite NPCIcon
		{
			get => _npcIcon;
		}

		public Sprite NPCNotFoundIcon
		{
			get => _npcNotFoundIcon;
		}
	}
}