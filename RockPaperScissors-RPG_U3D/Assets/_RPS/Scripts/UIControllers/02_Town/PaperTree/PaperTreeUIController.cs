using System.Collections.Generic;
using Kapibara.Util.Extensions;
using Kapibara.UI;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Kapibara.RPS
{
	/// <summary>
	/// Vista del árbol de habilidades Paper Tree. Instancia botones y líneas de conexión para cada árbol de estadísticas.
	/// </summary>
	public class PaperTreeUIController : UIController
	{

		[Header("UI")]
		[SerializeField] private GameObject _linePrefab;
		[SerializeField] private List<Color> _lineColors;
		[SerializeField] private List<PaperTreeButton> _rockPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _paperPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _scissorsPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _defensePaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _energyRecPaperTreeButtons;

		private IconsDictionary _icons;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		#endregion

		#region SETUP

		public override void SetUp()
		{
			Debug.Log($"[PaperTreeUIController] SetUp() -> ");
			HideCanvas(0);
		}

		public void SetData(List<StatAttribute> attributes, PaperTreeScrObj paperTreeScrObj, IconsDictionary icons, int playerGold)
		{
			Debug.Log($"[PaperTreeUIController] SetData() -> ");
			_icons = icons;
			SetUpTree(attributes, paperTreeScrObj, Stats.ROCK,            _rockPaperTreeButtons,      playerGold);
			SetUpTree(attributes, paperTreeScrObj, Stats.PAPER,           _paperPaperTreeButtons,     playerGold);
			SetUpTree(attributes, paperTreeScrObj, Stats.SCISSOR,         _scissorsPaperTreeButtons,  playerGold);
			SetUpTree(attributes, paperTreeScrObj, Stats.DEFENSE,         _defensePaperTreeButtons,   playerGold);
			SetUpTree(attributes, paperTreeScrObj, Stats.ENERGY_RECOVERY, _energyRecPaperTreeButtons, playerGold);
		}

		#endregion

		#region CONTROL

		private void SetUpTree(List<StatAttribute> attributes, PaperTreeScrObj scrObj, Stats stat, List<PaperTreeButton> buttons, int playerGold)
		{
			StatAttribute attribute = attributes.Find(a => a.Stat == stat);
			List<SkillNode> unlockedNodes = attribute?.GetModifier<PaperTreeModifier>()?.UnlockedNodes ?? new List<SkillNode>();
			List<PaperTreeNode> tree = scrObj[stat];
			SetUpSkillNodes(buttons, tree, unlockedNodes);
			SetUpSkillTreeLines(buttons, tree, unlockedNodes, playerGold);
		}

		private void SetUpSkillNodes(List<PaperTreeButton> buttons, List<PaperTreeNode> tree, List<SkillNode> unlockedNodes)
		{
			foreach (PaperTreeNode node in tree)
			{
				PaperTreeButton btn = buttons.Find(b => b.NodeID == node.NodeID);
				if (btn == null) continue;
				bool isUnlocked = unlockedNodes.Contains(node.NodeID);
				btn.SetUp(node, _icons[node.Stats], isUnlocked, SelectPaperTreeButton);
			}
		}

		private void SetUpSkillTreeLines(List<PaperTreeButton> buttons, List<PaperTreeNode> tree, List<SkillNode> unlockedNodes, int playerGold)
		{
			Transform parentPanel = buttons[0].transform.parent;
			parentPanel.DestroyChildren<PaperTreeLine>();

			foreach (PaperTreeNode currentNode in tree)
			{
				PaperTreeButton currentBtn = buttons.Find(b => b.NodeID == currentNode.NodeID);
				if (currentBtn == null) continue;
				Vector3 pointA = currentBtn.ExitPoint;
				bool currentUnlocked = unlockedNodes.Contains(currentNode.NodeID);

				foreach (PaperTreeNode nextNode in currentNode.NextNodes)
				{
					PaperTreeButton nextBtn = buttons.Find(b => b.NodeID == nextNode.NodeID);
					if (nextBtn == null) continue;
					Vector3 pointB = nextBtn.EntryPoint;
					PaperTreeLine line = Instantiate(_linePrefab, parentPanel).GetComponent<PaperTreeLine>();
					line.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					line.SetPosition(pointA, pointB);

					Color lineColor;
					bool nextUnlocked = unlockedNodes.Contains(nextNode.NodeID);
					if (nextUnlocked && currentUnlocked)
						lineColor = _lineColors[0];
					else if (nextNode.CanUnlock(unlockedNodes))
						lineColor = playerGold >= nextNode.Cost ? _lineColors[1] : _lineColors[2];
					else
						lineColor = _lineColors[3];

					line.SetStatus(lineColor);
				}
			}
		}

		private void SelectPaperTreeButton(PaperTreeNode selectedNode)
		{
			AppEvents.OnPaperTreeNodeSelected?.Invoke(selectedNode);
		}

		#endregion
	}
}