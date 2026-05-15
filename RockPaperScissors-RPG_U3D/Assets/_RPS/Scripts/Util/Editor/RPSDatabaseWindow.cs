using System;
using System.Collections.Generic;
using Kapibara.RPS;
using UnityEditor;
using UnityEngine;

namespace Kapibara.Util.Editor
{
	public class RPSDatabaseWindow : EditorWindow
	{
		// ── Category definition ────────────────────────────────────────────────────

		private class AssetCategory
		{
			public string Group;
			public string Label;
			public Type   AssetType;
			public string Folder;
			public string DefaultName;
		}

		private static readonly AssetCategory[] s_categories =
		{
			new AssetCategory { Group = "ENEMIES", Label = "Enemies",        AssetType = typeof(EnemyScrObj),              Folder = "Enemies",      DefaultName = "Enemy"              },
			new AssetCategory { Group = "ENEMIES", Label = "Languages",      AssetType = typeof(LanguageScrObj),           Folder = "Languages",    DefaultName = "Language"           },
			new AssetCategory { Group = "LEVELS",  Label = "Map Levels",     AssetType = typeof(MapLevelScrObj),           Folder = "Levels",       DefaultName = "MapLevels"          },
			new AssetCategory { Group = "PLAYER",  Label = "Stat Icons",     AssetType = typeof(IconsScrObj),              Folder = "Player",       DefaultName = "StatIcons"          },
			new AssetCategory { Group = "TOWN",    Label = "Credits",        AssetType = typeof(CreditsTimeCounterScrObj), Folder = "Town",         DefaultName = "CreditsTimeCounter" },
			new AssetCategory { Group = "TOWN",    Label = "PaperTree",      AssetType = typeof(PaperTreeScrObj),          Folder = "Town",         DefaultName = "PaperTreeSkillTrees"},
			new AssetCategory { Group = "TOWN",    Label = "StoneSmithy",    AssetType = typeof(StoneSmithyScrObj),        Folder = "Town",         DefaultName = "StoneSmithyData"    },
			new AssetCategory { Group = "TOWN",    Label = "Town Views",     AssetType = typeof(TownViewScrObj),           Folder = "Town",         DefaultName = "TownViews"          },
			new AssetCategory { Group = "TOWN",    Label = "Library",        AssetType = typeof(LibraryScrObj),            Folder = "Town",         DefaultName = "Library"            },
			new AssetCategory { Group = "THEATER", Label = "Theater Data",   AssetType = typeof(TheaterScrObj),            Folder = "Town/Theater", DefaultName = "TheaterData"        },
			new AssetCategory { Group = "THEATER", Label = "Comic Stories",  AssetType = typeof(ComicStoryScrObj),         Folder = "Town/Theater", DefaultName = "ComicStory"         },
			new AssetCategory { Group = "THEATER", Label = "Comic Pages",    AssetType = typeof(ComicPageScrObj),          Folder = "Town/Theater", DefaultName = "ComicPage"          },
		};

		private const string BASE_PATH = "Assets/_RPS/ScriptableObjects";

		// ── Sub-view ───────────────────────────────────────────────────────────────

		private enum SubView { None, MapLevel, Theater }

		private SubView              _subView         = SubView.None;
		private MapLevelEditorWindow _mapLevelEdInst;
		private TheaterEditorWindow  _theaterEdInst;

		// ── State ──────────────────────────────────────────────────────────────────

		private int                    _selectedCategoryIndex = 0;
		private List<ScriptableObject> _assets                = new List<ScriptableObject>();
		private string                 _newAssetName          = "";
		private Vector2                _categoryScroll;
		private Vector2                _assetScroll;

		private ScriptableObject   _editingAsset      = null;
		private int                _editingAssetIndex = -1;
		private UnityEditor.Editor _embeddedEditor    = null;
		private Vector2            _detailScroll;

		// ── Layout ─────────────────────────────────────────────────────────────────

		private const float HEADER_H   = 44f;
		private const float CATEGORY_W = 175f;
		private const float DIVIDER_W  = 2f;
		private const float ROW_H      = 28f;
		private const float CAT_ROW_H  = 26f;
		private const float GROUP_H    = 20f;

		private static readonly Color SelectedBg   = new Color(0.24f, 0.48f, 0.90f, 0.30f);
		private static readonly Color DividerColor = new Color(0.15f, 0.15f, 0.15f, 1f);
		private static readonly Color GroupBg      = new Color(0.18f, 0.18f, 0.18f, 1f);
		private static readonly Color RowEven      = new Color(1f, 1f, 1f, 0.02f);
		private static readonly Color RowOdd       = new Color(0f, 0f, 0f, 0.08f);

		// ── Menu ───────────────────────────────────────────────────────────────────

