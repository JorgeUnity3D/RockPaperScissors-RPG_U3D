using UnityEditor;
using UnityEngine;

namespace Kapibara.Util.Editor
{
	/// <summary>
	/// 3-panel editor for MapLevelScrObj: Levels | Steps | Step Detail.
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
		private SerializedProperty _stepsProp;

		private int                _selectedStepIndex = -1;
		private SerializedProperty _selectedStepProp;

		// ── Scroll ─────────────────────────────────────────────────────────────────

		private Vector2 _levelScroll;
		private Vector2 _stepScroll;

		// ── Draw dimensions ────────────────────────────────────────────────────────

		private float _w;
		private float _h;

		// ── Layout constants ───────────────────────────────────────────────────────

		private const float HEADER_HEIGHT = 50f;
		private const float LEVEL_PANEL_W = 210f;
		private const float STEP_PANEL_W  = 190f;
		private const float DIVIDER_W     = 2f;
		private const float ROW_H         = 36f;

		private static readonly Color SelectedBg   = new Color(0.24f, 0.48f, 0.90f, 0.30f);
		private static readonly Color DividerColor = new Color(0.15f, 0.15f, 0.15f, 1f);

		// ── Menu ───────────────────────────────────────────────────────────────────

		[MenuItem("Kapibara/Map Level Editor")]
		public static void Open()
		{
			MapLevelEditorWindow w = GetWindow<MapLevelEditorWindow>("Map Level Editor");
			w.minSize = new Vector2(740f, 500f);
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

			float contentY = HEADER_HEIGHT;
			float contentH = _h - HEADER_HEIGHT;
			float detailW  = _w - LEVEL_PANEL_W - STEP_PANEL_W - DIVIDER_W * 2f;

			Rect levelArea  = new Rect(0f,                                       contentY, LEVEL_PANEL_W, contentH);
			Rect div1       = new Rect(LEVEL_PANEL_W,                            contentY, DIVIDER_W,     contentH);
			Rect stepArea   = new Rect(LEVEL_PANEL_W + DIVIDER_W,               contentY, STEP_PANEL_W,  contentH);
			Rect div2       = new Rect(LEVEL_PANEL_W + DIVIDER_W + STEP_PANEL_W, contentY, DIVIDER_W,    contentH);
			Rect detailArea = new Rect(div2.xMax,                                contentY, detailW,       contentH);

			EditorGUI.DrawRect(div1, DividerColor);
			EditorGUI.DrawRect(div2, DividerColor);

			GUILayout.BeginArea(levelArea);
			DrawLevelsPanel();
			GUILayout.EndArea();

			GUILayout.BeginArea(stepArea);
			DrawStepsPanel();
			GUILayout.EndArea();

			GUILayout.BeginArea(detailArea);
			DrawStepDetailPanel();
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
				GUILayout.Height(_h - HEADER_HEIGHT - 148f));
			for (int i = 0; i < _levelsProp.arraySize; i++)
				DrawLevelRow(i);
			EditorGUILayout.EndScrollView();

