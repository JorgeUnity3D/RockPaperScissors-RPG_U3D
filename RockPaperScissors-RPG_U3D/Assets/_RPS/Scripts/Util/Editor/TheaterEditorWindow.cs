using System.IO;
using Kapibara.RPS;
using UnityEditor;
using UnityEngine;

namespace Kapibara.Util.Editor
{
	/// <summary>
	/// Ventana de editor para gestionar historias del Theater sin navegar entre assets.
	/// Tres paneles: Historias | Páginas | Viñetas.
	/// Menú: Kapibara/Theater Story Editor
	/// </summary>
	public class TheaterEditorWindow : EditorWindow
	{
		// ── State ──────────────────────────────────────────────────────────────────

		private TheaterScrObj        _theater;
		private SerializedObject     _theaterSO;
		private SerializedProperty   _storiesProp;

		private int                  _selectedStoryIndex = -1;
		private ComicStoryScrObj     _selectedStory;
		private SerializedObject     _storySO;
		private SerializedProperty   _titleProp;
		private SerializedProperty   _thumbnailProp;
		private SerializedProperty   _pagesProp;

		private int                  _selectedPageIndex = -1;
		private ComicPageScrObj      _selectedPage;
		private SerializedObject     _pageSO;
		private SerializedProperty   _layoutProp;
		private SerializedProperty   _vignettesProp;

		// ── Scroll ─────────────────────────────────────────────────────────────────

		private Vector2 _storyScroll;
		private Vector2 _pageScroll;
		private Vector2 _vignetteScroll;

		// ── Layout constants ───────────────────────────────────────────────────────

		private const float HEADER_HEIGHT      = 50f;
		private const float STORY_PANEL_WIDTH  = 210f;
		private const float PAGE_PANEL_WIDTH   = 200f;
		private const float DIVIDER_WIDTH      = 2f;
		private const float THUMB_SIZE         = 40f;
		private const float ROW_HEIGHT         = 48f;

		private static readonly Color SelectedBg  = new Color(0.24f, 0.48f, 0.90f, 0.30f);
		private static readonly Color DividerColor = new Color(0.15f, 0.15f, 0.15f, 1f);
		private static readonly Color EmptyThumb   = new Color(0.28f, 0.28f, 0.28f, 0.50f);

		// ── Menu ───────────────────────────────────────────────────────────────────

		[MenuItem("Kapibara/Theater Story Editor")]
		public static void Open()
		{
			TheaterEditorWindow window = GetWindow<TheaterEditorWindow>("Theater Editor");
			window.minSize = new Vector2(700f, 460f);
			window.Show();
		}

		// ── OnGUI ──────────────────────────────────────────────────────────────────

		private void OnGUI()
		{
			DrawHeader();

			if (_theater == null)
			{
				Rect helpRect = new Rect(16f, HEADER_HEIGHT + 12f, position.width - 32f, 40f);
				EditorGUI.HelpBox(helpRect, "Asigna un TheaterScrObj para comenzar.", MessageType.Info);
				return;
			}

			_theaterSO.Update();
			if (_storySO != null) _storySO.Update();
			if (_pageSO  != null) _pageSO.Update();

			float contentY      = HEADER_HEIGHT;
			float contentHeight = position.height - HEADER_HEIGHT;
			float vigWidth      = position.width - STORY_PANEL_WIDTH - PAGE_PANEL_WIDTH - DIVIDER_WIDTH * 2f;

			Rect storyArea  = new Rect(0f,                                                contentY, STORY_PANEL_WIDTH, contentHeight);
			Rect div1Rect   = new Rect(STORY_PANEL_WIDTH,                                 contentY, DIVIDER_WIDTH,      contentHeight);
			Rect pageArea   = new Rect(STORY_PANEL_WIDTH + DIVIDER_WIDTH,                 contentY, PAGE_PANEL_WIDTH,  contentHeight);
			Rect div2Rect   = new Rect(STORY_PANEL_WIDTH + DIVIDER_WIDTH + PAGE_PANEL_WIDTH, contentY, DIVIDER_WIDTH,   contentHeight);
			Rect vigArea    = new Rect(div2Rect.xMax,                                     contentY, vigWidth,          contentHeight);

			EditorGUI.DrawRect(div1Rect, DividerColor);
			EditorGUI.DrawRect(div2Rect, DividerColor);

			GUILayout.BeginArea(storyArea);
			DrawStoriesPanel();
			GUILayout.EndArea();

			GUILayout.BeginArea(pageArea);
			DrawPagesPanel();
			GUILayout.EndArea();

			GUILayout.BeginArea(vigArea);
			DrawVignettesPanel();
			GUILayout.EndArea();

			_theaterSO.ApplyModifiedProperties();
			if (_storySO != null) _storySO.ApplyModifiedProperties();
			if (_pageSO  != null) _pageSO.ApplyModifiedProperties();
		}

