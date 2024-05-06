using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[Serializable]
	public class PaperTreeNode
	{
		[SerializeField] private SkillNode _nodeID;
		[SerializeField] private Stats _stat;
		[SerializeField] private int _modifier;
		[SerializeField] private bool _isUnlocked;
		[SerializeField] private int _cost;
		private List<PaperTreeNode> _nextNodes;
		private List<PaperTreeNode> _previousNodes;
		private List<SkillNode> _previousNodesIDs;
		[SerializeField] private List<SkillNode> _nextNodesIDs;

		public SkillNode NodeID
		{
			get => _nodeID;
		}

		public Stats Stats
		{
			get => _stat;
			set => _stat = value;
		}

		public int Modifier
		{
			get => _modifier;
			set => _modifier = value;
		}

		public bool IsUnlocked
		{
			get => _isUnlocked;
			set => _isUnlocked = value;
		}

		public int Cost
		{
			get => _cost;			
		}

		public List<PaperTreeNode> NextNodes
		{
			get => _nextNodes;			
		}

		public List<PaperTreeNode> PreviousNodes
		{
			get => _previousNodes;			
		}

		public List<SkillNode> NextNodesIDs
		{
			get => _nextNodesIDs;			
		}

		public List<SkillNode> PreviousNodesIDs
		{
			get => _previousNodesIDs;			
		}

		public PaperTreeNode()
		{
			_isUnlocked = false;
			_modifier = 1;
			_cost = 10;
			_nextNodesIDs = new List<SkillNode>();
			_nextNodes = new List<PaperTreeNode>();
			_previousNodes = new List<PaperTreeNode>();
			_previousNodesIDs = new List<SkillNode>();
		}

		public void SetUp(List<PaperTreeNode> allNodes)
		{
			foreach (SkillNode nextNodeID in _nextNodesIDs)
			{
				if (allNodes.Exists(ptn => ptn.NodeID == nextNodeID))
				{
					PaperTreeNode nextNode = allNodes.Find(ptn => ptn.NodeID == nextNodeID);
					nextNode.PreviousNodes.Add(this);
					_nextNodes.Add(nextNode);
				}
			}
		}
	}
}