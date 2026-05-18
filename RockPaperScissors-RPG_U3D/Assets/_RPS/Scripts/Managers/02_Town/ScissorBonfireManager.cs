using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de la Hoguera de las Tijeras. Gestiona la subida de nivel del jugador y la actualización de modificadores ScissorBonfire.
	/// </summary>
	public class ScissorBonfireManager : BaseManager, ITownBuilding
	{
		[SerializeField, ReadOnly] private Player _player;
		[SerializeField, ReadOnly] private List<StatAttribute> _levelAttributes;

		[SerializeField, ReadOnly] private ScissorsBonfireUIController _scissorsBonfireUIController;
		
		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[ScissorBonfireManager] SetUp() -> ");
			_scissorsBonfireUIController = ServiceLocator.Instance.GetService<UIService>().GetController<ScissorsBonfireUIController>();
			_player = AppContext.Player;
			_levelAttributes = AppContext.Attributes.FindAll(attribute => attribute.GetModifier<ScissorBonfireModifier>() != null);
		}
		
		protected override void Subscribe()
		{
			Debug.Log($"[ScissorBonfireManager] Subscribe() -> ");
			AppContext.Player.OnLevelValueChanged += UpdateLevelUpView;
			AppEvents.OnConfirmLevelUp += ConfirmLevelUp;
		}
		

		protected override void UnSubscribe()
		{
			Debug.Log($"[ScissorBonfireManager] UnSubscribe() -> ");
			AppContext.Player.OnLevelValueChanged -= UpdateLevelUpView;
			AppEvents.OnConfirmLevelUp -= ConfirmLevelUp;
		}
		
		#endregion

		#region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log($"[ScissorBonfireManager] OnMenuOpen() -> ");
			int level = AppContext.Player.Level;
			GetLevelUpData(level, out int cost, out bool canAfford, out ScissorBonfireModLevel statVariations);
			_scissorsBonfireUIController.SetData(level, cost, canAfford, statVariations);
		}
		
		private void ConfirmLevelUp()
		{
			Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> Current Level: {AppContext.Player.Level}");

			// TODO: SCISSOR_MODS only covers 10 level-ups. Add more entries to GameConsts.SCISSOR_MODS
			// and GameConsts.LEVEL_PRICES_AUX when higher level data is designed.
			int levelIndex = AppContext.Player.Level;
			if (levelIndex >= GameConsts.SCISSOR_MODS.Count)
			{
				Debug.LogWarning($"[ScissorBonfireManager] ConfirmLevelUp() -> No SCISSOR_MODS data for level {levelIndex}. Add more entries to GameConsts.");
				return;
			}

			AppContext.Player.Gold = Mathf.Max(0, AppContext.Player.Gold - GameConsts.LEVEL_PRICES_AUX[levelIndex]);
			AppContext.Player.Level++;
			List<StatAttribute> attributes = AppContext.Player.Attributes.FindAll(att => att.GetModifier<ScissorBonfireModifier>() != null);
			attributes.ForEach(att =>
			{
				Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> Stat {att.GetModifier<ScissorBonfireModifier>().Stat} currently is: {att.GetModifier<ScissorBonfireModifier>().Modifier}");
				att.GetModifier<ScissorBonfireModifier>().Level = AppContext.Player.Level;
				int statVariation = GameConsts.SCISSOR_MODS[levelIndex][att.Stat];
				Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> Adding {statVariation}");
				att.GetModifier<ScissorBonfireModifier>().Modifier += statVariation;
				Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> Stat {att.GetModifier<ScissorBonfireModifier>().Stat} upgraded to: {att.GetModifier<ScissorBonfireModifier>().Modifier}");
			});
			Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> New Level: {AppContext.Player.Level}");
			AppEvents.OnGameContextUpdated?.Invoke();
		}
		
		private void UpdateLevelUpView(int level)
		{
			Debug.Log($"[ScissorBonfireManager] UpdateLevelUpView() -> level {level}");
			GetLevelUpData(level, out int cost, out bool canAfford, out ScissorBonfireModLevel statVariations);
			_scissorsBonfireUIController.UpdateLevelUpView(level, cost, canAfford, statVariations);
		}

		private void GetLevelUpData(int level, out int cost, out bool canAfford, out ScissorBonfireModLevel statVariations)
		{
			bool hasData = level < GameConsts.SCISSOR_MODS.Count;
			cost = hasData ? GameConsts.LEVEL_PRICES_AUX[level - 1] : 0;
			canAfford = hasData && AppContext.Player.Gold >= cost;
			statVariations = hasData ? GameConsts.SCISSOR_MODS[level] : null;
		}
		
		#endregion
	}
}