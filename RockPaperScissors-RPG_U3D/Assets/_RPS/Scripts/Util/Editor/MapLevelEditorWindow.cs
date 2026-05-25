using UnityEditor;
using UnityEngine;

namespace Kapibara.Util.Editor
{
	/// <summary>
	/// 2-panel editor for MapLevelScrObj: Levels | Level Detail.
	/// Menu: Kapibara/Map Level Editor  —  also embeddable via DrawEmbedded().
	/// </summary>
	public class MapLevelEditorWindow : EditorWindow
	{
		// ── State ──────────────────────────────────────────────────────────────────

		private Kapibara.RPS.MapLevelScrObj _asset;
		private SerializedObject             _assetSO;
		private SerializedProperty           _levelsProp;

		private int                _selectedLevelIndex = -1;
		private SerializedProperty _selectedLevelProp;

		// ── Scroll ─────────────────────────────────────────────────────────────────

		private Vector2 _levelScroll;
		private Vector2 _detailScroll;

		// ── Draw dimensions ────────────────────────────────────────────────────────

		private float _w;
		private float _h;

		// ── Layout constants ───────────────────────────────────────────────────────

		private const float HEADER_HEIGHT = 50f;
		private const float LEVEL_PANEL_W = 210f;
		private const float DIVIDER_W     = 2f;
		private const float ROW_H         = 36f;

		private static readonly Color SelectedBg    = new Color(0.24f, 0.48f, 0.90f, 0.30f);
		private static readonly Color DividerColor  = new Color(0.15f, 0.15f, 0.15f, 1f);
		private static readonly Color SectionColor  = new Color(0.20f, 0.20f, 0.20f, 0.40f);

		// ── Menu ───────────────────────────────────────────────────────────────────

		[MenuItem("Kapibara/Map Level Editor")]
		public static void Open()
		{
			MapLevelEditorWindow w = GetWindow<MapLevelEditorWindow>("Map Level Editor");
			w.minSize = new Vector2(600f, 500f);
			w.Show();
		}

		// ── OnGUI / Embedded ───────────────────────────────────────────────────────

		private void OnGUI()
		{
			_w = position.width;
			_h = position.height;
			DrawContents();
		}

		public void DrawEmbedded(float w, float h)
		{
			_w = w;
			_h = h;
			DrawContents();
		}

		private void DrawContents()
		{
			DrawHeader();

			if (_asset == null)
			{
				Rect helpRect = new Rect(16f, HEADER_HEIGHT + 12f, _w - 32f, 40f);
				EditorGUI.HelpBox(helpRect, "Assign a MapLevelScrObj to begin.", MessageType.Info);
				return;
			}

			_assetSO.Update();

			float contentY  = HEADER_HEIGHT;
			float contentH  = _h - HEADER_HEIGHT;
			float detailW   = _w - LEVEL_PANEL_W - DIVIDER_W;

			Rect levelArea  = new Rect(0f,                      contentY, LEVEL_PANEL_W, contentH);
			Rect div        = new Rect(LEVEL_PANEL_W,           contentY, DIVIDER_W,     contentH);
			Rect detailArea = new Rect(LEVEL_PANEL_W + DIVIDER_W, contentY, detailW,    contentH);

			EditorGUI.DrawRect(div, DividerColor);

			GUILayout.BeginArea(levelArea);
			DrawLevelsPanel();
			GUILayout.EndArea();

			GUILayout.BeginArea(detailArea);
			DrawLevelDetailPanel();
			GUILayout.EndArea();

			_assetSO.ApplyModifiedProperties();
		}

		// ── Header ─────────────────────────────────────────────────────────────────

		private void DrawHeader()
		{
			GUILayout.BeginArea(new Rect(0f, 0f, _w, HEADER_HEIGHT));
			GUILayout.Space(6f);
			EditorGUI.BeginChangeCheck();
			Kapibara.RPS.MapLevelScrObj next = (Kapibara.RPS.MapLevelScrObj)EditorGUILayout.ObjectField(
				"Map Level Data", _asset, typeof(Kapibara.RPS.MapLevelScrObj), false);
			if (EditorGUI.EndChangeCheck()) SetAsset(next);
			GUILayout.Space(4f);
			EditorGUI.DrawRect(new Rect(0f, HEADER_HEIGHT - 1f, _w, 1f), DividerColor);
			GUILayout.EndArea();
		}

		private void SetAsset(Kapibara.RPS.MapLevelScrObj asset)
		{
			_asset      = asset;
			_assetSO    = asset != null ? new SerializedObject(asset) : null;
			_levelsProp = _assetSO?.FindProperty("_data");
			ClearLevelSelection();
		}

		// ── Levels Panel ───────────────────────────────────────────────────────────

