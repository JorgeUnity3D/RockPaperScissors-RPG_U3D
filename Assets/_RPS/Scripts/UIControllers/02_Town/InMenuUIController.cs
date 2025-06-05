using Kapibara.Util.Extensions;
using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class InMenuUIController : BaseUIController
	{
		[Header("UI")]
		[SerializeField] private Button _backButton;
		[SerializeField] private TextMeshProUGUI _menuNameText;
		[SerializeField] private TextMeshProUGUI _menuLevelText;
		[SerializeField] private Slider _menuLevelSlider;
		[SerializeField] private GameObject _menuNPCHolder;
		[SerializeField] private TextMeshProUGUI _menuNPCText;
		[SerializeField] private Image _menuNPCImage;
		
		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion
		
		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[InMenuUIController] SetUp() -> ");
			HideCanvas(0);
			_backButton.AddListener(GoToMain);
		}
		
		public void SetData(TownData townData, TownView townView)
		{
			Debug.Log($"[InMenuUIController] SetData() -> townView {townData.Name}");
			_menuNameText.text = townData.Name;
			_menuLevelText.text = townData.Level.ToString();
			_menuLevelSlider.value = townData.LevelProgress;
			_menuNPCHolder.SetActive(townData.HasNpc);
			_menuNPCText.text = townData.Message + (townData.NpcUnlocked ? "" : " but there's no NPC here!");
			_menuNPCImage.sprite = townData.NpcUnlocked ? townView.npcIcon : townView.npcNotFoundIcon;
		}

		#endregion
		
		#region CONTROL

		private void GoToMain()
		{
			Debug.Log($"[InMenuUIController] SetData() -> Invoking: AppEvents.OnBackFromTownMenu");
			AppEvents.OnBackFromTownMenu?.Invoke();
		}

		#endregion
		
	}
}