		[MenuItem("Kapibara/RPSRPG Database")]
		public static void Open()
		{
			RPSDatabaseWindow w = GetWindow<RPSDatabaseWindow>("RPSRPG Database");
			w.minSize = new Vector2(900f, 620f);
			w.maxSize = new Vector2(900f, 620f);
			w.Show();
		}

		// ── Lifecycle ──────────────────────────────────────────────────────────────

		private void OnEnable()
		{
			_newAssetName = s_categories[_selectedCategoryIndex].DefaultName;
			RefreshAssets();
			EditorApplication.projectChanged += OnProjectChanged;
		}

		private void OnDisable()
		{
			EditorApplication.projectChanged -= OnProjectChanged;
			DestroyEmbeddedEditor();
			DestroySubViewInstances();
		}

		private void OnProjectChanged()
		{
			if (_editingAsset == null)
				CloseDetailView();
			RefreshAssets();
			Repaint();
		}

		// ── OnGUI ──────────────────────────────────────────────────────────────────

		private void OnGUI()
		{
			DrawHeader();

			float contentY = HEADER_H;
			float contentH = position.height - HEADER_H;

			if (_subView != SubView.None)
			{
				GUILayout.BeginArea(new Rect(0f, contentY, position.width, contentH));
				DrawSubView(position.width, contentH);
				GUILayout.EndArea();
				return;
			}

			float assetPanW = position.width - CATEGORY_W - DIVIDER_W;

			Rect categoryArea = new Rect(0f,                     contentY, CATEGORY_W, contentH);
			Rect divider      = new Rect(CATEGORY_W,             contentY, DIVIDER_W,  contentH);
			Rect assetArea    = new Rect(CATEGORY_W + DIVIDER_W, contentY, assetPanW,  contentH);

			EditorGUI.DrawRect(divider, DividerColor);

			GUILayout.BeginArea(categoryArea);
			DrawCategoryPanel();
			GUILayout.EndArea();

			GUILayout.BeginArea(assetArea);
			DrawAssetPanel();
			GUILayout.EndArea();
		}

		// ── Header ─────────────────────────────────────────────────────────────────

		private void DrawHeader()
		{
			GUILayout.BeginArea(new Rect(0f, 0f, position.width, HEADER_H));
			GUILayout.Space(8f);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10f);

			if (_subView != SubView.None)
			{
				if (GUILayout.Button("← Back", EditorStyles.miniButton, GUILayout.Height(20f)))
					CloseSubView();
				GUILayout.Space(6f);
				string subLabel = _subView == SubView.MapLevel ? "Map Level Editor" : "Theater Editor";
				GUILayout.Label(subLabel, EditorStyles.boldLabel);
			}
			else
			{
				GUILayout.Label("RPSRPG Database", EditorStyles.boldLabel);
			}

			GUILayout.FlexibleSpace();

			if (_subView == SubView.None)
			{
				if (GUILayout.Button("Map Level Editor", EditorStyles.miniButton, GUILayout.Height(20f)))
					OpenSubView(SubView.MapLevel);
				GUILayout.Space(4f);
				if (GUILayout.Button("Theater Editor", EditorStyles.miniButton, GUILayout.Height(20f)))
					OpenSubView(SubView.Theater);
			}

			GUILayout.Space(10f);
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(4f);
			EditorGUI.DrawRect(new Rect(0f, HEADER_H - 1f, position.width, 1f), DividerColor);
			GUILayout.EndArea();
		}

		// ── Sub-view ───────────────────────────────────────────────────────────────

		private void OpenSubView(SubView view)
		{
			_subView = view;
			if (view == SubView.MapLevel && _mapLevelEdInst == null)
				_mapLevelEdInst = CreateInstance<MapLevelEditorWindow>();
			else if (view == SubView.Theater && _theaterEdInst == null)
				_theaterEdInst = CreateInstance<TheaterEditorWindow>();
		}

		private void CloseSubView()
		{
			_subView = SubView.None;
		}

		private void DrawSubView(float w, float h)
		{
			if (_subView == SubView.MapLevel && _mapLevelEdInst != null)
				_mapLevelEdInst.DrawEmbedded(w, h);
			else if (_subView == SubView.Theater && _theaterEdInst != null)
				_theaterEdInst.DrawEmbedded(w, h);
		}

		private void DestroySubViewInstances()
		{
			if (_mapLevelEdInst != null) { DestroyImmediate(_mapLevelEdInst); _mapLevelEdInst = null; }
			if (_theaterEdInst  != null) { DestroyImmediate(_theaterEdInst);  _theaterEdInst  = null; }
		}

		// ── Category Panel ─────────────────────────────────────────────────────────

