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
		[SerializeField] private bool _isUnlocked;
		[SerializeField] private int _cost;
		[NonSerialized] private List<PaperTreeNode> _nextNodes;
		[NonSerialized] private List<PaperTreeNode> _previousNodes;
		[NonSerialized] private List<SkillNode> _previousNodesIDs;
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

		/// <summary>True si el jugador ya compró este nodo.</summary>
		public bool IsUnlocked
		{
			get => _isUnlocked;
			set => _isUnlocked = value;
		}

		/// <summary>True si todos los nodos previos están desbloqueados y este puede comprarse.</summary>
		public bool CanUnlock
		{
			get
			{
				return _previousNodes.TrueForAll(ptn => ptn.IsUnlocked);
			}
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

		/// <summary>IDs de los nodos padres; se rellenan durante SetUp() por los propios hijos.</summary>
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

		/// <summary>Enlaza las referencias de nodos siguientes y registra este nodo como padre de sus hijos.</summary>
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