using Kapibara.RPS;
using UnityEditor;
using UnityEngine;

namespace Kapibara.Util.Editor
{
	/// <summary>
	/// 3-panel editor for PaperTreeScrObj: tree tabs | node list | node detail.
	/// Menu: Kapibara/Paper Tree Editor  —  also embeddable via DrawEmbedded().
	/// </summary>
	public class PaperTreeEditorWindow : EditorWindow
	{
		// ── Asset ──────────────────────────────────────────────────────────────────

		private PaperTreeScrObj  _asset;
		private SerializedObject _assetSO;

		private static readonly string[] TreePropNames =
		{
			"_rockSkillTree", "_paperSkillTree", "_scissorsSkillTree",
			"_defenseSkillTree", "_energyRecoverySkillTree"
		};

		private static readonly string[] TreeLabels =
		{
			"ROCK", "PAPER", "SCISSOR", "DEFENSE", "ENERGY REC"
		};

		// ── State ──────────────────────────────────────────────────────────────────

		private int                _selectedTree      = 0;
		private int                _selectedNodeIndex = -1;
		private SerializedProperty _currentTreeProp;
		private SerializedProperty _selectedNodeProp;

		// ── Scroll ─────────────────────────────────────────────────────────────────

		private Vector2 _nodeScroll;
		private Vector2 _detailScroll;

		// ── Draw dimensions ────────────────────────────────────────────────────────

		private float _w;
		private float _h;

		// ── Layout constants ───────────────────────────────────────────────────────

		private const float HEADER_H   = 50f;
		private const float TAB_H      = 26f;
		private const float NODE_PAN_W = 220f;
		private const float DIVIDER_W  = 2f;
		private const float ROW_H      = 30f;

		private static readonly Color SelectedBg   = new Color(0.24f, 0.48f, 0.90f, 0.30f);
		private static readonly Color DividerColor = new Color(0.15f, 0.15f, 0.15f, 1f);
		private static readonly Color SectionColor = new Color(0.20f, 0.20f, 0.20f, 0.40f);
		private static readonly Color RowEven      = new Color(1f,    1f,    1f,    0.02f);
		private static readonly Color RowOdd       = new Color(0f,    0f,    0f,    0.08f);

		// ── Menu ───────────────────────────────────────────────────────────────────

		[MenuItem("Kapibara/Paper Tree Editor")]
		public static void Open()
		{
			PaperTreeEditorWindow w = GetWindow<PaperTreeEditorWindow>("Paper Tree Editor");
			w.minSize = new Vector2(700f, 520f);
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
				Rect helpRect = new Rect(16f, HEADER_H + 12f, _w - 32f, 40f);
				EditorGUI.HelpBox(helpRect, "Assign a PaperTreeScrObj to begin.", MessageType.Info);
				return;
			}

			_assetSO.Update();

			float contentY = HEADER_H + TAB_H;
			float contentH = _h - contentY;
			float detailW  = _w - NODE_PAN_W - DIVIDER_W;

			DrawTreeTabs();

			Rect nodeArea   = new Rect(0f,                       contentY, NODE_PAN_W, contentH);
			Rect div        = new Rect(NODE_PAN_W,               contentY, DIVIDER_W,  contentH);
			Rect detailArea = new Rect(NODE_PAN_W + DIVIDER_W,   contentY, detailW,    contentH);

			EditorGUI.DrawRect(div, DividerColor);

			GUILayout.BeginArea(nodeArea);
			DrawNodeListPanel();
			GUILayout.EndArea();

			GUILayout.BeginArea(detailArea);
			DrawNodeDetailPanel();
			GUILayout.EndArea();

			_assetSO.ApplyModifiedProperties();
		}

		// ── Header ─────────────────────────────────────────────────────────────────