		private void DrawCategoryPanel()
		{
			_categoryScroll = EditorGUILayout.BeginScrollView(_categoryScroll);

			string currentGroup = null;
			for (int i = 0; i < s_categories.Length; i++)
			{
				AssetCategory cat = s_categories[i];

				if (cat.Group != currentGroup)
				{
					currentGroup = cat.Group;
					Rect groupRect = EditorGUILayout.GetControlRect(false, GROUP_H);
					EditorGUI.DrawRect(groupRect, GroupBg);
					GUI.Label(new Rect(groupRect.x + 8f, groupRect.y + 2f, groupRect.width, 16f),
						cat.Group, EditorStyles.centeredGreyMiniLabel);
				}

				bool selected = i == _selectedCategoryIndex;
				Rect rowRect  = EditorGUILayout.GetControlRect(false, CAT_ROW_H);
				if (selected) EditorGUI.DrawRect(rowRect, SelectedBg);

				Rect labelRect = new Rect(rowRect.x + 14f,
					rowRect.y + (CAT_ROW_H - EditorGUIUtility.singleLineHeight) * 0.5f,
					rowRect.width - 16f, EditorGUIUtility.singleLineHeight);
				GUI.Label(labelRect, cat.Label, selected ? EditorStyles.whiteLabel : EditorStyles.label);

				if (Event.current.type == EventType.MouseDown && rowRect.Contains(Event.current.mousePosition))
				{
					SelectCategory(i);
					Event.current.Use();
					Repaint();
				}
			}

			EditorGUILayout.EndScrollView();
		}

		// ── Asset Panel ────────────────────────────────────────────────────────────

		private void DrawAssetPanel()
		{
			if (_editingAsset != null)
			{
				DrawDetailView();
				return;
			}

			DrawListView();
		}

