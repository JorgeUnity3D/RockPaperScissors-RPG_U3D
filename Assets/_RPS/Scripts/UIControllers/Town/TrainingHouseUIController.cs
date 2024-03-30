using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class TrainingHouseUIController : BaseUIController
	{
		[Header("Stat Info")]
		[SerializeField] private GameObject _statInfoHolder;
		[SerializeField] private TextMeshProUGUI _statNameText;
		[SerializeField] private Image _statIconImage;
		[SerializeField] private TextMeshProUGUI _statLevelText;
		[SerializeField] private TextMeshProUGUI _statExperienceText;
		[SerializeField] private Slider _statLevelProgressSlider;
		[SerializeField] private TextMeshProUGUI _statBonusText;
		[SerializeField] private UIButton _selectTrainingButton;
		[Header("Unlock Stat")]
		[SerializeField] private GameObject _unlockStatHolder;
		[SerializeField] private TextMeshProUGUI _unlockStatNameText;
		[SerializeField] private Image _unlockStatIconImage;
		[SerializeField] private TextMeshProUGUI _unlockCostText;
		[SerializeField] private UIButton _unlockStatButton;
		
		
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

		

		#endregion
	}
}