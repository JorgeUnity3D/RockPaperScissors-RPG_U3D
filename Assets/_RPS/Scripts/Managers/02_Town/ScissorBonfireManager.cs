using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class ScissorBonfireManager : BaseManager
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

		public override void Initialize()
		{
			Debug.Log($"[ScissorBonfireManager] Initialize() -> ");
			_scissorsBonfireUIController.SetData(AppContext.Player.Level);
		}
		
		private void ConfirmLevelUp()
		{
			Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> Current Level: {AppContext.Player.Level}");
			AppContext.Player.Gold = Mathf.Max(0, AppContext.Player.Gold - GameConsts.LEVEL_PRICES_AUX[AppContext.Player.Level]);
			AppContext.Player.Level++;
			List<StatAttribute> attributes = AppContext.Player.Attributes.FindAll(att => att.GetModifier<ScissorBonfireModifier>() != null);
			attributes.ForEach(att =>
			{
				Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> Stat {att.GetModifier<ScissorBonfireModifier>().Stat} currently is: {att.GetModifier<ScissorBonfireModifier>().Modifier}");
				att.GetModifier<ScissorBonfireModifier>().Level = AppContext.Player.Level;
				int statVariation = GameConsts.SCISSOR_MODS[AppContext.Player.Level][att.Stat];
				Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> Adding {statVariation}");
				att.GetModifier<ScissorBonfireModifier>().Modifier += statVariation;
				Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> Stat {att.GetModifier<ScissorBonfireModifier>().Stat} upgraded to: {att.GetModifier<ScissorBonfireModifier>().Modifier}");

			});
			Debug.Log($"[ScissorBonfireManager] ConfirmLevelUp() -> New Level: {AppContext.Player.Level}");
		}
		
		private void UpdateLevelUpView(int level)
		{
			Debug.Log($"[ScissorBonfireManager] UpdateLevelUpView() -> level {level}");
			_scissorsBonfireUIController.UpdateLevelUpView(level);
		}
		
		#endregion
	}
}