		private void DrawListView()
		{
			AssetCategory cat = s_categories[_selectedCategoryIndex];

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label(cat.Label.ToUpper(), EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			GUILayout.Label($"{_assets.Count}", EditorStyles.centeredGreyMiniLabel);
			GUILayout.Space(2f);
			if (GUILayout.Button("↺", EditorStyles.toolbarButton, GUILayout.Width(22f)))
				RefreshAssets();
			EditorGUILayout.EndHorizontal();

			float listH = position.height - HEADER_H - 58f;
			_assetScroll = EditorGUILayout.BeginScrollView(_assetScroll, GUILayout.Height(listH));

			if (_assets.Count == 0)
			{
				GUILayout.Space(12f);
				GUILayout.Label("No assets found.", EditorStyles.centeredGreyMiniLabel);
			}

			ScriptableObject toEdit   = null;
			ScriptableObject toDelete = null;

			for (int i = 0; i < _assets.Count; i++)
			{
				ScriptableObject asset = _assets[i];
				if (asset == null) continue;
				Rect rowRect    = EditorGUILayout.GetControlRect(false, ROW_H);
				EditorGUI.DrawRect(rowRect, i % 2 == 0 ? RowEven : RowOdd);
				float btnY      = rowRect.y + 4f;
				float btnH      = ROW_H - 8f;
				Rect deleteRect = new Rect(rowRect.xMax - 22f,  btnY, 18f, btnH);
				Rect editRect   = new Rect(rowRect.xMax - 80f,  btnY, 54f, btnH);
				Rect pingRect   = new Rect(rowRect.xMax - 118f, btnY, 34f, btnH);
				Rect nameRect   = new Rect(rowRect.x + 10f,
					rowRect.y + (ROW_H - EditorGUIUtility.singleLineHeight) * 0.5f,
					rowRect.width - 126f, EditorGUIUtility.singleLineHeight);

				GUI.Label(nameRect, asset.name, EditorStyles.label);
				if (GUI.Button(pingRect,   "Ping", EditorStyles.miniButton)) EditorGUIUtility.PingObject(asset);
				if (GUI.Button(editRect,   "Edit", EditorStyles.miniButton)) toEdit   = asset;
				if (GUI.Button(deleteRect, "X",    EditorStyles.miniButton)) toDelete = asset;
			}

			if (toEdit   != null) OpenDetailView(toEdit);
			if (toDelete != null) DeleteAsset(toDelete);

			EditorGUILayout.EndScrollView();

			EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), DividerColor);
			GUILayout.Space(4f);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(8f);
			_newAssetName = EditorGUILayout.TextField(_newAssetName, GUILayout.ExpandWidth(true));
			GUILayout.Space(4f);
			if (GUILayout.Button("+ Create", GUILayout.Width(70f)))
				CreateAsset(cat);
			GUILayout.Space(8f);
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(6f);
		}

		private void DrawDetailView()
		{
			bool goBack   = false;
			bool goPrev   = false;
			bool goNext   = false;
			bool hasPrev  = _editingAssetIndex > 0;
			bool hasNext  = _editingAssetIndex < _assets.Count - 1;

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			if (GUILayout.Button("← Back", EditorStyles.toolbarButton, GUILayout.Width(60f)))
				goBack = true;
			GUILayout.Space(6f);
			GUILayout.Label(_editingAsset.name, EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			EditorGUI.BeginDisabledGroup(!hasPrev);
			if (GUILayout.Button("< Prev", EditorStyles.toolbarButton, GUILayout.Width(50f)))
				goPrev = true;
			EditorGUI.EndDisabledGroup();
			EditorGUI.BeginDisabledGroup(!hasNext);
			if (GUILayout.Button("Next >", EditorStyles.toolbarButton, GUILayout.Width(50f)))
				goNext = true;
			EditorGUI.EndDisabledGroup();
			GUILayout.Space(6f);
			if (GUILayout.Button("Ping", EditorStyles.toolbarButton, GUILayout.Width(40f)))
			{
				Selection.activeObject = _editingAsset;
				EditorGUIUtility.PingObject(_editingAsset);
			}
			EditorGUILayout.EndHorizontal();

			if (goBack)  { CloseDetailView(); return; }
			if (goPrev)  { OpenDetailView(_assets[_editingAssetIndex - 1]); return; }
			if (goNext)  { OpenDetailView(_assets[_editingAssetIndex + 1]); return; }

			EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), DividerColor);
			GUILayout.Space(4f);

			_detailScroll = EditorGUILayout.BeginScrollView(_detailScroll);
			if (_embeddedEditor != null)
				_embeddedEditor.OnInspectorGUI();
			EditorGUILayout.EndScrollView();
		}

		// ── Helpers ────────────────────────────────────────────────────────────────

		private void SelectCategory(int index)
		{
			if (_selectedCategoryIndex == index) return;
			CloseDetailView();
			_selectedCategoryIndex = index;
			_newAssetName          = s_categories[index].DefaultName;
			RefreshAssets();
		}

		private void OpenDetailView(ScriptableObject asset)
		{
			DestroyEmbeddedEditor();
			_editingAsset      = asset;
			_editingAssetIndex = _assets.IndexOf(asset);
			_embeddedEditor    = UnityEditor.Editor.CreateEditor(asset);
			_detailScroll      = Vector2.zero;
		}

		private void CloseDetailView()
		{
			DestroyEmbeddedEditor();
			_editingAsset      = null;
			_editingAssetIndex = -1;
		}

		private void DestroyEmbeddedEditor()
		{
			if (_embeddedEditor != null)
			{
				DestroyImmediate(_embeddedEditor);
				_embeddedEditor = null;
			}
		}

		private void RefreshAssets()
		{
			_assets.Clear();
			AssetCategory cat    = s_categories[_selectedCategoryIndex];
			string        folder = $"{BASE_PATH}/{cat.Folder}";
			string[]      guids  = AssetDatabase.FindAssets($"t:{cat.AssetType.Name}", new[] { folder });
			foreach (string guid in guids)
			{
				string           path  = AssetDatabase.GUIDToAssetPath(guid);
				ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
				if (asset != null) _assets.Add(asset);
			}
		}

		private void CreateAsset(AssetCategory cat)
		{
			string name     = string.IsNullOrWhiteSpace(_newAssetName) ? cat.DefaultName : _newAssetName.Trim();
			string folder   = $"{BASE_PATH}/{cat.Folder}";
			string path     = FindUniqueAssetPath(folder, name);

			ScriptableObject instance = ScriptableObject.CreateInstance(cat.AssetType);
			AssetDatabase.CreateAsset(instance, path);
			AssetDatabase.SaveAssets();
			EditorGUIUtility.PingObject(instance);
			Selection.activeObject = instance;
			RefreshAssets();
			Repaint();
		}

		private void DeleteAsset(ScriptableObject asset)
		{
			bool confirm = EditorUtility.DisplayDialog(
				"Delete asset",
				$"Delete '{asset.name}'?\nThis cannot be undone.",
				"Delete", "Cancel");
			if (!confirm) return;

			if (_editingAsset == asset)
				CloseDetailView();

			string path = AssetDatabase.GetAssetPath(asset);
			AssetDatabase.DeleteAsset(path);
			RefreshAssets();
			Repaint();
		}

		private static string FindUniqueAssetPath(string folder, string baseName)
		{
			string path = $"{folder}/{baseName}.asset";
			if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) == null)
				return path;
			for (int i = 1; i <= 99; i++)
			{
				path = $"{folder}/{baseName}_{i:D2}.asset";
				if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) == null)
					return path;
			}
			return AssetDatabase.GenerateUniqueAssetPath($"{folder}/{baseName}.asset");
		}
	}
}