		// ── Header ─────────────────────────────────────────────────────────────────

		private void DrawHeader()
		{
			GUILayout.BeginArea(new Rect(0f, 0f, position.width, HEADER_HEIGHT));
			GUILayout.Space(6f);

			EditorGUI.BeginChangeCheck();
			TheaterScrObj next = (TheaterScrObj)EditorGUILayout.ObjectField(
				"Theater Data", _theater, typeof(TheaterScrObj), false);
			if (EditorGUI.EndChangeCheck())
				SetTheater(next);

			GUILayout.Space(4f);
			EditorGUI.DrawRect(new Rect(0f, HEADER_HEIGHT - 1f, position.width, 1f), DividerColor);
			GUILayout.EndArea();
		}

		// ── SetTheater ─────────────────────────────────────────────────────────────

		private void SetTheater(TheaterScrObj theater)
		{
			_theater            = theater;
			_theaterSO          = theater != null ? new SerializedObject(theater) : null;
			_storiesProp        = _theaterSO?.FindProperty("_data");
			ClearStorySelection();
		}

		// ── Stories Panel ──────────────────────────────────────────────────────────

		private void DrawStoriesPanel()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label("HISTORIAS", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(22f)))
				AddStory();
			GUI.enabled = _selectedStoryIndex >= 0;
			if (GUILayout.Button("–", EditorStyles.toolbarButton, GUILayout.Width(22f)))
			{
				RemoveSelectedStory();
				GUI.enabled = true;
				return;
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();

			if (_storiesProp == null) return;

			_storyScroll = EditorGUILayout.BeginScrollView(_storyScroll,
				GUILayout.Height(position.height - HEADER_HEIGHT - 90f));

			for (int i = 0; i < _storiesProp.arraySize; i++)
				DrawStoryRow(i);

			EditorGUILayout.EndScrollView();

			// Story details: title + thumbnail below the list
			if (_selectedStory != null && _storySO != null)
			{
				EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), DividerColor);
				GUILayout.Space(2f);
				if (_titleProp     != null) EditorGUILayout.PropertyField(_titleProp,     new GUIContent("Título"));
				if (_thumbnailProp != null) EditorGUILayout.PropertyField(_thumbnailProp, new GUIContent("Miniatura"));
			}
		}

		private void DrawStoryRow(int i)
		{
			SerializedProperty storyRef = _storiesProp.GetArrayElementAtIndex(i);
			ComicStoryScrObj   story    = storyRef.objectReferenceValue as ComicStoryScrObj;
			bool               selected = i == _selectedStoryIndex;

			string title = story != null && story.Data != null && !string.IsNullOrEmpty(story.Data.Title)
				? story.Data.Title
				: $"Historia {i + 1}";

			Rect rowRect = EditorGUILayout.GetControlRect(false, ROW_HEIGHT);
			if (selected)
				EditorGUI.DrawRect(rowRect, SelectedBg);

			// Thumbnail
			Rect thumbRect = new Rect(rowRect.x + 4f, rowRect.y + 4f, THUMB_SIZE, THUMB_SIZE);
			Sprite thumb = story?.Data?.Thumbnail;
			if (thumb != null)
			{
				Texture2D preview = AssetPreview.GetAssetPreview(thumb);
				if (preview != null)
					GUI.DrawTexture(thumbRect, preview, ScaleMode.ScaleToFit);
				else
					EditorGUI.DrawRect(thumbRect, EmptyThumb);
			}
			else
			{
				EditorGUI.DrawRect(thumbRect, EmptyThumb);
			}

			// Label
			float labelX = thumbRect.xMax + 6f;
			Rect labelRect = new Rect(labelX, rowRect.y + (ROW_HEIGHT - EditorGUIUtility.singleLineHeight) * 0.5f,
				rowRect.xMax - labelX - 4f, EditorGUIUtility.singleLineHeight);
			GUI.Label(labelRect, title, selected ? EditorStyles.whiteLabel : EditorStyles.label);

			// Click
			if (Event.current.type == EventType.MouseDown && rowRect.Contains(Event.current.mousePosition))
			{
				SelectStory(i);
				Event.current.Use();
				Repaint();
			}
		}

		private void SelectStory(int index)
		{
			if (_selectedStoryIndex == index) return;
			_selectedStoryIndex = index;
			_selectedStory      = _storiesProp.GetArrayElementAtIndex(index).objectReferenceValue as ComicStoryScrObj;
			ClearPageSelection();

			if (_selectedStory != null)
			{
				_storySO            = new SerializedObject(_selectedStory);
				SerializedProperty d = _storySO.FindProperty("_data");
				_titleProp          = d.FindPropertyRelative("_title");
				_thumbnailProp      = d.FindPropertyRelative("_thumbnail");
				_pagesProp          = d.FindPropertyRelative("_pages");
			}
			else
			{
				_storySO       = null;
				_titleProp     = null;
				_thumbnailProp = null;
				_pagesProp     = null;
			}
		}

		private void AddStory()
		{
			string folder = EnsureSubfolder(AssetDatabase.GetAssetPath(_theater), "Stories");
			string path   = AssetDatabase.GenerateUniqueAssetPath(folder + "/Story.asset");
			ComicStoryScrObj story = CreateInstance<ComicStoryScrObj>();
			AssetDatabase.CreateAsset(story, path);
			AssetDatabase.SaveAssets();

			_theaterSO.Update();
			_storiesProp.arraySize++;
			_storiesProp.GetArrayElementAtIndex(_storiesProp.arraySize - 1).objectReferenceValue = story;
			_theaterSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_theater);
			Repaint();
		}

		private void RemoveSelectedStory()
		{
			_storiesProp.GetArrayElementAtIndex(_selectedStoryIndex).objectReferenceValue = null;
			_storiesProp.DeleteArrayElementAtIndex(_selectedStoryIndex);
			_theaterSO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_theater);
			ClearStorySelection();
			Repaint();
		}

		// ── Pages Panel ────────────────────────────────────────────────────────────

		private void DrawPagesPanel()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label("PÁGINAS", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			GUI.enabled = _selectedStoryIndex >= 0;
			if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(22f)))
				AddPage();
			GUI.enabled = _selectedStoryIndex >= 0 && _selectedPageIndex >= 0;
			if (GUILayout.Button("–", EditorStyles.toolbarButton, GUILayout.Width(22f)))
			{
				RemoveSelectedPage();
				GUI.enabled = true;
				return;
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();

			if (_selectedStoryIndex < 0 || _pagesProp == null)
			{
				GUILayout.Space(12f);
				GUILayout.Label("← Selecciona una historia", EditorStyles.centeredGreyMiniLabel);
				return;
			}

			_pageScroll = EditorGUILayout.BeginScrollView(_pageScroll);

			for (int i = 0; i < _pagesProp.arraySize; i++)
				DrawPageRow(i);

			EditorGUILayout.EndScrollView();
		}

		private void DrawPageRow(int i)
		{
			SerializedProperty pageRef = _pagesProp.GetArrayElementAtIndex(i);
			ComicPageScrObj    page    = pageRef.objectReferenceValue as ComicPageScrObj;
			bool               selected = i == _selectedPageIndex;

			int    vigCount     = page?.Data?.Vignettes?.Count ?? 0;
			string layoutLabel  = page?.Data != null ? page.Data.Layout.ToString().Replace("_", " ") : "—";
			string label        = $"P{i + 1}  {layoutLabel}  ({vigCount}v)";

			float rowH   = EditorGUIUtility.singleLineHeight + 6f;
			Rect rowRect = EditorGUILayout.GetControlRect(false, rowH);
			if (selected)
				EditorGUI.DrawRect(rowRect, SelectedBg);

			Rect lr = new Rect(rowRect.x + 6f, rowRect.y + 3f, rowRect.width - 8f, EditorGUIUtility.singleLineHeight);
			GUI.Label(lr, label, selected ? EditorStyles.whiteLabel : EditorStyles.label);

			if (Event.current.type == EventType.MouseDown && rowRect.Contains(Event.current.mousePosition))
			{
				SelectPage(i);
				Event.current.Use();
				Repaint();
			}
		}

		private void SelectPage(int index)
		{
			if (_selectedPageIndex == index) return;
			_selectedPageIndex = index;
			_selectedPage      = _pagesProp.GetArrayElementAtIndex(index).objectReferenceValue as ComicPageScrObj;

			if (_selectedPage != null)
			{
				_pageSO               = new SerializedObject(_selectedPage);
				SerializedProperty d  = _pageSO.FindProperty("_data");
				_layoutProp           = d.FindPropertyRelative("_layout");
				_vignettesProp        = d.FindPropertyRelative("_vignettes");
			}
			else
			{
				_pageSO        = null;
				_layoutProp    = null;
				_vignettesProp = null;
			}
		}

		private void AddPage()
		{
			if (_selectedStory == null) return;
			string storyPath = AssetDatabase.GetAssetPath(_selectedStory);
			string storyName = Path.GetFileNameWithoutExtension(storyPath);
			string folder    = EnsureSubfolder(storyPath, "Pages");
			string path      = AssetDatabase.GenerateUniqueAssetPath(folder + "/" + storyName + "_Page.asset");
			ComicPageScrObj page = CreateInstance<ComicPageScrObj>();
			AssetDatabase.CreateAsset(page, path);
			AssetDatabase.SaveAssets();

			_storySO.Update();
			_pagesProp.arraySize++;
			_pagesProp.GetArrayElementAtIndex(_pagesProp.arraySize - 1).objectReferenceValue = page;
			_storySO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_selectedStory);
			Repaint();
		}

		private void RemoveSelectedPage()
		{
			_pagesProp.GetArrayElementAtIndex(_selectedPageIndex).objectReferenceValue = null;
			_pagesProp.DeleteArrayElementAtIndex(_selectedPageIndex);
			_storySO.ApplyModifiedProperties();
			EditorUtility.SetDirty(_selectedStory);
			ClearPageSelection();
			Repaint();
		}

		// ── Vignettes Panel ────────────────────────────────────────────────────────

		private void DrawVignettesPanel()
		{
			// Toolbar
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			string header = _selectedPage != null
				? $"VIÑETAS  —  Página {_selectedPageIndex + 1}"
				: "VIÑETAS";
			GUILayout.Label(header, EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			GUI.enabled = _selectedPageIndex >= 0 && _vignettesProp != null;
			if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(22f)))
			{
				_vignettesProp.arraySize++;
				_pageSO.ApplyModifiedProperties();
				EditorUtility.SetDirty(_selectedPage);
			}
			GUI.enabled = _selectedPageIndex >= 0 && _vignettesProp != null && _vignettesProp.arraySize > 0;
			if (GUILayout.Button("–", EditorStyles.toolbarButton, GUILayout.Width(22f)))
			{
				_vignettesProp.arraySize--;
				_pageSO.ApplyModifiedProperties();
				EditorUtility.SetDirty(_selectedPage);
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();

			if (_selectedPageIndex < 0 || _pageSO == null || _vignettesProp == null)
			{
				GUILayout.Space(12f);
				GUILayout.Label("← Selecciona una página", EditorStyles.centeredGreyMiniLabel);
				return;
			}

			// Layout picker
			GUILayout.Space(4f);
			EditorGUILayout.PropertyField(_layoutProp, new GUIContent("Layout de página"));
			GUILayout.Space(4f);
			EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1f), DividerColor);
			GUILayout.Space(4f);

			// Vignette cards
			_vignetteScroll = EditorGUILayout.BeginScrollView(_vignetteScroll);

			for (int i = 0; i < _vignettesProp.arraySize; i++)
				DrawVignetteCard(i);

			EditorGUILayout.EndScrollView();
		}

		private void DrawVignetteCard(int i)
		{
			SerializedProperty vig      = _vignettesProp.GetArrayElementAtIndex(i);
			SerializedProperty spritePr = vig.FindPropertyRelative("_sprite");
			SerializedProperty textPr   = vig.FindPropertyRelative("_dialogText");
			SerializedProperty animPr   = vig.FindPropertyRelative("_animation");

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			EditorGUILayout.LabelField($"Slot_{i}", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();

			// Sprite preview (64×64)
			Rect previewRect = EditorGUILayout.GetControlRect(false, 64f, GUILayout.Width(64f));
			Sprite sprite = spritePr.objectReferenceValue as Sprite;
			if (sprite != null)
			{
				Texture2D preview = AssetPreview.GetAssetPreview(sprite);
				if (preview != null)
					GUI.DrawTexture(previewRect, preview, ScaleMode.ScaleToFit);
				else
					EditorGUI.DrawRect(previewRect, EmptyThumb);
			}
			else
			{
				EditorGUI.DrawRect(previewRect, EmptyThumb);
				GUI.Label(previewRect, "sin sprite", EditorStyles.centeredGreyMiniLabel);
			}

			// Fields
			EditorGUILayout.BeginVertical();
			EditorGUILayout.PropertyField(spritePr, new GUIContent("Sprite"));
			EditorGUILayout.PropertyField(animPr,   new GUIContent("Animación"));
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(textPr, new GUIContent("Diálogo"));
			EditorGUILayout.EndVertical();
			GUILayout.Space(3f);
		}

		// ── Helpers ────────────────────────────────────────────────────────────────

		private void ClearStorySelection()
		{
			_selectedStoryIndex = -1;
			_selectedStory      = null;
			_storySO            = null;
			_titleProp          = null;
			_thumbnailProp      = null;
			_pagesProp          = null;
			ClearPageSelection();
		}

		private void ClearPageSelection()
		{
			_selectedPageIndex = -1;
			_selectedPage      = null;
			_pageSO            = null;
			_layoutProp        = null;
			_vignettesProp     = null;
		}

		private static string EnsureSubfolder(string assetPath, string subfolder)
		{
			string parent = Path.GetDirectoryName(assetPath);
			string full   = parent + "/" + subfolder;
			if (!AssetDatabase.IsValidFolder(full))
				AssetDatabase.CreateFolder(parent, subfolder);
			return full;
		}
	}
}