			if (_selectedLevelProp != null)
			{
				EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), DividerColor);
				GUILayout.Space(2f);
				EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_level"),        new GUIContent("Number"));
				EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_levelName"),    new GUIContent("Name"));
				EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_isAvailable"),  new GUIContent("Available"));
				EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_levelIcon"),    new GUIContent("Icon"));
				EditorGUILayout.PropertyField(_selectedLevelProp.FindPropertyRelative("_levelPortrait"), new GUIContent("Portrait"));
			}
		}

		private void DrawLevelRow(int i)
		{
			SerializedProperty lp        = _levelsProp.GetArrayElementAtIndex(i);
			SerializedProperty nameProp  = lp.FindPropertyRelative("_levelName");
			SerializedProperty stepsProp = lp.FindPropertyRelative("_steps");
			SerializedProperty availProp = lp.FindPropertyRelative("_isAvailable");
			bool selected = i == _selectedLevelIndex;

			string label     = !string.IsNullOrEmpty(nameProp.stringValue) ? nameProp.stringValue : $"Level {i + 1}";
			int    stepCount = stepsProp?.arraySize ?? 0;
			string avail     = availProp.boolValue ? "+" : "-";

			Rect rowRect = EditorGUILayout.GetControlRect(false, ROW_H);
			if (selected) EditorGUI.DrawRect(rowRect, SelectedBg);

			Rect labelRect = new Rect(rowRect.x + 22f, rowRect.y + (ROW_H - EditorGUIUtility.singleLineHeight) * 0.5f,
				rowRect.width - 52f, EditorGUIUtility.singleLineHeight);
			Rect availRect = new Rect(rowRect.x + 4f, labelRect.y, 16f, EditorGUIUtility.singleLineHeight);
			Rect countRect = new Rect(rowRect.xMax - 30f, labelRect.y, 30f, EditorGUIUtility.singleLineHeight);

			GUI.Label(availRect, avail, EditorStyles.centeredGreyMiniLabel);
			GUI.Label(labelRect, label, selected ? EditorStyles.whiteLabel : EditorStyles.label);
			GUI.Label(countRect, $"{stepCount}s", EditorStyles.centeredGreyMiniLabel);

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
			_stepsProp          = _selectedLevelProp.FindPropertyRelative("_steps");
			ClearStepSelection();
		}

		private void AddLevel()
		{
			_levelsProp.arraySize++;
			SerializedProperty np = _levelsProp.GetArrayElementAtIndex(_levelsProp.arraySize - 1);
			np.FindPropertyRelative("_level").intValue        = _levelsProp.arraySize;
			np.FindPropertyRelative("_levelName").stringValue = $"Level {_levelsProp.arraySize}";
			np.FindPropertyRelative("_isAvailable").boolValue = false;
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

		// ── Steps Panel ────────────────────────────────────────────────────────────

		private void DrawStepsPanel()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label("STEPS", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			GUI.enabled = _selectedLevelIndex >= 0;
			if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(22f)))
				AddStep();
			GUI.enabled = _selectedLevelIndex >= 0 && _selectedStepIndex >= 0;
			if (GUILayout.Button("–", EditorStyles.toolbarButton, GUILayout.Width(22f)))
			{
				RemoveSelectedStep();
				GUI.enabled = true;
				return;
			}
			if (GUILayout.Button("↑", EditorStyles.toolbarButton, GUILayout.Width(22f)))
				MoveStep(-1);
			if (GUILayout.Button("↓", EditorStyles.toolbarButton, GUILayout.Width(22f)))
				MoveStep(1);
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();

			if (_selectedLevelIndex < 0 || _stepsProp == null)
			{
				GUILayout.Space(12f);
				GUILayout.Label("← Select a level", EditorStyles.centeredGreyMiniLabel);
				return;
			}

			_stepScroll = EditorGUILayout.BeginScrollView(_stepScroll);
			for (int i = 0; i < _stepsProp.arraySize; i++)
				DrawStepRow(i);
			EditorGUILayout.EndScrollView();
		}

		private void DrawStepRow(int i)
		{
			SerializedProperty sp       = _stepsProp.GetArrayElementAtIndex(i);
			SerializedProperty typeProp = sp.FindPropertyRelative("_type");
			bool selected = i == _selectedStepIndex;

			RPS.MapStepType type = (RPS.MapStepType)typeProp.enumValueIndex;
			string tag = type switch
			{
				RPS.MapStepType.Combat    => "[C]",
				RPS.MapStepType.Boss      => "[B]",
				RPS.MapStepType.Treasure  => "[T]",
				RPS.MapStepType.NpcRescue => "[N]",
				_                         => "[?]"
			};

			float rowH   = EditorGUIUtility.singleLineHeight + 8f;
			Rect rowRect = EditorGUILayout.GetControlRect(false, rowH);
			if (selected) EditorGUI.DrawRect(rowRect, SelectedBg);

			Rect lr = new Rect(rowRect.x + 6f, rowRect.y + 4f, rowRect.width - 8f, EditorGUIUtility.singleLineHeight);
			GUI.Label(lr, $"{i + 1}.  {tag}  {type}", selected ? EditorStyles.whiteLabel : EditorStyles.label);

			if (Event.current.type == EventType.MouseDown && rowRect.Contains(Event.current.mousePosition))
			{
				SelectStep(i);
				Event.current.Use();
				Repaint();
			}
		}

		private void SelectStep(int index)
		{
			if (_selectedStepIndex == index) return;
			_selectedStepIndex = index;
			_selectedStepProp  = _stepsProp.GetArrayElementAtIndex(index);
		}

		private void AddStep()
		{
			_stepsProp.arraySize++;
			_assetSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_asset);
			Repaint();
		}

		private void RemoveSelectedStep()
		{
			_stepsProp.DeleteArrayElementAtIndex(_selectedStepIndex);
			_assetSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_asset);
			ClearStepSelection();
			Repaint();
		}

		private void MoveStep(int dir)
		{
			int newIndex = _selectedStepIndex + dir;
			if (newIndex < 0 || newIndex >= _stepsProp.arraySize) return;
			_stepsProp.MoveArrayElement(_selectedStepIndex, newIndex);
			_selectedStepIndex = newIndex;
			_selectedStepProp  = _stepsProp.GetArrayElementAtIndex(newIndex);
			_assetSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_asset);
			Repaint();
		}

		// ── Step Detail Panel ──────────────────────────────────────────────────────

		private void DrawStepDetailPanel()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			string header = _selectedStepProp != null ? $"STEP  {_selectedStepIndex + 1}" : "STEP DETAIL";
			GUILayout.Label(header, EditorStyles.boldLabel);
			GUILayout.EndHorizontal();

			if (_selectedStepProp == null)
			{
				GUILayout.Space(12f);
				GUILayout.Label("← Select a step", EditorStyles.centeredGreyMiniLabel);
				return;
			}

			GUILayout.Space(8f);

			SerializedProperty typeProp = _selectedStepProp.FindPropertyRelative("_type");
			EditorGUILayout.PropertyField(typeProp, new GUIContent("Type"));

			GUILayout.Space(6f);
			EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), DividerColor);
			GUILayout.Space(6f);

			RPS.MapStepType type = (RPS.MapStepType)typeProp.enumValueIndex;
			switch (type)
			{
				case RPS.MapStepType.Combat:
				case RPS.MapStepType.Boss:
					EditorGUILayout.PropertyField(
						_selectedStepProp.FindPropertyRelative("_enemy"),
						new GUIContent("Enemy"));
					break;
				case RPS.MapStepType.Treasure:
					EditorGUILayout.PropertyField(
						_selectedStepProp.FindPropertyRelative("_goldAmount"),
						new GUIContent("Gold Amount"));
					EditorGUILayout.PropertyField(
						_selectedStepProp.FindPropertyRelative("_treasureSprite"),
						new GUIContent("Treasure Sprite"));
					break;
				case RPS.MapStepType.NpcRescue:
					EditorGUILayout.PropertyField(
						_selectedStepProp.FindPropertyRelative("_targetBuilding"),
						new GUIContent("Target Building"));
					EditorGUILayout.PropertyField(
						_selectedStepProp.FindPropertyRelative("_npcSprite"),
						new GUIContent("NPC Sprite"));
					EditorGUILayout.PropertyField(
						_selectedStepProp.FindPropertyRelative("_npcDialogueLines"),
						new GUIContent("Dialogue Lines"));
					break;
			}
		}

		// ── Helpers ────────────────────────────────────────────────────────────────

		private void ClearLevelSelection()
		{
			_selectedLevelIndex = -1;
			_selectedLevelProp  = null;
			_stepsProp          = null;
			ClearStepSelection();
		}

		private void ClearStepSelection()
		{
			_selectedStepIndex = -1;
			_selectedStepProp  = null;
		}
	}
}
