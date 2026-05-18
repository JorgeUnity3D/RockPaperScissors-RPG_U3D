using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Manager de la Biblioteca. Lee la config de quests del ScriptableObject e inyecta
	/// el progreso del jugador para mostrar el estado de cada quest.
	/// Kill tracking requiere combat (Phase 4); por ahora todos los contadores son 0.
	/// </summary>
	public class LibraryManager : BaseManager, ITownBuilding
	{
		[Header("DATA")]
		[SerializeField] private LibraryScrObj _libraryScrObj;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private LibraryUIController _libraryUIController;
		[SerializeField, ReadOnly] private Player _player;

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[LibraryManager] SetUp() -> ");
			_libraryUIController = ServiceLocator.Instance.GetService<UIService>().GetController<LibraryUIController>();
			_player = AppContext.Player;
		}

		protected override void Subscribe()
		{
			Debug.Log($"[LibraryManager] Subscribe() -> Nothing to subscribe!");
		}

		protected override void UnSubscribe()
		{
			Debug.Log($"[LibraryManager] UnSubscribe() -> Nothing to unsubscribe!");
		}

		#endregion

		#region CONTROL

		public void OnMenuOpen()
		{
			Debug.Log($"[LibraryManager] OnMenuOpen() -> ");
			_libraryUIController.SetData(_libraryScrObj.Data, _player.LibraryKillCounts);
		}

		#endregion
	}
}
