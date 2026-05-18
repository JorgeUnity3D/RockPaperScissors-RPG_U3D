using Kapibara.UI;
using UnityEngine;
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
			_attackButton.GetComponent<Button>().onClick.AddListener(OnUpgradeAttackClicked);
			_healButton.GetComponent<Button>().onClick.AddListener(OnUpgradeHealClicked);
			_energyButton.GetComponent<Button>().onClick.AddListener(OnUpgradeEnergyClicked);
		}

		#endregion

		#region CONTROL

		public void SetData(Item attackItem, Item healItem, Item energyItem)
		{
			_attackButton.SetButtonData(attackItem);
			_healButton.SetButtonData(healItem);
			_energyButton.SetButtonData(energyItem);
		}

		public void RefreshAttackLevel(int level) => _attackButton.SetLevel(level);
		public void RefreshHealLevel(int level)   => _healButton.SetLevel(level);
		public void RefreshEnergyLevel(int level) => _energyButton.SetLevel(level);

		private void OnUpgradeAttackClicked()
		{
			AppEvents.OnUpgradeAttack?.Invoke();
		}

		private void OnUpgradeHealClicked()
		{
			AppEvents.OnUpgradeHeal?.Invoke();
		}

		private void OnUpgradeEnergyClicked()
		{
			AppEvents.OnUpgradeEnergy?.Invoke();
		}

		#endregion
	}
}
