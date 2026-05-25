using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Nodo del árbol de habilidades Paper Tree. Almacena la estadística que mejora, su coste y las conexiones con nodos adyacentes.
	/// </summary>
	[Serializable]
	public class PaperTreeNode
	{
		[SerializeField] private SkillNode _nodeID;
		[SerializeField] private Stats _stat;
		[SerializeField] private int _modifier;
		[SerializeField] private int _cost;
		[NonSerialized] private List<PaperTreeNode> _nextNodes;
		[NonSerialized] private List<PaperTreeNode> _previousNodes;
		[SerializeField] private List<SkillNode> _nextNodesIDs;

		/// <summary>Identificador único del nodo en el árbol.</summary>
		public SkillNode NodeID
		{
			get => _nodeID;
		}

		/// <summary>Estadística que incrementa este nodo al desbloquearse.</summary>
		public Stats Stats
		{
			get => _stat;
			set => _stat = value;
		}

		/// <summary>Cantidad de bonificación que aporta el nodo al atributo.</summary>
		public int Modifier
		{
			get => _modifier;
			set => _modifier = value;
		}

		/// <summary>True si todos los nodos previos están desbloqueados y este puede comprarse.</summary>
		public bool CanUnlock(List<SkillNode> unlockedNodes)
		{
			return _previousNodes.TrueForAll(pn => unlockedNodes.Contains(pn.NodeID));
		}

		/// <summary>Coste en oro para desbloquear este nodo.</summary>
		public int Cost
		{
			get => _cost;
		}

		/// <summary>Nodos que se habilitan tras desbloquear este (hijos en el árbol).</summary>
		public List<PaperTreeNode> NextNodes
		{
			get => _nextNodes;
		}

		/// <summary>Nodos que deben estar desbloqueados antes que este (padres en el árbol).</summary>
		public List<PaperTreeNode> PreviousNodes
		{
			get => _previousNodes;
		}

		/// <summary>IDs de los nodos hijos; se resuelven a referencias en SetUp().</summary>
		public List<SkillNode> NextNodesIDs
		{
			get => _nextNodesIDs;
		}

		public PaperTreeNode()
		{
			_modifier = 1;
			_cost = 10;
			_nextNodesIDs = new List<SkillNode>();
			_nextNodes = new List<PaperTreeNode>();
			_previousNodes = new List<PaperTreeNode>();
		}

		/// <summary>Limpia los links de runtime. Llamado por PaperTreeScrObj.OnEnable() antes de reconstruir.</summary>
		public void ClearLinks()
		{
			_nextNodes.Clear();
			_previousNodes.Clear();
		}

		/// <summary>Enlaza referencias de nodos siguientes y registra este nodo como padre de sus hijos.</summary>
		public void SetUp(List<PaperTreeNode> allNodes)
		{
			foreach (SkillNode nextNodeID in _nextNodesIDs)
			{
				PaperTreeNode nextNode = allNodes.Find(ptn => ptn.NodeID == nextNodeID);
				if (nextNode == null) continue;
				nextNode.PreviousNodes.Add(this);
				_nextNodes.Add(nextNode);
			}
		}
	}
}