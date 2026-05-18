using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Gestiona el loop de un step de NPC. Muestra NPCUIController, navega por las líneas de diálogo
	/// y dispara OnCombatFinished(true) al pasar la última.
	/// </summary>
	public class NPCStepManager : MonoBehaviour
	{
		private NPCUIController _npcUI;
		private Player          _player;
		private List<string>    _dialogueLines;
		private int             _currentLineIndex;

		#region UNITY LIFECYCLE

		private void Awake()
		{
			_npcUI  = ServiceLocator.Instance.GetService<UIService>().GetController<NPCUIController>();
			_player = AppContext.Player;
		}

		#endregion

		#region CONTROL

		public void Initialize(MapStep step)
		{
			_dialogueLines    = step.NPCDialogueLines;
			_currentLineIndex = 0;

			AppEvents.OnNPCDialogueNext += OnNext;
			AppEvents.OnNPCDialoguePrev += OnPrev;

			_npcUI.SetData(step.NPCSprite, _player.CurrentHealth, _player.MaxHealth.TotalValue);
			_npcUI.SetDialogueLine(_dialogueLines[_currentLineIndex], _currentLineIndex % 2 == 1);
			_npcUI.SetPrevInteractable(false);
			_npcUI.ShowCanvas();

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
				_npcUI.HideCanvas();
				AppEvents.OnCombatFinished?.Invoke(true);
				return;
			}

			_npcUI.SetDialogueLine(_dialogueLines[_currentLineIndex], _currentLineIndex % 2 == 1);
			_npcUI.SetPrevInteractable(_currentLineIndex > 0);
			Debug.Log($"[NPCStepManager] Line {_currentLineIndex}/{_dialogueLines.Count - 1}");
		}

		private void OnPrev()
		{
			if (_currentLineIndex <= 0) return;

			_currentLineIndex--;
			_npcUI.SetDialogueLine(_dialogueLines[_currentLineIndex], _currentLineIndex % 2 == 1);
			_npcUI.SetPrevInteractable(_currentLineIndex > 0);
			Debug.Log($"[NPCStepManager] Line {_currentLineIndex}/{_dialogueLines.Count - 1}");
		}

		#endregion
	}
}
