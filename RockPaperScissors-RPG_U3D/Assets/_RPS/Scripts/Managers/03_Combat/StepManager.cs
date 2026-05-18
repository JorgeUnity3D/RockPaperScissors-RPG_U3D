using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Punto de entrada de la escena Combat. Lee el tipo del step actual y delega a CombatManager,
	/// TreasureStepManager o NPCStepManager. GameManager llama Initialize(); los sub-managers
	/// disparan OnCombatFinished cuando terminan.
	/// </summary>
	public class StepManager : BaseManager
	{
		[SerializeField, ReadOnly] private CombatManager       _combatManager;
		[SerializeField, ReadOnly] private TreasureStepManager _treasureStepManager;
		[SerializeField, ReadOnly] private NPCStepManager      _npcStepManager;

		#region SETUP

		public override void SetUp()
		{
			_combatManager       = GetComponentInChildren<CombatManager>();
			_treasureStepManager = GetComponentInChildren<TreasureStepManager>();
			_npcStepManager      = GetComponentInChildren<NPCStepManager>();
		}

		protected override void Subscribe()   { }
		protected override void UnSubscribe() { }

		#endregion

		#region CONTROL

		public override void Initialize()
		{
			CombatContext ctx = AppContext.CombatContext;
			if (ctx == null)
			{
				Debug.LogError("[StepManager] Initialize() -> CombatContext is null.");
				return;
			}

			if (ctx.CurrentStepIndex == 0)
				AppContext.Player.CurrentHealth = AppContext.Player.MaxHealth.TotalValue;

			MapStep step = ctx.CurrentStep;
			Debug.Log($"[StepManager] Initialize() -> step {ctx.CurrentStepIndex}  type:{step.Type}");

			switch (step.Type)
			{
				case MapStepType.Combat:
				case MapStepType.Boss:
					if (_combatManager == null) { Debug.LogError("[StepManager] CombatManager not found in children."); return; }
					_combatManager.Initialize();
					break;
				case MapStepType.Treasure:
					if (_treasureStepManager == null) { Debug.LogError("[StepManager] TreasureStepManager not found in children."); return; }
					_treasureStepManager.Initialize(step);
					break;
				case MapStepType.NpcRescue:
					if (_npcStepManager == null) { Debug.LogError("[StepManager] NPCStepManager not found in children."); return; }
					_npcStepManager.Initialize(step);
					break;
				default:
					Debug.LogError($"[StepManager] Unhandled step type: {step.Type}");
					break;
			}
		}

		#endregion
	}
}