		private void DrawLevelsPanel()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label("LEVELS", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(22f)))
				AddLevel();
			GUI.enabled = _selectedLevelIndex >= 0;
			if (GUILayout.Button("–", EditorStyles.toolbarButton, GUILayout.Width(22f)))
			{
				RemoveSelectedLevel();
				GUI.enabled = true;
				return;
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();

			if (_levelsProp == null) return;

			_levelScroll = EditorGUILayout.BeginScrollView(_levelScroll,
				GUILayout.Height(_h - HEADER_HEIGHT - 112f));
			for (int i = 0; i < _levelsProp.arraySize; i++)
				DrawLevelRow(i);
			EditorGUILayout.EndScrollView();

			if (_selectedLevelProp != null)
			{
				EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), DividerColor);
				GUILayout.Space(2f);
				EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_level"),         new GUIContent("Number"));
				EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_levelName"),     new GUIContent("Name"));
				EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_requiredLevels"), new GUIContent("Requires"), true);
			}
		}

		private void DrawLevelRow(int i)
		{
			SerializedProperty lp        = _levelsProp.GetArrayElementAtIndex(i);
			SerializedProperty nameProp  = lp.FindPropertyRelative("_levelName");
			SerializedProperty reqsProp  = lp.FindPropertyRelative("_requiredLevels");
			bool selected = i == _selectedLevelIndex;

			string label = !string.IsNullOrEmpty(nameProp.stringValue) ? nameProp.stringValue : $"Level {i + 1}";
			string avail = reqsProp.arraySize == 0 ? "★" : $"[{reqsProp.arraySize}]";

			Rect rowRect = EditorGUILayout.GetControlRect(false, ROW_H);
			if (selected) EditorGUI.DrawRect(rowRect, SelectedBg);

			Rect availRect = new Rect(rowRect.x + 4f,  rowRect.y + (ROW_H - EditorGUIUtility.singleLineHeight) * 0.5f, 16f, EditorGUIUtility.singleLineHeight);
			Rect labelRect = new Rect(rowRect.x + 22f, availRect.y, rowRect.width - 28f, EditorGUIUtility.singleLineHeight);

			GUI.Label(availRect, avail,  EditorStyles.centeredGreyMiniLabel);
			GUI.Label(labelRect, label, selected ? EditorStyles.whiteLabel : EditorStyles.label);

			if (Event.current.type == EventType.MouseDown && rowRect.Contains(Event.current.mousePosition))
			{
				SelectLevel(i);
				Event.current.Use();
				Repaint();
			}
		}

		private void SelectLevel(int index)
		{
			if (_selectedLevelIndex == index) return;
			_selectedLevelIndex = index;
			_selectedLevelProp  = _levelsProp.GetArrayElementAtIndex(index);
		}

		private void AddLevel()
		{
			_levelsProp.arraySize++;
			SerializedProperty np = _levelsProp.GetArrayElementAtIndex(_levelsProp.arraySize - 1);
			np.FindPropertyRelative("_level").intValue        = _levelsProp.arraySize;
			np.FindPropertyRelative("_levelName").stringValue = $"Level {_levelsProp.arraySize}";
			np.FindPropertyRelative("_requiredLevels").ClearArray();
			_assetSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_asset);
			Repaint();
		}

		private void RemoveSelectedLevel()
		{
			_levelsProp.DeleteArrayElementAtIndex(_selectedLevelIndex);
			_assetSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_asset);
			ClearLevelSelection();
			Repaint();
		}

		// ── Level Detail Panel ─────────────────────────────────────────────────────

		private void DrawLevelDetailPanel()
		{
			string header = _selectedLevelProp != null
				? $"LEVEL  {_selectedLevelProp.FindPropertyRelative("_levelName").stringValue}"
				: "LEVEL DETAIL";

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label(header, EditorStyles.boldLabel);
			GUILayout.EndHorizontal();

			if (_selectedLevelProp == null)
			{
				GUILayout.Space(12f);
				GUILayout.Label("← Select a level", EditorStyles.centeredGreyMiniLabel);
				return;
			}

			_detailScroll = EditorGUILayout.BeginScrollView(_detailScroll);
			GUILayout.Space(6f);

			DrawSection("Unlock Requirements");
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_requiredLevels"), new GUIContent("Required Levels  (empty = start available)"), true);

			GUILayout.Space(8f);
			DrawSection("Visuals");
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_levelIcon"),     new GUIContent("Icon"));
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_levelPortrait"), new GUIContent("Portrait"));

			GUILayout.Space(8f);
			DrawSection("Enemies  (3 recommended)");
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_possibleEnemies"), new GUIContent("Possible Enemies"), true);
			GUILayout.Space(4f);
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_boss"), new GUIContent("Boss"));

			GUILayout.Space(8f);
			DrawSection("Treasure Step");
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_treasureGoldAmount"), new GUIContent("Gold Amount"));
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_treasureSprite"),     new GUIContent("Sprite"));

			GUILayout.Space(8f);
			DrawSection("Boss Historia  (first run only)");
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_bossStory"), new GUIContent("Comic Story"));

			GUILayout.Space(8f);
			DrawSection("NPC Step  (first run only)");
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_targetBuilding"),   new GUIContent("Target Building"));
			EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_npcDialogueLines"), new GUIContent("Dialogue Lines"), true);

			GUILayout.Space(8f);
			EditorGUILayout.EndScrollView();
		}

		private void DrawSection(string title)
		{
			Rect r = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight + 4f);
			EditorGUI.DrawRect(r, SectionColor);
			EditorGUI.LabelField(new Rect(r.x + 6f, r.y + 2f, r.width, EditorGUIUtility.singleLineHeight),
				title, EditorStyles.boldLabel);
			GUILayout.Space(2f);
		}

		// ── Helpers ────────────────────────────────────────────────────────────────

		private void ClearLevelSelection()
		{
			_selectedLevelIndex = -1;
			_selectedLevelProp  = null;
		}
	}
}
