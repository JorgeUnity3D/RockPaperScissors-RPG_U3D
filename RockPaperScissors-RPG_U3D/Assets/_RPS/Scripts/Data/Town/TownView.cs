using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class TownView
	{
		[SerializeField] private TownMenu _townMenu;
		[SerializeField] private bool _hasNpc;
		[HorizontalGroup("Icons"), PreviewField, SerializeField] private Sprite _buildingIcon;
		[HorizontalGroup("Icons"), PreviewField, SerializeField] private Sprite _notBuiltIcon;
		[HorizontalGroup("Icons"), PreviewField, SerializeField] private Sprite _npcIcon;
		[HorizontalGroup("Icons"), PreviewField, SerializeField] private Sprite _npcNotFoundIcon;

		public TownMenu TownMenu     => _townMenu;
		public bool     HasNpc       => _hasNpc;
		public Sprite   BuildingIcon    => _buildingIcon;
		public Sprite   NotBuiltIcon    => _notBuiltIcon;
		public Sprite   NPCIcon         => _npcIcon;
		public Sprite   NPCNotFoundIcon => _npcNotFoundIcon;
	}
}