using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class TownView
	{
		public TownMenu townMenu;
		[HorizontalGroup("Icons"), PreviewField] public Sprite buildingIcon;
		[HorizontalGroup("Icons"), PreviewField] public Sprite notBuiltIcon;
		[HorizontalGroup("Icons"), PreviewField] public Sprite npcIcon;
		[HorizontalGroup("Icons"), PreviewField] public Sprite npcNotFoundIcon;
	}
}