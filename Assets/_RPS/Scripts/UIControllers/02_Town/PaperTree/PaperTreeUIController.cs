using System.Collections.Generic;
using Kapibara.Util.Extensions;
using Kapibara.UI;
using UnityEngine;

namespace Kapibara.RPS
{
	public class PaperTreeUIController : BaseUIController
	{
		[Header("DATA")]
		[SerializeField] private PaperTreeScrObj _paperTreeScrObj;
		[Header("UI")]
		[SerializeField] private IconsScrObj _iconsScrObj;
		[SerializeField] private GameObject _linePrefab;
		[SerializeField] private List<Color> _lineColors;
		[SerializeField] private List<PaperTreeButton> _rockPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _paperPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _scissorsPaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _defensePaperTreeButtons;
		[SerializeField] private List<PaperTreeButton> _energyRecPaperTreeButtons;
		private List<PaperTreeNode> _rockSkillTree;
		private List<PaperTreeNode> _paperSkillTree;
		private List<PaperTreeNode> _scissorsSkillTree;
		private List<PaperTreeNode> _defenseSkillTree;
		private List<PaperTreeNode> _energyRecSkillTree;
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
			_rockSkillTree = _paperTreeScrObj[Stats.ROCK];
			_paperSkillTree = _paperTreeScrObj[Stats.PAPER];
			_scissorsSkillTree = _paperTreeScrObj[Stats.SCISSOR];
			_defenseSkillTree = _paperTreeScrObj[Stats.DEFENSE];
			_energyRecSkillTree = _paperTreeScrObj[Stats.ENERGY_RECOVERY];
			_icons = _iconsScrObj.Data;
		}		

		public void SetData(List<StatAttribute> attributes)
		{
			Debug.Log($"[PaperTreeUIController] SetData() -> ");
			SetUpPaperTreeUI(_rockPaperTreeButtons, _rockSkillTree);
			SetUpPaperTreeUI(_paperPaperTreeButtons, _paperSkillTree);
			SetUpPaperTreeUI(_scissorsPaperTreeButtons, _scissorsSkillTree);
			SetUpPaperTreeUI(_defensePaperTreeButtons, _defenseSkillTree);
			SetUpPaperTreeUI(_energyRecPaperTreeButtons, _energyRecSkillTree);
		}

		#endregion
		
		#region CONTROL

		private void SetUpPaperTreeUI(List<PaperTreeButton> paperTreeButtons, List<PaperTreeNode> paperTreeSkillTree)
		{
			SetUpSkillNodes(paperTreeButtons, paperTreeSkillTree);
			SetUpSkillTreeLines(paperTreeButtons, paperTreeSkillTree);
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

		private void SetUpSkillTreeLines(List<PaperTreeButton> paperTreeButtons, List<PaperTreeNode> paperTreeSkillTree)
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
						if (AppContext.Player.Gold >= nextNode.Cost)
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