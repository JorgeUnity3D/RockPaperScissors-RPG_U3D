using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class PaperTreeManager : BaseManager
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

		public override void Initialize()
		{
			Debug.Log($"[PaperTreeManager] Initialize() -> ");
			_paperTreeUIController.SetData(_player.Attributes, _paperTreeScrObj);
		}
		
		#endregion
	}
}