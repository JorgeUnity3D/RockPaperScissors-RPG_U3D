using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Kapibara.RPS
{
	[Serializable]
	public class PaperTreeSkillTree
	{
		[SerializeField] private List<PaperTreeNode> _nodes;

		public List<PaperTreeNode> Nodes
		{
			get => _nodes;
		}
		
		public PaperTreeSkillTree()
		{
			_nodes = new List<PaperTreeNode>();
		}
	}
}