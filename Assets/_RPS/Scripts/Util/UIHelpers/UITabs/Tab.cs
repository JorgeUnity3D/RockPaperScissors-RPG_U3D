using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS.UI
{
	[Serializable]
	public class Tab
	{
		[SerializeField] private Button _tabButton;
		[SerializeField] private TabContent _tabContent;

		public Button TabButton
		{
			get => _tabButton;
		}

		public TabContent TabContent
		{
			get => _tabContent;
		}
	}
}
