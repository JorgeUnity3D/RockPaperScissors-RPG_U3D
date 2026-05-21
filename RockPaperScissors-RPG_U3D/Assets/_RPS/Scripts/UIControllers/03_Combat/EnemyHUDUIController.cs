using DG.Tweening;
using Kapibara.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// HUD del enemigo durante un step de combate. Pasiva: renderiza datos recibidos de CombatStepManager.
	/// </summary>
	public class EnemyHUDUIController : UIController
	{
		[Header("Enemy Bars")]
		[SerializeField] private Image           _enemyHPFill;
		[SerializeField] private Image           _enemyEnergyFill;
		[SerializeField] private Image           _enemyHPGhost;
		[SerializeField] private Image           _enemyEnergyGhost;
		[SerializeField] private TextMeshProUGUI _enemyHPText;
		[SerializeField] private TextMeshProUGUI _enemyEnergyText;

		[Header("Enemy Zone")]
		[SerializeField] private Image           _enemySprite;
		[SerializeField] private GameObject      _enemyActionBubble;
		[SerializeField] private Image           _enemyActionIcon;
		[SerializeField] private TextMeshProUGUI _enemyDebugText;
		[SerializeField] private GameObject      _enemyThoughtBubble;
		[SerializeField] private Image           _enemyThoughtIcon;

		private int _enemyMaxHP;
		private int _enemyMaxEnergy;

		#region UNITY LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			HideCanvas(0);
			HideBubbles();
		}

		public void SetData(Sprite enemySprite, int enemyMaxHP, int enemyMaxEnergy)
		{
			_enemySprite.sprite = enemySprite;

			_enemyMaxHP     = enemyMaxHP;
			_enemyMaxEnergy = enemyMaxEnergy;

			_enemyHPFill.rectTransform.anchorMin     = Vector2.zero;
			_enemyEnergyFill.rectTransform.anchorMin = Vector2.zero;

			if (_enemyHPGhost     != null) _enemyHPGhost.rectTransform.anchorMin     = Vector2.zero;
			if (_enemyEnergyGhost != null) _enemyEnergyGhost.rectTransform.anchorMin = Vector2.zero;

			if (_enemyHPText     != null) _enemyHPText.text     = $"{enemyMaxHP}/{enemyMaxHP}";
			if (_enemyEnergyText != null) _enemyEnergyText.text = $"{enemyMaxEnergy}/{enemyMaxEnergy}";
		}

		#endregion

		#region REFRESH

		public void RefreshBars(int currentHP, int currentEnergy)
		{
			float hpFill     = _enemyMaxHP     > 0 ? (float)currentHP     / _enemyMaxHP     : 0f;
			float energyFill = _enemyMaxEnergy > 0 ? (float)currentEnergy / _enemyMaxEnergy : 0f;

			Debug.Log($"[EnemyHUDUIController] RefreshBars → HP:{currentHP}/{_enemyMaxHP} ({hpFill:F2})  Energy:{currentEnergy}/{_enemyMaxEnergy} ({energyFill:F2})");

			AnimateFill(_enemyHPFill,     _enemyHPGhost,     hpFill);
			AnimateFill(_enemyEnergyFill, _enemyEnergyGhost, energyFill);

			if (_enemyHPText     != null) _enemyHPText.text     = $"{currentHP}/{_enemyMaxHP}";
			if (_enemyEnergyText != null) _enemyEnergyText.text = $"{currentEnergy}/{_enemyMaxEnergy}";
		}

		private void AnimateFill(Image fill, Image ghost, float target)
		{
			RectTransform fillRT = fill.rectTransform;
			fillRT.DOKill();
			fillRT.DOAnchorMin(new Vector2(1f - target, 0f), GameConsts.COMBAT_BAR_ANIM)
			      .SetEase(Ease.OutQuad)
			      .SetLink(fill.gameObject);

			if (ghost == null) return;
			RectTransform ghostRT = ghost.rectTransform;
			ghostRT.DOKill();
			ghostRT.DOAnchorMin(new Vector2(1f - target, 0f), GameConsts.COMBAT_BAR_GHOST_DUR)
			       .SetDelay(GameConsts.COMBAT_BAR_GHOST_DELAY)
			       .SetEase(Ease.OutQuad)
			       .SetLink(ghost.gameObject);
		}

		#endregion

		#region BUBBLES

		public void ShowEnemyActionBubble(Sprite actionIcon, int debugDamage)
		{
			if (actionIcon == null) return;
			_enemyActionBubble.SetActive(true);
			_enemyActionIcon.sprite = actionIcon;
			_enemyDebugText.text    = debugDamage.ToString();
		}

		public void ShowEnemyThoughtBubble(Sprite languageIcon)
		{
			if (languageIcon == null) return;
			_enemyThoughtBubble.SetActive(true);
			_enemyThoughtIcon.sprite = languageIcon;
		}

		public void HideBubbles()
		{
			_enemyActionBubble.SetActive(false);
			_enemyThoughtBubble.SetActive(false);
		}

		#endregion
	}
}
