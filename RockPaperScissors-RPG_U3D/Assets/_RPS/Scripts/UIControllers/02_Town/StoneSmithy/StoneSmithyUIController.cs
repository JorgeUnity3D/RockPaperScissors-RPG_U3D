using Kapibara.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class StoneSmithyUIController : UIController
	{
		[Header("UI")]
		[SerializeField] private StoneSmithyButton _attackButton;
		[SerializeField] private StoneSmithyButton _healButton;
		[SerializeField] private StoneSmithyButton _energyButton;

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

		#endregion

		#region CONTROL

		/// <summary>
		/// Inicializa los tres slots de consumible con sus datos y callbacks de mejora.
		/// </summary>
		public void SetData(
			Item attackItem, Item healItem, Item energyItem,
			UnityAction onUpgradeAttack, UnityAction onUpgradeHeal, UnityAction onUpgradeEnergy)
		{
			SetupButton(_attackButton, attackItem, onUpgradeAttack);
			SetupButton(_healButton, healItem, onUpgradeHeal);
			SetupButton(_energyButton, energyItem, onUpgradeEnergy);
		}

		public void RefreshAttackLevel(int level) => _attackButton.SetLevel(level);
		public void RefreshHealLevel(int level)   => _healButton.SetLevel(level);
		public void RefreshEnergyLevel(int level) => _energyButton.SetLevel(level);

		private void SetupButton(StoneSmithyButton smithyButton, Item item, UnityAction onUpgrade)
		{
			smithyButton.SetButtonData(item);
			Button btn = smithyButton.GetComponent<Button>();
			btn.onClick.RemoveAllListeners();
			btn.onClick.AddListener(onUpgrade);
		}

		#endregion
	}
}
