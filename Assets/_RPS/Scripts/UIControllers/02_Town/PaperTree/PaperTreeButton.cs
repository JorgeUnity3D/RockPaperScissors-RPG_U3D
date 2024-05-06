using System;
using Kapibara.RPS.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Kapibara.RPS
{
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
		public SkillNode NodeID
		{
			get => _nodeID;
		}

		public Vector3 EntryPoint
		{
			get
			{				
				return transform.parent.InverseTransformPoint(_entryPoint.position);
			}
		}

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

		public void SetUp(PaperTreeNode paperTreeNode, Sprite statIcon, UnityAction OnClickAction)
		{
			_paperTreeNode = paperTreeNode;
			_unlockedIcon = statIcon;
			_iconImage.sprite = _paperTreeNode.IsUnlocked ? _unlockedIcon : _lockedIcon;
			_button.AddListener(OnClickAction);
		}	

		#endregion

		#region CONTROL

		#endregion
	}
}