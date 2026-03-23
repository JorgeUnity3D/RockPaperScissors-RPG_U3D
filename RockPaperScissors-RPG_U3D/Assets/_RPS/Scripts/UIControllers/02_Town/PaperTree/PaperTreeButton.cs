using Kapibara.Util.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Botón de nodo del árbol Paper Tree. Muestra icono bloqueado/desbloqueado y expone puntos de entrada/salida para las líneas de conexión.
	/// </summary>
	public class PaperTreeButton : MonoBehaviour
	{
		[SerializeField] private SkillNode _nodeID;
		[SerializeField] private Image _iconImage;
		[SerializeField] private Transform _entryPoint;
		[SerializeField] private Transform _exitPoint;
		[SerializeField, ReadOnly] private Button _button;
		[SerializeField, ReadOnly] private PaperTreeNode _paperTreeNode;
		[SerializeField, ReadOnly] private Sprite _lockedIcon;
		[SerializeField, ReadOnly] private Sprite _unlockedIcon;
		/// <summary>Identificador del nodo que representa este botón en el árbol.</summary>
		public SkillNode NodeID
		{
			get => _nodeID;
		}

		/// <summary>Punto de entrada en coordenadas locales del panel padre; usado para trazar la línea desde el nodo anterior.</summary>
		public Vector3 EntryPoint
		{
			get
			{				
				return transform.parent.InverseTransformPoint(_entryPoint.position);
			}
		}

		/// <summary>Punto de salida en coordenadas locales del panel padre; usado para trazar la línea hacia el nodo siguiente.</summary>
		public Vector3 ExitPoint
		{
			get
			{				
				return transform.parent.InverseTransformPoint(_exitPoint.position);
			}
		}

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			_button = GetComponent<Button>();
			_lockedIcon = _iconImage.sprite;
		}

		#endregion

		#region SETUP

		/// <summary>Asigna el nodo, el icono de la estadística y el callback al pulsar el botón.</summary>
		public void SetUp(PaperTreeNode paperTreeNode, Sprite statIcon, UnityAction<PaperTreeNode> OnClickAction)
		{
			_paperTreeNode = paperTreeNode;
			_unlockedIcon = statIcon;
			_iconImage.sprite = _paperTreeNode.IsUnlocked ? _unlockedIcon : _lockedIcon;
			_button.AddListener(() => OnClickAction?.Invoke(_paperTreeNode));
		}	

		#endregion

		#region CONTROL

		#endregion
	}
}