		private void DrawHeader()
		{
			GUILayout.BeginArea(new Rect(0f, 0f, _w, HEADER_H));
			GUILayout.Space(6f);
			EditorGUI.BeginChangeCheck();
			PaperTreeScrObj next = (PaperTreeScrObj)EditorGUILayout.ObjectField(
				"PaperTree Data", _asset, typeof(PaperTreeScrObj), false);
			if (EditorGUI.EndChangeCheck()) SetAsset(next);
			GUILayout.Space(4f);
			EditorGUI.DrawRect(new Rect(0f, HEADER_H - 1f, _w, 1f), DividerColor);
			GUILayout.EndArea();
		}

		private void SetAsset(PaperTreeScrObj asset)
		{
			_asset  = asset;
			_assetSO = asset != null ? new SerializedObject(asset) : null;
			SelectTree(_selectedTree);
		}

		// ── Tree Tabs ──────────────────────────────────────────────────────────────

		private void DrawTreeTabs()
		{
			GUILayout.BeginArea(new Rect(0f, HEADER_H, _w, TAB_H));
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			for (int i = 0; i < TreeLabels.Length; i++)
			{
				bool active = i == _selectedTree;
				GUIStyle style = active ? EditorStyles.toolbarButton : EditorStyles.toolbarButton;
				GUI.enabled = !active;
				if (GUILayout.Button(TreeLabels[i], style))
					SelectTree(i);
				GUI.enabled = true;
			}
			EditorGUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		private void SelectTree(int index)
		{
			_selectedTree      = index;
			_selectedNodeIndex = -1;
			_selectedNodeProp  = null;
			if (_assetSO != null)
				_currentTreeProp = _assetSO.FindProperty(TreePropNames[index]);
		}

		// ── Node List Panel ────────────────────────────────────────────────────────

		private void DrawNodeListPanel()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label("NODES", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			if (_currentTreeProp != null)
				GUILayout.Label($"{_currentTreeProp.arraySize}", EditorStyles.centeredGreyMiniLabel);
			GUILayout.Space(2f);
			if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(22f)))
				AddNode();
			GUI.enabled = _selectedNodeIndex >= 0;
			if (GUILayout.Button("–", EditorStyles.toolbarButton, GUILayout.Width(22f)))
			{
				RemoveSelectedNode();
				GUI.enabled = true;
				return;
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();

			if (_currentTreeProp == null) return;

			_nodeScroll = EditorGUILayout.BeginScrollView(_nodeScroll);
			for (int i = 0; i < _currentTreeProp.arraySize; i++)
				DrawNodeRow(i);
			EditorGUILayout.EndScrollView();
		}

		private void DrawNodeRow(int i)
		{
			SerializedProperty np       = _currentTreeProp.GetArrayElementAtIndex(i);
			SerializedProperty nodeIDp  = np.FindPropertyRelative("_nodeID");
			SerializedProperty modp     = np.FindPropertyRelative("_modifier");
			SerializedProperty costp    = np.FindPropertyRelative("_cost");
			bool selected = i == _selectedNodeIndex;

			string label = $"{SafeEnumName(nodeIDp)}  +{modp.intValue}  ({costp.intValue}g)";

			Rect rowRect = EditorGUILayout.GetControlRect(false, ROW_H);
			EditorGUI.DrawRect(rowRect, i % 2 == 0 ? RowEven : RowOdd);
			if (selected) EditorGUI.DrawRect(rowRect, SelectedBg);

			Rect labelRect = new Rect(rowRect.x + 10f,
				rowRect.y + (ROW_H - EditorGUIUtility.singleLineHeight) * 0.5f,
				rowRect.width - 14f, EditorGUIUtility.singleLineHeight);

			GUI.Label(labelRect, label, selected ? EditorStyles.whiteLabel : EditorStyles.label);

			if (Event.current.type == EventType.MouseDown && rowRect.Contains(Event.current.mousePosition))
			{
				SelectNode(i);
				Event.current.Use();
				Repaint();
			}
		}

		private void SelectNode(int index)
		{
			if (_selectedNodeIndex == index) return;
			_selectedNodeIndex = index;
			_selectedNodeProp  = _currentTreeProp.GetArrayElementAtIndex(index);
		}

