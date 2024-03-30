using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class TownView
	{
		public TownMenu townMenu;
		[PreviewField] public Sprite buildingIcon;
		[PreviewField] public Sprite notBuiltIcon;
		[PreviewField] public Sprite npcIcon;
		[PreviewField] public Sprite npcNotFoundIcon;
	}
}