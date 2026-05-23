using TMPro;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Tarjeta de una quest de la Biblioteca.
	/// Muestra el enemigo objetivo, el progreso de kills y la recompensa de stat.
	/// Kill count es 0 hasta que combat esté implementado (Phase 4).
	/// </summary>
	public class LibraryQuestCard : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _enemyNameText;
		[SerializeField] private TextMeshProUGUI _killProgressText;
		[SerializeField] private TextMeshProUGUI _rewardText;

		public void SetData(LibraryQuestProgress quest)
		{
			_enemyNameText.text    = quest.EnemyDisplayName;
			_killProgressText.text = $"{quest.CurrentKills} / {quest.TargetKills}";
			_rewardText.text       = $"+{quest.RewardAmount} {quest.RewardStat}";
		}
	}
}
