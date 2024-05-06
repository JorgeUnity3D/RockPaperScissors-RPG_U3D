using System.Collections.Generic;
using System.Linq;
using Kapibara.RPS.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class PaperTreeUIController : BaseUIController
	{
		[SerializeField] private PaperTreeScrObj _paperTreeScrObj;
		[SerializeField] private IconsScrObj _iconsScrObj;
		[SerializeField] private GameObject _linePrefab;
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

		public void SetData(List<Attribute> attributes)
		{
			Debug.Log($"[PaperTreeUIController] SetData() -> ");
			SetUpPaperTreeUI(_rockPaperTreeButtons, _rockSkillTree);
			SetUpPaperTreeUI(_paperPaperTreeButtons, _paperSkillTree);
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
			
			foreach (PaperTreeNode paperTreeNode in paperTreeSkillTree)
			{
				PaperTreeButton currentPaperTreeButton = paperTreeButtons.Find(ptb => ptb.NodeID == paperTreeNode.NodeID);
				Vector3 pointA = currentPaperTreeButton.ExitPoint;
				foreach (PaperTreeNode nextNode in paperTreeNode.NextNodes)
				{
					PaperTreeButton nextPaperTreeButton = paperTreeButtons.Find(ptb => ptb.NodeID == nextNode.NodeID);
					Vector3 pointB = nextPaperTreeButton.EntryPoint;
					PaperTreeLine paperTreeLine = Instantiate(_linePrefab, parentPanel).GetComponent<PaperTreeLine>();
					paperTreeLine.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					paperTreeLine.SetPosition(pointA, pointB);
					paperTreeLine.SetStatus(nextNode.IsUnlocked ? Color.green : Color.red);
				}
			}
		}

		private void SelectPaperTreeButton()
		{
			throw new System.NotImplementedException();
		}
		
		#endregion
	}
}