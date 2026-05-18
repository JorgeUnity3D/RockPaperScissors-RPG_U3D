using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager del árbol de habilidades Paper Tree. Alimenta la vista con los datos del jugador y el ScriptableObject del árbol.
	/// </summary>
	public class PaperTreeManager : BaseManager, ITownBuilding
	{
		[Header("UI")]
		[SerializeField] private PaperTreeScrObj _paperTreeScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private Player _player;
		[SerializeField, ReadOnly] private PaperTreeUIController _paperTreeUIController;
		
		#region SETUP
		
		public override void SetUp()
		{
			Debug.Log($"[PaperTreeManager] SetUp() -> ");
			_paperTreeUIController = ServiceLocator.Instance.GetService<UIService>().GetController<PaperTreeUIController>();
			_player = AppContext.Player;
		}

		protected override void Subscribe()
		{
			Debug.Log($"[PaperTreeManager] Subscribe() ->  Nothing to subscribe!");
		}
		
		protected override void UnSubscribe()
		{
			Debug.Log($"[PaperTreeManager] UnSubscribe() ->  Nothing to unsubscribe!");
		}

        #endregion

        #region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log($"[PaperTreeManager] OnMenuOpen() -> ");
			RestoreNodeState();
			_paperTreeUIController.SetData(_player.Attributes, _paperTreeScrObj, AppContext.Player.Gold);
		}

		private static readonly Stats[] PaperTreeStats =
		{
			Stats.ROCK, Stats.PAPER, Stats.SCISSOR, Stats.DEFENSE, Stats.ENERGY_RECOVERY
		};

		private void RestoreNodeState()
		{
			foreach (Stats stat in PaperTreeStats)
			{
				List<PaperTreeNode> nodes = _paperTreeScrObj[stat];
				if (nodes == null) continue;
				StatAttribute attribute = _player.Attributes.Find(a => a.Stat == stat);
				if (attribute == null) continue;
				PaperTreeModifier modifier = attribute.GetModifier<PaperTreeModifier>();
				if (modifier == null) continue;
				foreach (PaperTreeNode node in nodes)
					node.IsUnlocked = modifier.UnlockedNodes.Contains(node.NodeID);
			}
		}
		
		#endregion
	}
}