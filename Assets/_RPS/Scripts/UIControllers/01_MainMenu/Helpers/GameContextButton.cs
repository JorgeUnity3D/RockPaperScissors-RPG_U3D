using Kapibara.RPS.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kapibara.RPS
{
    public class GameContextButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _gameIDText;
        private Button _selectButton;
        private GameContext _gameContext;

        #region UNTY_LIFECYCLE

        void Awake()
        {
            _selectButton = GetComponent<Button>();
        }

        #endregion

        #region CONTROL
        
        public void SetData(GameContext gameContext, UnityAction<GameContext, Button> OnSelectedCallback)
        {
            _gameContext = gameContext;
            _gameIDText.text = _gameContext.GameName;
            _selectButton.AddListener(() =>
            {
                OnSelectedCallback?.Invoke(_gameContext, _selectButton);
            });
        }
        
        #endregion
    }
}