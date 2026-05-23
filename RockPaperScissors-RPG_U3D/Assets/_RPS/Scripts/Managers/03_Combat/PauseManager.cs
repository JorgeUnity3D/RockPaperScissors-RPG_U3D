using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Gestiona la lógica del menú de pausa. Suscribe a OnOptionsRequested y OnCombatExited.
	/// </summary>
	public class PauseManager : BaseManager
	{
		#region SETUP

		public override void SetUp()    { }
		public override void Initialize() { }

		protected override void Subscribe()
		{
			AppEvents.OnOptionsRequested += OnOptionsRequested;
			AppEvents.OnCombatExited     += OnCombatExited;
		}

		protected override void UnSubscribe()
		{
			AppEvents.OnOptionsRequested -= OnOptionsRequested;
			AppEvents.OnCombatExited     -= OnCombatExited;
		}

		#endregion

		#region CALLBACKS

		private void OnOptionsRequested()
		{
			Debug.Log("[PauseManager] OnOptionsRequested — not yet implemented.");
		}

		private void OnCombatExited()
		{
			Debug.Log("[PauseManager] OnCombatExited — abandoning level.");
			AppEvents.OnStepFinished?.Invoke(false);
		}

		#endregion
	}
}
