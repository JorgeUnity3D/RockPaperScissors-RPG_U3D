using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	public class PaperTreeLine : MonoBehaviour
	{
		[SerializeField, ReadOnly] private UILineRenderer _backgroundLineRenderer;
		[SerializeField, ReadOnly] private UILineRenderer _frontLineRenderer;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			_backgroundLineRenderer = GetComponent<UILineRenderer>();
			_frontLineRenderer = transform.GetChild(0).GetComponent<UILineRenderer>();
		}

		#endregion
		
		#region CONTROL

		public void SetStatus(Color statusColor)
		{
			_backgroundLineRenderer.color = Color.black;
			_frontLineRenderer.color = statusColor;	
		}

		public void SetPosition(Vector2 entryPosition, Vector2 exitPosition)
		{
			_backgroundLineRenderer.points = new Vector2[]
			{
				entryPosition,
				exitPosition
			};
			_frontLineRenderer.points = new Vector2[]
			{
				entryPosition,
				exitPosition
			};
		}

		#endregion
	}
}