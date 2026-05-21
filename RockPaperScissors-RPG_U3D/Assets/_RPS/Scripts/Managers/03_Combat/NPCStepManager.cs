using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Gestiona el loop de un step de NPC. Muestra NPCHUDUIController, navega por las líneas de diálogo
	/// vía PlayerHUDUIController (Prev/Next) y dispara OnCombatFinished(true) al pasar la última.
	/// </summary>
	public class NPCStepManager : BaseManager
	{
		private NPCHUDUIController    _npcHUD;
		private PlayerHUDUIController _playerHUD;
		private Player                _player;
		private List<string>          _dialogueLines;
		private int                   _currentLineIndex;

		#region SETUP

		public override void SetUp()
		{
			UIService uiService = ServiceLocator.Instance.GetService<UIService>();
			_npcHUD    = uiService.GetController<NPCHUDUIController>();
			_playerHUD = uiService.GetController<PlayerHUDUIController>();
			_player    = AppContext.Player;
		}

		protected override void Subscribe()   { }
		protected override void UnSubscribe() { }

		#endregion

		#region CONTROL

		public void Initialize(MapStep step)
		{
			_dialogueLines    = step.NPCDialogueLines;
			_currentLineIndex = 0;

			if (_dialogueLines == null || _dialogueLines.Count == 0)
			{
				Debug.LogWarning("[NPCStepManager] Initialize() -> NPC dialogue lines are empty — completing step immediately.");
				AppEvents.OnCombatFinished?.Invoke(true);
				return;
			}

			AppEvents.OnNPCDialogueNext += OnNext;
			AppEvents.OnNPCDialoguePrev += OnPrev;

			_npcHUD.SetData(step.NPCSprite);
			_npcHUD.SetDialogueLine(_dialogueLines[_currentLineIndex], _currentLineIndex % 2 == 1);
			_npcHUD.ShowCanvas();

			_playerHUD.HidePlayerBubbles();
			_playerHUD.SetNPCPrevInteractable(false);

			Debug.Log($"[NPCStepManager] Initialize() -> lines:{_dialogueLines.Count}");
		}

		private void OnNext()
		{
			_currentLineIndex++;

			if (_currentLineIndex >= _dialogueLines.Count)
			{
				AppEvents.OnNPCDialogueNext -= OnNext;
				AppEvents.OnNPCDialoguePrev -= OnPrev;

				Debug.Log($"[NPCStepManager] Dialogue finished.");
				_npcHUD.HideCanvas();
				AppEvents.OnCombatFinished?.Invoke(true);
				return;
			}

			_npcHUD.SetDialogueLine(_dialogueLines[_currentLineIndex], _currentLineIndex % 2 == 1);
			_playerHUD.SetNPCPrevInteractable(_currentLineIndex > 0);
			Debug.Log($"[NPCStepManager] Line {_currentLineIndex}/{_dialogueLines.Count - 1}");
		}

		private void OnPrev()
		{
			if (_currentLineIndex <= 0) return;

			_currentLineIndex--;
			_npcHUD.SetDialogueLine(_dialogueLines[_currentLineIndex], _currentLineIndex % 2 == 1);
			_playerHUD.SetNPCPrevInteractable(_currentLineIndex > 0);
			Debug.Log($"[NPCStepManager] Line {_currentLineIndex}/{_dialogueLines.Count - 1}");
		}

		#endregion
	}
}
