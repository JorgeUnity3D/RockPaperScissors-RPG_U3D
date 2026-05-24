using Kapibara.Util.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.UI
{
	public class UITabsController : MonoBehaviour
	{
		[SerializeField] private List<Tab> _tabs;
		private TabContent _currentTab;

		#region UNITY_LIFECYCLE

		private void Awake()
		{
			SetUp();
		}

		private void Start()
		{
			HideAllTabs();
			ShowTab(_tabs[0].TabContent);
		}

		#endregion

		#region SETUP

		private void SetUp()
		{
			foreach (Tab tab in _tabs)
			{
				if (tab.TabButton == null || tab.TabContent == null) { Debug.LogError($"[UITabsController] Tab on {gameObject.name} has missing references — check Inspector."); continue; }
				tab.TabButton.AddListener(() => ShowTab(tab.TabContent), true);
			}
		}

		#endregion

		#region CONTROL

		private void ShowTab(TabContent tabContent)
		{
			if (_currentTab != null)
			{
				_currentTab.HideCanvas();
			}
			tabContent.ShowCanvas();
			_currentTab = tabContent;
		}

		private void HideAllTabs()
		{
			foreach (Tab tab in _tabs)
			{
				if (tab.TabContent == null) continue;
				tab.TabContent.HideCanvas();
			}
		}

		#endregion

	}
}
