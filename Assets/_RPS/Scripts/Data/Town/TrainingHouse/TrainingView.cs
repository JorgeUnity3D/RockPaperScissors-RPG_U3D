using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
	public class TrainingView {
		[HorizontalGroup("TrainingView")]
		public Stats stat;
		[HorizontalGroup("TrainingView"), PreviewField] public Sprite icon;
	}
}