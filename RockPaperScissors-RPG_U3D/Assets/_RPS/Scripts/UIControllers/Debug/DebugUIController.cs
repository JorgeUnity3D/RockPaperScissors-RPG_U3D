using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kapibara.RPS
{
	/// <summary>
	/// Panel de debug en runtime generado por IMGUI — sin prefab ni Canvas necesarios.
	/// Siempre visible: botón [DBG] en esquina superior derecha. Al pulsarlo aparece la ventana
	/// arrastrable con todos los métodos de DebugService. Hijo de DebugService en GameCore.
	/// </summary>
	public class DebugUIController : MonoBehaviour
	{
		private DebugService _service;

		private bool    _isOpen     = false;
		private Rect    _windowRect = new Rect(20f, 50f, 320f, 480f);
		private Vector2 _scroll;
		private string  _goldInput  = "1000";

		private GUIStyle _sectionStyle;

		private const float TOGGLE_W = 60f;
		private const float TOGGLE_H = 22f;

		// ── Unity lifecycle ────────────────────────────────────────────────────

		private void Awake()
		{
			_service = GetComponentInParent<DebugService>();
		}

		// ── Public control (usable desde DebugService o gestos externos) ───────

		public void Show()   => _isOpen = true;
		public void Hide()   => _isOpen = false;
		public void Toggle() => _isOpen = !_isOpen;

		// ── IMGUI ──────────────────────────────────────────────────────────────

		private void OnGUI()
		{
			EnsureStyles();

			// Botón de toggle — siempre visible en esquina superior derecha
			Rect toggleRect = new Rect(Screen.width - TOGGLE_W - 8f, 8f, TOGGLE_W, TOGGLE_H);
			if (GUI.Button(toggleRect, "[DBG]"))
				_isOpen = !_isOpen;

			if (!_isOpen) return;

			_windowRect = GUILayout.Window(9999, _windowRect, DrawWindow, "  RPS Debug");
		}

		private void DrawWindow(int windowId)
		{
			// Info header
			string sceneName = SceneManager.GetActiveScene().name;
			string gold      = AppContext.GameContext != null ? AppContext.Player.Gold.ToString() : "–";
			GUILayout.Label($"Scene: {sceneName}   |   Gold: {gold}");

			GUILayout.Space(2f);
			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1f));

			_scroll = GUILayout.BeginScrollView(_scroll);

			// ── Economy ──────────────────────────────────────────────────────
			Section("Economy");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Gold:", GUILayout.Width(38f));
			_goldInput = GUILayout.TextField(_goldInput, GUILayout.Width(76f));
			if (GUILayout.Button("+Add")  && int.TryParse(_goldInput, out int addAmt)) _service.AddGold(addAmt);
			if (GUILayout.Button("Set") && int.TryParse(_goldInput, out int setAmt)) _service.SetGold(setAmt);
			GUILayout.EndHorizontal();

			// ── Player Level ─────────────────────────────────────────────────
			Section("Player Level  (ScissorBonfire)");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Force Level Up"))  _service.ForceLevelUp();
			if (GUILayout.Button("Reset Level"))     _service.ResetPlayerLevel();
			GUILayout.EndHorizontal();

			// ── Items ─────────────────────────────────────────────────────────
			Section("Items  (StoneSmithy)");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Max All Items"))   _service.MaxAllItems();
			if (GUILayout.Button("Reset Items"))     _service.ResetAllItems();
			GUILayout.EndHorizontal();

			// ── Paper Tree ───────────────────────────────────────────────────
			Section("Paper Tree");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Unlock All Nodes"))  _service.UnlockAllPaperTreeNodes();
			if (GUILayout.Button("Reset Nodes"))       _service.ResetAllPaperTreeNodes();
			GUILayout.EndHorizontal();

			// ── Stories ───────────────────────────────────────────────────────
			Section("Stories  (Theater)");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Unlock All"))  _service.UnlockAllStories();
			if (GUILayout.Button("Lock All"))    _service.LockAllStories();
			GUILayout.EndHorizontal();

			// ── Library ───────────────────────────────────────────────────────
			Section("Library");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Complete All Quests"))  _service.CompleteAllLibraryQuests();
			if (GUILayout.Button("Reset Quests"))         _service.ResetLibraryQuests();
			GUILayout.EndHorizontal();

			// ── Town ──────────────────────────────────────────────────────────
			Section("Town");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Unlock All"))   _service.UnlockAllBuildings();
			if (GUILayout.Button("Rescue NPCs"))  _service.RescueAllNPCs();
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Reset All Buildings"))  _service.ResetAllBuildings();

			GUILayout.Space(10f);
			if (GUILayout.Button("✕  Close Debug Panel"))
				_isOpen = false;

			GUILayout.EndScrollView();

			// Hace la ventana arrastrable desde cualquier punto no consumido por controles
			GUI.DragWindow();
		}

		// ── Helpers ────────────────────────────────────────────────────────────

		private void Section(string title)
		{
			GUILayout.Space(6f);
			GUILayout.Label($"── {title}", _sectionStyle);
			GUILayout.Space(1f);
		}

		private void EnsureStyles()
		{
			if (_sectionStyle != null) return;
			_sectionStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
		}
	}
}
