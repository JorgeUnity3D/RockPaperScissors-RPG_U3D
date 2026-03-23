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
		[SerializeField] private IconsScrObj _iconsScrObj;
		[SerializeField] private GameObject _linePrefab;
		[SerializeField] private List<Color> _lineColors;
		[SerializeField] private List<PaperTreeButton> _rockPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _paperPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _scissorsPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _defensePaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _energyRecPaperTreeButtons;
		[Header("DEBUG")]
		[SerializeField, ReadOnly] private List<PaperTreeNode> _rockSkillTree;
		[SerializeField, ReadOnly] private List<PaperTreeNode> _paperSkillTree;
		[SerializeField, ReadOnly] private List<PaperTreeNode> _scissorsSkillTree;
		[SerializeField, ReadOnly] private List<PaperTreeNode> _defenseSkillTree;
		[SerializeField, ReadOnly] private List<PaperTreeNode> _energyRecSkillTree;
		[SerializeField, ReadOnly] private IconsDictionary _icons;

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
			_icons = _iconsScrObj.Data;
		}

		/// <summary>Carga los datos del árbol, restaura el estado de los nodos y construye toda la UI de habilidades.</summary>
		public void SetData(List<StatAttribute> attributes, PaperTreeScrObj paperTreeScrObj, int playerGold)
		{
			Debug.Log($"[PaperTreeUIController] SetData() -> ");
			_rockSkillTree = paperTreeScrObj[Stats.ROCK];
			_paperSkillTree = paperTreeScrObj[Stats.PAPER];
			_scissorsSkillTree = paperTreeScrObj[Stats.SCISSOR];
			_defenseSkillTree = paperTreeScrObj[Stats.DEFENSE];
			_energyRecSkillTree = paperTreeScrObj[Stats.ENERGY_RECOVERY];
			RestoreNodeState(attributes, Stats.ROCK, _rockSkillTree);
			RestoreNodeState(attributes, Stats.PAPER, _paperSkillTree);
			RestoreNodeState(attributes, Stats.SCISSOR, _scissorsSkillTree);
			RestoreNodeState(attributes, Stats.DEFENSE, _defenseSkillTree);
			RestoreNodeState(attributes, Stats.ENERGY_RECOVERY, _energyRecSkillTree);
			SetUpPaperTreeUI(_rockPaperTreeButtons, _rockSkillTree, playerGold);
			SetUpPaperTreeUI(_paperPaperTreeButtons, _paperSkillTree, playerGold);
			SetUpPaperTreeUI(_scissorsPaperTreeButtons, _scissorsSkillTree, playerGold);
			SetUpPaperTreeUI(_defensePaperTreeButtons, _defenseSkillTree, playerGold);
			SetUpPaperTreeUI(_energyRecPaperTreeButtons, _energyRecSkillTree, playerGold);
		}

		private void RestoreNodeState(List<StatAttribute> attributes, Stats stat, List<PaperTreeNode> nodes)
		{
			StatAttribute attribute = attributes.Find(a => a.Stat == stat);
			if (attribute == null) return;
			PaperTreeModifier modifier = attribute.GetModifier<PaperTreeModifier>();
			if (modifier == null) return;
			foreach (PaperTreeNode node in nodes)
			{
				node.IsUnlocked = modifier.UnlockedNodes.Contains(node.NodeID);
			}
		}

		#endregion

		#region CONTROL

		private void SetUpPaperTreeUI(List<PaperTreeButton> paperTreeButtons, List<PaperTreeNode> paperTreeSkillTree, int playerGold)
		{
			SetUpSkillNodes(paperTreeButtons, paperTreeSkillTree);
			SetUpSkillTreeLines(paperTreeButtons, paperTreeSkillTree, playerGold);
		}

		private void SetUpSkillNodes(List<PaperTreeButton> paperTreeButtons, List<PaperTreeNode> paperTreeSkillTree)
		{
			Debug.Log($"[PaperTreeUIController] SetSkillTreeUI() -> ");
			foreach (PaperTreeNode paperTreeNode in paperTreeSkillTree)
			{
				PaperTreeButton paperTreeButton = paperTreeButtons.Find(ptn => ptn.NodeID == paperTreeNode.NodeID);
				Stats currentStat = paperTreeNode.Stats;
				Sprite icon = _icons[currentStat];
				paperTreeButton.SetUp(paperTreeNode, icon, SelectPaperTreeButton);
				paperTreeNode.SetUp(paperTreeSkillTree);
			}
		}

		private void SetUpSkillTreeLines(List<PaperTreeButton> paperTreeButtons, List<PaperTreeNode> paperTreeSkillTree, int playerGold)
		{
			Debug.Log($"[PaperTreeUIController] SetUpSkillTreeLines() -> ");

			Transform parentPanel = paperTreeButtons[0].transform.parent;
			parentPanel.DestroyChildren<PaperTreeLine>();

			foreach (PaperTreeNode currentNode in paperTreeSkillTree)
			{
				PaperTreeButton currentPaperTreeButton = paperTreeButtons.Find(ptb => ptb.NodeID == currentNode.NodeID);
				Vector3 pointA = currentPaperTreeButton.ExitPoint;
				foreach (PaperTreeNode nextNode in currentNode.NextNodes)
				{
					PaperTreeButton nextPaperTreeButton = paperTreeButtons.Find(ptb => ptb.NodeID == nextNode.NodeID);
					Vector3 pointB = nextPaperTreeButton.EntryPoint;
					PaperTreeLine paperTreeLine = Instantiate(_linePrefab, parentPanel).GetComponent<PaperTreeLine>();
					paperTreeLine.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					paperTreeLine.SetPosition(pointA, pointB);
					Color lineColor;
					if (nextNode.IsUnlocked && currentNode.IsUnlocked)
					{
						lineColor = _lineColors[0];
					}
					else if (nextNode.CanUnlock)
					{
						if (playerGold >= nextNode.Cost)
						{
							lineColor = _lineColors[1];
						}
						else
						{
							lineColor = _lineColors[2];
						}
					}
					else
					{
						lineColor = _lineColors[3];
					}
					paperTreeLine.SetStatus(lineColor);
				}
			}
		}

		private void SelectPaperTreeButton(PaperTreeNode selectedNode)
		{
			
		}

		#endregion
	}
}