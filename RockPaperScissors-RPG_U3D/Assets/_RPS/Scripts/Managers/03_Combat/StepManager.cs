using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Punto de entrada de la escena Combat. Lee el tipo del step actual, inicializa PlayerHUDUIController
	/// y delega a CombatStepManager, TreasureStepManager o NPCStepManager.
	/// </summary>
	public class StepManager : BaseManager
	{
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private CombatStepManager       _combatManager;
		[SerializeField, ReadOnly] private TreasureStepManager    _treasureStepManager;
		[SerializeField, ReadOnly] private NPCStepManager         _npcStepManager;
		[SerializeField, ReadOnly] private SurpriseBoxStepManager _surpriseBoxManager;
		[SerializeField, ReadOnly] private PlayerHUDUIController   _playerHUD;
		[SerializeField, ReadOnly] private EnemyHUDUIController    _enemyHUD;
		[SerializeField, ReadOnly] private TreasureHUDUIController _treasureHUD;
		[SerializeField, ReadOnly] private NPCHUDUIController      _npcHUD;
		[SerializeField, ReadOnly] private SurpriseBoxHUDUIController _surpriseBoxHUD;

		#region SETUP

		public override void SetUp()
		{
			_combatManager       = GetComponentInChildren<CombatStepManager>();
			_treasureStepManager = GetComponentInChildren<TreasureStepManager>();
			_npcStepManager      = GetComponentInChildren<NPCStepManager>();
			_surpriseBoxManager  = GetComponentInChildren<SurpriseBoxStepManager>();

			UIService uiService  = ServiceLocator.Instance.GetService<UIService>();
			_playerHUD           = uiService.GetController<PlayerHUDUIController>();
			_enemyHUD            = uiService.GetController<EnemyHUDUIController>();
			_treasureHUD         = uiService.GetController<TreasureHUDUIController>();
			_npcHUD              = uiService.GetController<NPCHUDUIController>();
			_surpriseBoxHUD      = uiService.GetController<SurpriseBoxHUDUIController>();
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

			CombatResultUIController resultUI = ServiceLocator.Instance.GetService<UIService>().GetController<CombatResultUIController>();
			if (resultUI != null) resultUI.HideCanvas(0);

			_enemyHUD?.HideCanvas(0);
			_treasureHUD?.HideCanvas(0);
			_npcHUD?.HideCanvas(0);
			_surpriseBoxHUD?.HideCanvas(0);

			MapStep step   = ctx.CurrentStep;
			Player  player = AppContext.Player;
			Debug.Log($"[StepManager] Initialize() -> step {ctx.CurrentStepIndex}  type:{step.Type}");

			_playerHUD.SetBackground(ctx.SelectedLevel.LevelPortrait);
			_playerHUD.SetData(player.MaxHealth.TotalValue, GameConsts.COMBAT_MAX_ENERGY);
			_playerHUD.RefreshBars(player.CurrentHealth, player.CurrentEnergy);
			_playerHUD.SetStep(step.Type);
			_playerHUD.ShowCanvas();

			switch (step.Type)
			{
				case MapStepType.COMBAT:
				case MapStepType.BOSS:
					if (_combatManager == null) { Debug.LogError("[StepManager] CombatStepManager not found in children."); return; }
					_combatManager.Initialize();
					break;
				case MapStepType.TREASURE:
					if (_treasureStepManager == null) { Debug.LogError("[StepManager] TreasureStepManager not found in children."); return; }
					_treasureStepManager.Initialize(step);
					break;
				case MapStepType.NPC_RESCUE:
					if (_npcStepManager == null) { Debug.LogError("[StepManager] NPCStepManager not found in children."); return; }
					_npcStepManager.Initialize(step);
					break;
				case MapStepType.SURPRISE_BOX:
					if (_surpriseBoxManager == null) { Debug.LogError("[StepManager] SurpriseBoxStepManager not found in children."); return; }
					_surpriseBoxManager.Initialize();
					break;
				default:
					Debug.LogError($"[StepManager] Unhandled step type: {step.Type}");
					break;
			}
		}

		#endregion

		#region DEBUG

		[Button("Skip Step")]
		private void SkipStep()
		{
			AppEvents.OnStepFinished?.Invoke(true);
		}

		#endregion
	}
}
