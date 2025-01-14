using Doozy.Runtime.UIManager.Components;
using Kapibara.RPS.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Kapibara.RPS
{
    public class GameContextButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _gameIDText;
        private UIButton _selectButton;
        private GameContext _gameContext;

        #region UNTY_LIFECYCLE

        void Awake()
        {
            _selectButton = GetComponent<UIButton>();
        }

        #endregion

        #region CONTROL
        
        public void SetData(GameContext gameContext, UnityAction<GameContext, UIButton> OnSelectedCallback)
        {
            _gameContext = gameContext;
            _gameIDText.text = _gameContext.gameName;
            _selectButton.AddListener(() =>
            {
                OnSelectedCallback?.Invoke(_gameContext, _selectButton);
            });
        }
        
        #endregion
    }
}