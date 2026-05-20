using Kapibara.RPS;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.Util.Editor
{
	/// <summary>
	/// Crea la jerarquía Combat_UIController en la escena activa.
	/// Menú: Kapibara/UI/Create Combat UI
	/// </summary>
	public static class CombatUICreator
	{
		[MenuItem("Kapibara/UI/Create Combat UI")]
		public static void CreateCombatUI()
		{
			// ── Root: Combat_UIController ─────────────────────────────────────────
			GameObject root = new GameObject("Combat_UIController");

			Canvas canvas         = root.AddComponent<Canvas>();
			canvas.renderMode     = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder   = 0;

			CanvasScaler scaler        = root.AddComponent<CanvasScaler>();
			scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scaler.referenceResolution = new Vector2(1920, 1080);
			scaler.screenMatchMode     = CanvasScaler.ScreenMatchMode.Expand;
			scaler.matchWidthOrHeight  = 0.5f;

			root.AddComponent<GraphicRaycaster>();
			root.AddComponent<CanvasGroup>();

			RectTransform rootRect = root.GetComponent<RectTransform>();
			Stretch(rootRect);

			// ── Background_Image ──────────────────────────────────────────────────
			RectTransform bg  = StretchChild("Background_Image", rootRect);
			Image bgImg       = bg.gameObject.AddComponent<Image>();
			bgImg.color       = Color.black;

			// ── Content ───────────────────────────────────────────────────────────
			RectTransform content = StretchChild("Content", rootRect);

			// ── TopBars ───────────────────────────────────────────────────────────
			RectTransform topBars = Section("TopBars", content, new Vector2(0f, 0.90f), new Vector2(1f, 1f));

			RectTransform plrBars = Section("PlayerBars", topBars, new Vector2(0f, 0f), new Vector2(0.5f, 1f));
			FillBar("PlayerHP_Bar",     plrBars, new Vector2(0f, 0.5f), new Vector2(1f, 1f),   new Color(0.10f, 0.80f, 0.20f));
			FillBar("PlayerEnergy_Bar", plrBars, new Vector2(0f, 0f),   new Vector2(1f, 0.5f), new Color(0.20f, 0.50f, 0.90f));

			RectTransform enmBars = Section("EnemyBars", topBars, new Vector2(0.5f, 0f), new Vector2(1f, 1f));
			FillBar("EnemyHP_Bar",     enmBars, new Vector2(0f, 0.5f), new Vector2(1f, 1f),   new Color(0.10f, 0.80f, 0.20f));
			FillBar("EnemyEnergy_Bar", enmBars, new Vector2(0f, 0f),   new Vector2(1f, 0.5f), new Color(0.20f, 0.50f, 0.90f));

			// ── Arena ─────────────────────────────────────────────────────────────
			RectTransform arena = Section("Arena", content, new Vector2(0f, 0.25f), new Vector2(1f, 0.90f));

			// Player Zone (left third)
			RectTransform playerZone = Section("PlayerZone", arena, new Vector2(0f, 0f), new Vector2(0.333f, 1f));
			ColoredBg(playerZone, new Color(0.87f, 0.45f, 0.69f));
			SpriteImage("PlayerSprite", playerZone);

			RectTransform playerThought = Section("PlayerThought_Bubble", playerZone, new Vector2(0f, 0.60f), new Vector2(0.40f, 1f));
			SpriteImage("ThoughtIcon", playerThought);

			RectTransform playerAction = Section("PlayerAction_Bubble", playerZone, new Vector2(0.50f, 0.55f), new Vector2(1f, 1f));
			SpriteImage("BubbleIcon", playerAction);
			TMP("DebugDamage_Text", playerAction, "0");

			// Center Zone
			RectTransform centerZone = Section("CenterZone", arena, new Vector2(0.333f, 0f), new Vector2(0.666f, 1f));
			ColoredBg(centerZone, new Color(0.95f, 0.65f, 0.55f));
			CenteredButton("Settings_Button", centerZone, new Vector2(120f, 120f));

			// Enemy Zone (right third)
			RectTransform enemyZone = Section("EnemyZone", arena, new Vector2(0.666f, 0f), new Vector2(1f, 1f));
			ColoredBg(enemyZone, new Color(0.35f, 0.15f, 0.45f));
			SpriteImage("EnemySprite", enemyZone);

			RectTransform enemyAction = Section("EnemyAction_Bubble", enemyZone, new Vector2(0f, 0.55f), new Vector2(0.50f, 1f));
			SpriteImage("BubbleIcon", enemyAction);
			TMP("DebugDamage_Text", enemyAction, "0");

			RectTransform enemyThought = Section("EnemyThought_Bubble", enemyZone, new Vector2(0.60f, 0.60f), new Vector2(1f, 1f));
			SpriteImage("ThoughtIcon", enemyThought);

			// ── ActionsPanel ──────────────────────────────────────────────────────
			RectTransform actions       = Section("ActionsPanel", content, new Vector2(0f, 0f), new Vector2(1f, 0.25f));
			HorizontalLayoutGroup hlg   = actions.gameObject.AddComponent<HorizontalLayoutGroup>();
			hlg.childForceExpandWidth   = true;
			hlg.childForceExpandHeight  = true;
			hlg.spacing                 = 8f;
			hlg.padding                 = new RectOffset(8, 8, 8, 8);

			ActionButton("Rock_Button",    actions);
			ActionButton("Paper_Button",   actions);
			ActionButton("Scissor_Button", actions);
			ActionButton("Defense_Button", actions);
			ActionButton("Energy_Button",  actions);

			Undo.RegisterCreatedObjectUndo(root, "Create Combat UI");
			Selection.activeGameObject = root;
			Debug.Log("[CombatUICreator] Combat_UIController creado. Asigna los campos en CombatUIController y guarda como prefab.");
		}

		// ── Helpers ───────────────────────────────────────────────────────────────

		private static RectTransform StretchChild(string name, RectTransform parent)
		{
			GameObject go   = new GameObject(name);
			RectTransform r = go.AddComponent<RectTransform>();
			r.SetParent(parent, false);
			Stretch(r);
			return r;
		}

		private static RectTransform Section(string name, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax)
		{
			GameObject go   = new GameObject(name);
			RectTransform r = go.AddComponent<RectTransform>();
			r.SetParent(parent, false);
			r.anchorMin = anchorMin;
			r.anchorMax = anchorMax;
			r.offsetMin = Vector2.zero;
			r.offsetMax = Vector2.zero;
			return r;
		}

		private static void ColoredBg(RectTransform parent, Color color)
		{
			RectTransform r = StretchChild("Background", parent);
			Image img       = r.gameObject.AddComponent<Image>();
			img.color       = color;
		}

		private static void SpriteImage(string name, RectTransform parent)
		{
			RectTransform r        = StretchChild(name, parent);
			Image img              = r.gameObject.AddComponent<Image>();
			img.preserveAspect     = true;
		}

		private static void TMP(string name, RectTransform parent, string placeholder)
		{
			RectTransform r         = StretchChild(name, parent);
			TextMeshProUGUI tmp     = r.gameObject.AddComponent<TextMeshProUGUI>();
			tmp.text                = placeholder;
			tmp.alignment           = TextAlignmentOptions.Center;
			tmp.fontSize            = 28;
		}

		private static void FillBar(string name, RectTransform parent, Vector2 anchorMin, Vector2 anchorMax, Color fillColor)
		{
			RectTransform bar = Section(name, parent, anchorMin, anchorMax);

			RectTransform bgR = StretchChild("Background", bar);
			Image bgImg       = bgR.gameObject.AddComponent<Image>();
			bgImg.color       = new Color(0.1f, 0.1f, 0.1f, 1f);

			RectTransform fR   = StretchChild("Fill", bar);
			Image fillImg      = fR.gameObject.AddComponent<Image>();
			fillImg.type       = Image.Type.Filled;
			fillImg.fillMethod = Image.FillMethod.Horizontal;
			fillImg.fillOrigin = 0;
			fillImg.fillAmount = 1f;
			fillImg.color      = fillColor;
		}

		private static void CenteredButton(string name, RectTransform parent, Vector2 size)
		{
			GameObject go          = new GameObject(name);
			RectTransform r        = go.AddComponent<RectTransform>();
			r.SetParent(parent, false);
			r.anchorMin            = new Vector2(0.5f, 0.5f);
			r.anchorMax            = new Vector2(0.5f, 0.5f);
			r.anchoredPosition     = Vector2.zero;
			r.sizeDelta            = size;

			Image img              = go.AddComponent<Image>();
			Button btn             = go.AddComponent<Button>();
			btn.targetGraphic      = img;

			RectTransform iconR    = StretchChild("Icon", r);
			Image iconImg          = iconR.gameObject.AddComponent<Image>();
			iconImg.preserveAspect = true;
		}

		private static void ActionButton(string name, RectTransform parent)
		{
			GameObject go          = new GameObject(name);
			RectTransform r        = go.AddComponent<RectTransform>();
			r.SetParent(parent, false);
			Image img              = go.AddComponent<Image>();
			Button btn             = go.AddComponent<Button>();
			btn.targetGraphic      = img;

			RectTransform iconR    = StretchChild("Icon", r);
			Image iconImg          = iconR.gameObject.AddComponent<Image>();
			iconImg.preserveAspect = true;
		}

		private static void Stretch(RectTransform r)
		{
			r.anchorMin = Vector2.zero;
			r.anchorMax = Vector2.one;
			r.offsetMin = Vector2.zero;
			r.offsetMax = Vector2.zero;
		}
	}
}
