using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS {
    public class CombatUIManagerOld : BaseManager_old {
        [Header("Combat Canvas")] public GameObject combatCanvas;
        [Header("Player")] public GameObject playerHolder;
        public GameObject playerActionHolder;
        public Image playerActionImage;
        public TextMeshProUGUI playerDamageText;
        public GameObject playerThoughtHolder;
        public Image playerThoughtImage;
        public Scrollbar playerHealthBar;
        public Scrollbar playerEnergyBar;
        [Header("Enemy")] public GameObject enemyHolder;
        public Image enemyImage;
        public GameObject enemyActionHolder;
        public Image enemyActionImage;
        public TextMeshProUGUI enemyDamageText;
        public GameObject enemyThoughtHolder;
        public Image enemyThoughtImage;
        public Scrollbar enemyHealthBar;
        public Scrollbar enemyEnergyBar;
        [Header("Actions")] public Button stoneButton;
        public Button paperButton;
        public Button scissorButton;
        public Button defenseButton;
        public Button energyButton;

        [Header("Sprites by Languages")] //0 - None
        public List<Language> iconsByLanguage;

        #region GAME STATES

        public override void OnInitializeGameEvent() {
            combatCanvas.SetActive(false);
        }

        public override void OnLevelSelectedEvent(Level level) {
            //Set UI
            combatCanvas.SetActive(true);
            AssignActionButtons();
            GameManager.CombatPreparationEvent();
        }

        #endregion

        #region COMBAT PHASES

        public override void OnCombatPreparationEvent() {
            //Set Player
            playerHolder.SetActive(true);
            playerActionHolder.SetActive(false);
            playerThoughtHolder.SetActive(false);
            //Set Enemy
            enemyHolder.SetActive(true);
            enemyActionHolder.SetActive(false);
            enemyThoughtHolder.SetActive(false);
        }

        #endregion

        #region ACTION BUTTONS

        public void AssignActionButtons() {
            stoneButton.onClick.AddListener(() => SelectAction(GameManager.Player, Actions.ROCK));
            paperButton.onClick.AddListener(() => SelectAction(GameManager.Player, Actions.PAPER));
            scissorButton.onClick.AddListener(() => SelectAction(GameManager.Player, Actions.SCISSOR));
            defenseButton.onClick.AddListener(() => SelectAction(GameManager.Player, Actions.DEFENSE));
            energyButton.onClick.AddListener(() => SelectAction(GameManager.Player, Actions.ENERGY));
        }

        public void SelectAction(Character targetCharacter, Actions action) {
            targetCharacter.currentAction = action;
            EnableActionButtons(false);
        }

        public void EnableActionButtons(bool isEnabled) {
            stoneButton.interactable = isEnabled;
            paperButton.interactable = isEnabled;
            scissorButton.interactable = isEnabled;
            defenseButton.interactable = isEnabled;
            energyButton.interactable = isEnabled;
        }

        #endregion

        #region CHARACTERS UI

        public void SetActionIcon(Character character) {
            Image targetImage = character.GetType() == typeof(Player) ? playerActionImage : enemyActionImage;
            targetImage.sprite = GetActionSprite(character);
            ShowActionIcon(character, true);
        }

        public void SetThoughtIcon(Character character, bool isUnknown = false) {
            Image targetImage = character.GetType() == typeof(Player) ? playerThoughtImage : enemyThoughtImage;
            targetImage.sprite = GetActionSprite(character, isUnknown);
            ShowThoughtIcon(character, true);
        }

        public void ShowActionIcon(Character character, bool isShowing) {
            GameObject targetIcon = character.GetType() == typeof(Player) ? playerActionHolder : enemyActionHolder;
            targetIcon.SetActive(isShowing);
        }

        public void ShowThoughtIcon(Character character, bool isShowing) {
            GameObject targetIcon = character.GetType() == typeof(Player) ? playerThoughtHolder : enemyThoughtHolder;
            targetIcon.SetActive(isShowing);
        }

        public void SetDamageText(Character character) {
            TextMeshProUGUI targetText = character.GetType() == typeof(Player) ? playerDamageText : enemyDamageText;
            targetText.text = character.currentTotalDamage.ToString();
        }

        public void ScaleActionHolder(int combatResult) {
            playerActionHolder.transform.localScale = Vector3.one;
            enemyActionHolder.transform.localScale = Vector3.one;
            Vector3 playerScale = playerActionHolder.transform.localScale;
            Vector3 enemyScale = enemyActionHolder.transform.localScale;
            if (combatResult > 0) {
                playerScale *= 1.2f;
                enemyScale *= 0.8f;
            } else if (combatResult < 0) {
                playerScale *= 0.8f;
                enemyScale *= 1.2f;
            }

            playerActionHolder.transform.localScale = playerScale;
            enemyActionHolder.transform.localScale = enemyScale;
        }

        public void SetEnemyImage(Enemy enemy) {
            // enemyImage.sprite = enemy.EnemySprite;
        }

        public void SetHealthBar(Character character) {
            Scrollbar targetHealthBar = character.GetType() == typeof(Player) ? playerHealthBar : enemyHealthBar;
            targetHealthBar.size = character.currentHealth / character.maxHealth;
        }

        public void SetEnergyBar(Character character) {
            Scrollbar targetEnergyhBar = character.GetType() == typeof(Player) ? playerEnergyBar : enemyEnergyBar;
            targetEnergyhBar.size = character.currentEnergy / character.maxEnergy;
        }

        private Sprite GetActionSprite(Character character, bool isUnknown = false) {
            Sprite targetSprite = null;
            Language targetLanguage = iconsByLanguage[0];
            if (isUnknown) {
                // targetSprite = targetLanguage.UnknownSprite;
            } else {
                if (character.GetType() == typeof(Enemy)) {
                    // targetLanguage = iconsByLanguage.Find(l => l.language == ((Enemy)character).currentLanguage);
                }

                switch (character.currentAction) {
                    //case Actions.NONE:
                    //    targetSprite = targetLanguage.UnknownSprite;
                    //    break;
                    case Actions.ROCK:
                        //targetSprite = targetLanguage.RockSprite;
                        break;
                    case Actions.PAPER:
                        //targetSprite = targetLanguage.PaperSprite;
                        break;
                    case Actions.SCISSOR:
                        //targetSprite = targetLanguage.ScissorSprite;
                        break;
                    case Actions.DEFENSE:
                        //targetSprite = targetLanguage.DefenseSprite;
                        break;
                    case Actions.ENERGY:
                        //targetSprite = targetLanguage.EnergySprite;
                        break;
                }
            }

            return targetSprite;
        }

        #endregion
    }
}