using TMPro;
using UnityEngine;
namespace Kapibara.RPS
{
	public class HouseUIController : BaseUIController
	{
		[SerializeField] private TextMeshProUGUI _nameText;
		[SerializeField] private TextMeshProUGUI _levelText;
		[SerializeField] private TextMeshProUGUI _rockLevelText;
		[SerializeField] private TextMeshProUGUI _paperLevelText;
		[SerializeField] private TextMeshProUGUI _scissorsLevelText;
		
		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion
		
		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[HouseUIController] SetUp() -> ");
			HideCanvas(0);
		}

		public void SetData(Player player)
		{
			Debug.Log($"[HouseUIController] SetData() -> ");
			_nameText.text = player.Name;
			_levelText.text = player.Level.ToString();
			_rockLevelText.text = player.Rock.ToString();
			_paperLevelText.text = player.Paper.ToString();
			_scissorsLevelText.text = player.Scissor.ToString();
		}

		#endregion
		
		#region CONTROL

		

		#endregion
	}
}