		private void AddNode()
		{
			if (_currentTreeProp == null) return;
			int newIndex   = _currentTreeProp.arraySize;
			int defaultID  = (_selectedTree + 1) * 100 + (newIndex + 1);
			_currentTreeProp.arraySize++;
			SerializedProperty np = _currentTreeProp.GetArrayElementAtIndex(newIndex);
			np.FindPropertyRelative("_nodeID").enumValueIndex  = EnumValueIndexForID(defaultID);
			np.FindPropertyRelative("_modifier").intValue      = 1;
			np.FindPropertyRelative("_cost").intValue          = 10;
			np.FindPropertyRelative("_nextNodesIDs").ClearArray();
			_assetSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_asset);
			Repaint();
		}

		private static int EnumValueIndexForID(int intValue)
		{
			System.Array values = System.Enum.GetValues(typeof(SkillNode));
			for (int i = 0; i < values.Length; i++)
			{
				if ((int)values.GetValue(i) == intValue) return i;
			}
			return 0;
		}

		private void RemoveSelectedNode()
		{
			if (_currentTreeProp == null || _selectedNodeIndex < 0) return;
			_currentTreeProp.DeleteArrayElementAtIndex(_selectedNodeIndex);
			_assetSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_asset);
			ClearNodeSelection();
			Repaint();
		}

		// ── Node Detail Panel ──────────────────────────────────────────────────────

		private void DrawNodeDetailPanel()
		{
			string header = _selectedNodeProp != null
				? SafeEnumName(_selectedNodeProp.FindPropertyRelative("_nodeID"))
				: "NODE DETAIL";

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label(header, EditorStyles.boldLabel);
			GUILayout.EndHorizontal();

			if (_selectedNodeProp == null)
			{
				GUILayout.Space(12f);
				GUILayout.Label("← Select a node", EditorStyles.centeredGreyMiniLabel);
				return;
			}

			_detailScroll = EditorGUILayout.BeginScrollView(_detailScroll);
			GUILayout.Space(6f);

			DrawSection("Identity");
			EditorGUILayout.PropertyField(_selectedNodeProp.FindPropertyRelative("_nodeID"),  new GUIContent("Node ID"));
			EditorGUILayout.PropertyField(_selectedNodeProp.FindPropertyRelative("_stat"),    new GUIContent("Stat"));

			GUILayout.Space(8f);
			DrawSection("Values");
			EditorGUILayout.PropertyField(_selectedNodeProp.FindPropertyRelative("_modifier"), new GUIContent("Modifier  (+stat bonus)"));
			EditorGUILayout.PropertyField(_selectedNodeProp.FindPropertyRelative("_cost"),     new GUIContent("Cost  (gold)"));

			GUILayout.Space(8f);
			DrawSection("Connections  (children in tree)");
			EditorGUILayout.PropertyField(
				_selectedNodeProp.FindPropertyRelative("_nextNodesIDs"),
				new GUIContent("Next Node IDs"), true);

			GUILayout.Space(8f);
			EditorGUILayout.EndScrollView();
		}

		// ── Helpers ────────────────────────────────────────────────────────────────

		private void DrawSection(string title)
		{
			Rect r = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight + 4f);
			EditorGUI.DrawRect(r, SectionColor);
			EditorGUI.LabelField(new Rect(r.x + 6f, r.y + 2f, r.width, EditorGUIUtility.singleLineHeight),
				title, EditorStyles.boldLabel);
			GUILayout.Space(2f);
		}

		private void ClearNodeSelection()
		{
			_selectedNodeIndex = -1;
			_selectedNodeProp  = null;
		}

		private static string SafeEnumName(SerializedProperty enumProp)
		{
			int idx = enumProp.enumValueIndex;
			if (idx < 0 || idx >= enumProp.enumDisplayNames.Length)
				return $"(stale:{enumProp.intValue})";
			return enumProp.enumDisplayNames[idx];
		}
	}
}
