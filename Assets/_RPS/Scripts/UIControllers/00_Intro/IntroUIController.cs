using System;
using DG.Tweening;
using Kapibara.UI;
using UnityEngine;

namespace Kapibara.RPS
{
    public class IntroUIController : UIController 
	{

		[Header("UI")]
		[SerializeField] private RectTransform _introHolder;
        
        #region UNITY_LIFECYCLE

        void Awake()
        {
	        SetUp();
        }
        
        #endregion
        
        #region SETUP

	    public override void SetUp()
	    {
		    _introHolder.localScale = Vector3.zero;
	    }

        #endregion
        
        #region CONTROL

        public void FadeInIntro()
        {
            Debug.Log($"[IntroUIController] FadeInIntro() ");
            _introHolder.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
                Debug.Log($"[IntroUIController] FadeInIntro() -> OnComplete -> AppEvents.OnIntroCompleted");
                AppEvents.OnIntroCompleted?.Invoke();
            });
        }

        #endregion
    }
}