using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.Util.Editor
{
	/// <summary>
	/// Crea en la escena activa los GameObjects raíz para cada ComicPageLayout.
	/// Cada root tiene hijos nombrados Slot_0, Slot_1... con Image + CanvasGroup,
	/// posicionados mediante anclas para cubrir exactamente su celda.
	/// Úsalos para crear los prefabs de _layoutPrefabs en ComicPlayerUIController.
	/// Menú: Kapibara/UI/Create Comic Layouts
	/// </summary>
	public static class ComicLayoutCreator
	{
		private struct SlotAnchor
		{
			public Vector2 min;
			public Vector2 max;

			public SlotAnchor(float xMin, float yMin, float xMax, float yMax)
			{
				min = new Vector2(xMin, yMin);
				max = new Vector2(xMax, yMax);
			}
		}

		[MenuItem("Kapibara/UI/Create Comic Layouts")]
		public static void CreateLayouts()
		{
			// 0 - One_Full
			CreateLayout("ComicLayout_One_Full", new SlotAnchor[]
			{
				new SlotAnchor(0f, 0f, 1f, 1f)
			});

			// 1 - Two_Horizontal
			CreateLayout("ComicLayout_Two_Horizontal", new SlotAnchor[]
			{
				new SlotAnchor(0f,   0f, 0.5f, 1f),
				new SlotAnchor(0.5f, 0f, 1f,   1f)
			});

			// 2 - Two_Vertical
			CreateLayout("ComicLayout_Two_Vertical", new SlotAnchor[]
			{
				new SlotAnchor(0f, 0.5f, 1f, 1f),
				new SlotAnchor(0f, 0f,   1f, 0.5f)
			});

			// 3 - Three_TopOne_BottomTwo
			CreateLayout("ComicLayout_Three_TopOne_BottomTwo", new SlotAnchor[]
			{
				new SlotAnchor(0f,   0.5f, 1f,   1f),
				new SlotAnchor(0f,   0f,   0.5f, 0.5f),
				new SlotAnchor(0.5f, 0f,   1f,   0.5f)
			});

			// 4 - Three_TopTwo_BottomOne
			CreateLayout("ComicLayout_Three_TopTwo_BottomOne", new SlotAnchor[]
			{
				new SlotAnchor(0f,   0.5f, 0.5f, 1f),
				new SlotAnchor(0.5f, 0.5f, 1f,   1f),
				new SlotAnchor(0f,   0f,   1f,   0.5f)
			});

			// 5 - Four_Grid (2x2)
			CreateLayout("ComicLayout_Four_Grid", new SlotAnchor[]
			{
				new SlotAnchor(0f,   0.5f, 0.5f, 1f),
				new SlotAnchor(0.5f, 0.5f, 1f,   1f),
				new SlotAnchor(0f,   0f,   0.5f, 0.5f),
				new SlotAnchor(0.5f, 0f,   1f,   0.5f)
			});

			// 6 - Six_Grid (2 filas x 3 columnas)
			CreateLayout("ComicLayout_Six_Grid", new SlotAnchor[]
			{
				new SlotAnchor(0f,      0.5f, 0.333f, 1f),
				new SlotAnchor(0.333f,  0.5f, 0.667f, 1f),
				new SlotAnchor(0.667f,  0.5f, 1f,     1f),
				new SlotAnchor(0f,      0f,   0.333f, 0.5f),
				new SlotAnchor(0.333f,  0f,   0.667f, 0.5f),
				new SlotAnchor(0.667f,  0f,   1f,     0.5f)
			});

			Debug.Log("[ComicLayoutCreator] 7 layouts creados en la escena. Arrastra cada uno a la carpeta de prefabs.");
		}

		private static void CreateLayout(string rootName, SlotAnchor[] slots)
		{
			GameObject root = new GameObject(rootName);
			RectTransform rootRect = root.AddComponent<RectTransform>();
			rootRect.anchorMin = Vector2.zero;
			rootRect.anchorMax = Vector2.one;
			rootRect.offsetMin = Vector2.zero;
			rootRect.offsetMax = Vector2.zero;

			for (int i = 0; i < slots.Length; i++)
			{
				GameObject slot = new GameObject($"Slot_{i}");
				RectTransform slotRect = slot.AddComponent<RectTransform>();
				slotRect.SetParent(rootRect, false);

				slotRect.anchorMin = slots[i].min;
				slotRect.anchorMax = slots[i].max;
				slotRect.offsetMin = Vector2.zero;
				slotRect.offsetMax = Vector2.zero;
				slotRect.pivot     = new Vector2(0.5f, 0.5f);

				slot.AddComponent<Image>();
				slot.AddComponent<CanvasGroup>();
			}

			Undo.RegisterCreatedObjectUndo(root, $"Create {rootName}");
		}
	}
}
