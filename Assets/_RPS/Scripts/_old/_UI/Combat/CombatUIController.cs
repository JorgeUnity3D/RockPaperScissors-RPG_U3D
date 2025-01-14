using System.Collections;
using Animancer;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kapibara.RPS {
    public class CombatUIController : UIController<CombatManagerRefactored> {
        [FoldoutGroup("UI")] [FoldoutGroup("UI/Player")] [ValidateInput("@playerHolder != null")]
        public GameObject playerHolder;

        [FoldoutGroup("UI/Player")] [ValidateInput("@playerImage != null")]
        public Image playerImage;

        [FoldoutGroup("UI/Player")] [ValidateInput("@playerDamageText != null")]
        public TextMeshProUGUI playerDamageText;

        [FoldoutGroup("UI/Player")] [ValidateInput("@playerActionImage != null")]
        public Image playerActionImage;

        [FoldoutGroup("UI/Player")] [ValidateInput("@playerThoughtImage != null")]
        public Image playerThoughtImage;

        [FoldoutGroup("UI/Player")] [ValidateInput("@playerHealthBar != null")]
        public Scrollbar playerHealthBar;

        [FoldoutGroup("UI/Player")] [ValidateInput("@playerEnergyBar != null")]
        public Scrollbar playerEnergyBar;

        [FoldoutGroup("UI/Player")] [ValidateInput("@playerPrevisionEnergyBar != null")]
        public Scrollbar playerPrevisionEnergyBar;

        [FoldoutGroup("UI/Enemy")] [ValidateInput("@enemyHolder != null")]
        public GameObject enemyHolder;

        [FoldoutGroup("UI/Enemy")] [ValidateInput("@enemyImage != null")]
        public Image enemyImage;

        [FoldoutGroup("UI/Enemy")] [ValidateInput("@enemyDamageText != null")]
        public TextMeshProUGUI enemyDamageText;

        [FoldoutGroup("UI/Enemy")] [ValidateInput("@enemyActionImage != null")]
        public Image enemyActionImage;

        [FoldoutGroup("UI/Enemy")] [ValidateInput("@enemyThoughtImage != null")]
        public Image enemyThoughtImage;

        [FoldoutGroup("UI/Enemy")] [ValidateInput("@enemyHealthBar != null")]
        public Scrollbar enemyHealthBar;

        [FoldoutGroup("UI/Enemy")] [ValidateInput("@enemyEnergyBar != null")]
        public Scrollbar enemyEnergyBar;

        [FoldoutGroup("UI/Enemy")] [ValidateInput("@enemyPrevisionEnergyBar != null")]
        public Scrollbar enemyPrevisionEnergyBar;

        [FoldoutGroup("UI/Animations")] [ValidateInput("@combatEnterAnim != null")]
        public AnimationClip combatEnterAnim;

        [FoldoutGroup("UI/Animations")] [ValidateInput("@combatDamageAnim != null")]
        public AnimationClip combatDamageAnim;

        [FoldoutGroup("UI/Animations")] [ValidateInput("@combatPlayerDieAnim != null")]
        public AnimationClip combatPlayerDieAnim;

        [FoldoutGroup("UI/Animations")] [ValidateInput("@combatEnemyDieAnim != null")]
        public AnimationClip combatEnemyDieAnim;

        [FoldoutGroup("DEBUG")] [ValidateInput("@nextStepButton != null")]
        public UIButton nextStepButton;


        private AnimancerComponent PlayerAnimancer => playerHolder.GetComponent<AnimancerComponent>();
        private GameObject PlayerActionHolder => playerActionImage.transform.parent.gameObject;
        private GameObject PlayerThoughtHolder => playerThoughtImage.transform.parent.gameObject;
        private AnimancerComponent EnemyAnimancer => enemyHolder.GetComponent<AnimancerComponent>();
        private GameObject EnemyActionHolder => enemyActionImage.transform.parent.gameObject;
        private GameObject EnemyThoughtHolder => enemyThoughtImage.transform.parent.gameObject;

        // protected void Start() {
        //     GetButtons<UIButtonId.CombatActionButtons>();
        // }
        //
        // protected override void LinkManager() {
        //     Manager.OnCombatSetUp += InitializeCombatUI;
        //     Manager.OnEnemyUpdated += UpdateEnemyUI;
        //
        //     //Phase 0
        //     Manager.OnCombatPhase0 += Phase0UIState;
        //
        //     //Phase 1
        //     Manager.OnCombatPhase1Begins += Phase1UIBeginState;
        //     Manager.OnCombatPhase1Ends += Phase1UIEndState;
        //     //Phase 2
        //     Manager.OnCombatPhase2Begins += Phase2UIBeginState;
        //     Manager.OnCombatPhase2PlayerSelects += Phase2PlayerSelectsState;
        //     Manager.OnCombatPhase2Ends += Phase2UIEndState;
        //     //Phase 3
        //     Manager.OnCombatPhase3Begins += Phase3UIBeginState;
        //     Manager.OnCombatPhase3Ends += Phase3UIEndState;
        //     //Phase 4
        //     Manager.OnCombatPhase4Begins += Phase4UIBeginState;
        //     Manager.OnCombatPhase4DamageCalculated += Phase4UIDamageCalculated;
        //     Manager.OnCombatPhase4Ends += Phase4UIEndState;
        //     //Phase 5
        //     Manager.OnCombatPhase5Begins += Phase5UIBeginState;
        //     Manager.OnCombatPhase5Ends += Phase5UIEndState;
        // }
        //
        // protected override void FilterStream(StreamId.CombatStream stream) {
        //     Debug.Log("[CombatUIController] FilterStream() -> stream: " + stream.ToString());
        //     switch (stream) {
        //         case StreamId.CombatStream.RockAction:
        //             Manager.SetPlayerAction(Actions.ROCK);
        //             break;
        //         case StreamId.CombatStream.PaperAction:
        //             Manager.SetPlayerAction(Actions.PAPER);
        //             break;
        //         case StreamId.CombatStream.ScissorsAction:
        //             Manager.SetPlayerAction(Actions.SCISSOR);
        //             break;
        //         case StreamId.CombatStream.DefenseAction:
        //             Manager.SetPlayerAction(Actions.DEFENSE);
        //             break;
        //         case StreamId.CombatStream.EnergyAction:
        //             Manager.SetPlayerAction(Actions.ENERGY);
        //             break;
        //         case StreamId.CombatStream.Start:
        //             Debug.Log("[CombatUIController] FilterStream() -> Filtered StreamId.CombatStream.Start");
        //             nextStepButton.gameObject.SetActive(false);
        //             Manager.StartCombat(false);
        //             break;
        //         case StreamId.CombatStream.StartWithSteps:
        //             Debug.Log("[CombatUIController] FilterStream() -> Filtered StreamId.CombatStream.StartWithSteps");
        //             nextStepButton.gameObject.SetActive(true);
        //             Manager.StartCombat(true);
        //             break;
        //         case StreamId.CombatStream.Pause:
        //             Debug.Log("[CombatUIController] FilterStream() -> Filtered StreamId.CombatStream.Pause");
        //             Manager.PauseCombat();
        //             break;
        //         case StreamId.CombatStream.Leave:
        //             Debug.Log("[CombatUIController] FilterStream() -> Filtered StreamId.CombatStream.Leave");
        //             Manager.StopCombat();
        //             break;
        //         case StreamId.CombatStream.NextStepSignal:
        //             Debug.Log("[CombatUIController] FilterStream() -> Filtered StreamId.CombatStream.NextStepSignal");
        //             Manager.NextStep();
        //             break;
        //     }
        // }
        //
        //
        // #region PHASE STATES
        //
        // private void Phase0UIState() {
        //     Debug.Log("[CombatUIController] Phase0UIState() ->");
        //     DisableActionButtons();
        //     PlayerAnimancer.Play(combatEnterAnim);
        //     EnemyAnimancer.Play(combatEnterAnim);
        //     nextStepButton.interactable = true;
        // }
        //
        // private void Phase1UIBeginState() {
        //     Debug.Log("[CombatUIController] Phase1UIBeginState() ->");
        //     DisableActionButtons();
        //     nextStepButton.interactable = false;
        // }
        //
        // private void Phase1UIEndState(Enemy enemy, bool isKnownIcon) {
        //     Debug.Log("[CombatUIController] Phase1UIEndState() ->");
        //     SetThoughtIcon(enemy, isKnownIcon);
        //     nextStepButton.interactable = true;
        // }
        //
        // private void Phase2UIBeginState(Player player) {
        //     Debug.Log("[CombatUIController] Phase2UIBeginState() ->");
        //     EnableActionButtons();
        //     SetActionButtonsStatus(player);
        //     nextStepButton.interactable = false;
        // }
        //
        // private void Phase2PlayerSelectsState(Player player) {
        //     Debug.Log("CombatUIController] Phase2PlayerSelectsState() ->");
        //     //SetEnergyBar(player, true);
        //     if (player.thinkingAction != Actions.NONE) {
        //         SetThoughtIcon(player, true);
        //         SetEnergyBar(player, true);
        //     } else {
        //         if (player.currentAction != Actions.NONE) {
        //             //TODO: not enough energy
        //             Debug.Log("[CombatUIController] Phase2PlayerSelectsState() -> Player doesn't have enough energy");
        //         }
        //     }
        // }
        //
        // private void Phase2UIEndState(Character character) {
        //     Debug.Log("[CombatUIController] Phase2UIEndState() ->");
        //     DisableActionButtons();
        //     nextStepButton.interactable = true;
        // }
        //
        // private void Phase3UIBeginState() {
        //     Debug.Log("[CombatUIController] Phase3UIBeginState() ->");
        //     nextStepButton.interactable = false;
        //     //TODO: whatever
        // }
        //
        // private void Phase3UIEndState(Player player, Enemy enemy) {
        //     Debug.Log("[CombatUIController] Phase3UIEndState() ->");
        //     ShowThoughtIcon(player, false);
        //     ShowThoughtIcon(enemy, false);
        //     SetActionIcon(player);
        //     SetActionIcon(enemy);
        //     nextStepButton.interactable = true;
        // }
        //
        // private void Phase4UIBeginState(Player player, Enemy enemy) {
        //     Debug.Log("[CombatUIController] Phase4UIBeginState() ->");
        //     SetEnergyBar(player);
        //     SetEnergyBar(enemy);
        //     nextStepButton.interactable = false;
        // }
        //
        // private void Phase4UIDamageCalculated(Player player, Enemy enemy, int combatResult) {
        //     Debug.Log("[CombatUIController] Phase4UIDamageCalculated() ->");
        //     SetDamageIcon(player, true);
        //     SetDamageIcon(enemy, true);
        //     SetActionHoldersSize(combatResult);
        //     if (combatResult > 0) {
        //         EnemyAnimancer.Play(combatDamageAnim);
        //     }
        //
        //     if (combatResult < 0) {
        //         PlayerAnimancer.Play(combatDamageAnim);
        //     }
        // }
        //
        // private void Phase4UIEndState(Player player, Enemy enemy) {
        //     Debug.Log("[CombatUIController] Phase4UIEndState() ->");
        //     SetHealthBar(player);
        //     SetHealthBar(enemy);
        //     nextStepButton.interactable = true;
        // }
        //
        // private void Phase5UIBeginState(Player player, Enemy enemy) {
        //     Debug.Log("[CombatUIController] Phase5UIBeginState() ->");
        //     ShowActionIcon(player, false);
        //     ShowActionIcon(enemy, false);
        //     
        //     nextStepButton.interactable = false;
        // }
        //
        // private void Phase5UIEndState(Player player, Enemy enemy) {
        //     Debug.Log("[CombatUIController] Phase5UIEndState() ->");
        //     SetDamageIcon(player, false);
        //     SetDamageIcon(enemy, false);
        //     SetActionHoldersSize();
        //     nextStepButton.interactable = true;
        // }
        //
        // #endregion
        //
        // #region UI FUNCTIONALITY
        //
        // private void InitializeCombatUI(Player player /*, Enemy enemy*/) {
        //     //Player
        //     UpdatePlayerUI(player);
        //     //Enemy
        //     //UpdateEnemyUI(enemy);
        // }
        //
        // private void UpdatePlayerUI(Player player) {
        //     Debug.Log("[CombatUIController] UpdatePlayerUI() ->");
        //     SetHealthBar(player);
        //     SetEnergyBar(player);
        //     ShowActionIcon(player, false);
        //     ShowThoughtIcon(player, false);
        //     SetDamageIcon(player, false);
        // }
        //
        // private void UpdateEnemyUI(Enemy enemy) {
        //     Debug.Log("[CombatUIController] UpdateEnemyUI() ->");
        //     enemyImage.sprite = enemy.portrait;
        //     SetHealthBar(enemy);
        //     SetEnergyBar(enemy);
        //     ShowActionIcon(enemy, false);
        //     ShowThoughtIcon(enemy, false);
        //     SetDamageIcon(enemy, false);
        // }
        //
        // private void EnableActionButtons() {
        //     foreach (var button in buttons) {
        //         button.interactable = true;
        //     }
        // }
        //
        // private void DisableActionButtons() {
        //     foreach (var button in buttons) {
        //         button.interactable = false;
        //     }
        // }
        //
        // private void SetActionButtonsStatus(Player player) {
        //     for (int i = 0; i < buttons.Count; i++) {
        //         Actions currentAction = Actions.NONE;
        //         if (buttons[i].Id.Name == UIButtonId.CombatActionButtons.Rock.ToString()) {
        //             currentAction = Actions.ROCK;
        //         } else if (buttons[i].Id.Name == UIButtonId.CombatActionButtons.Paper.ToString()) {
        //             currentAction = Actions.PAPER;
        //         } else if (buttons[i].Id.Name == UIButtonId.CombatActionButtons.Scissor.ToString()) {
        //             currentAction = Actions.SCISSOR;
        //         } else if (buttons[i].Id.Name == UIButtonId.CombatActionButtons.Defense.ToString()) {
        //             currentAction = Actions.DEFENSE;
        //         } else if (buttons[i].Id.Name == UIButtonId.CombatActionButtons.Energy.ToString()) {
        //             currentAction = Actions.ENERGY;
        //         }
        //
        //         if (player.HasEnoughEnergy(currentAction)) {
        //             buttons[i].GetComponent<Image>().color = Color.white;
        //             buttons[i].interactable = true;
        //         } else {
        //             buttons[i].GetComponent<Image>().color = Color.red;
        //             buttons[i].interactable = false;
        //         }
        //     }
        // }
        //
        // public void SetThoughtIcon(Character character, bool isKnownIcon) {
        //     //bool isPlayer = character.GetType() == typeof(Player);
        //     bool isPlayer = character.IsPlayer;
        //     Debug.Log("[CombatUIController] SetThoughtIcon() -> " +
        //               (isPlayer ? "Player Thought Icon" : "Enemy Thought Icon = " + character.thinkingAction));
        //     if (!isPlayer) {
        //         Debug.Log("[CombatUIController] SetThoughtIcon() -> Enemy isKnownIcon " + isKnownIcon);
        //     }
        //
        //     Image targetImage = isPlayer ? playerThoughtImage : enemyThoughtImage;
        //     targetImage.sprite = GetActionSprite(character, isKnownIcon);
        //     ShowThoughtIcon(character, true);
        // }
        //
        // public void SetActionIcon(Character character) {
        //     //bool isPlayer = character.GetType() == typeof(Player);
        //     bool isPlayer = character.IsPlayer;
        //     Debug.Log("[CombatUIController] SetActionIcon() -> " +
        //               (isPlayer ? "Player Action Icon" : "Enemy Action Icon"));
        //     Image targetImage = isPlayer ? playerActionImage : enemyActionImage;
        //     targetImage.sprite = GetActionSprite(character);
        //     ShowActionIcon(character, true);
        // }
        //
        // private void ShowThoughtIcon(Character character, bool isShowing) {
        //     //bool isPlayer = character.GetType() == typeof(Player);
        //     bool isPlayer = character.IsPlayer;
        //     Debug.Log("[CombatUIController] ShowThoughtIcon() -> " +
        //               (isPlayer ? "Player Thought Icon" : "Enemy Thought Icon"));
        //     GameObject targetHolder = isPlayer ? PlayerThoughtHolder : EnemyThoughtHolder;
        //     targetHolder.SetActive(isShowing);
        // }
        //
        // private void ShowActionIcon(Character character, bool isShowing) {
        //     //bool isPlayer = character.GetType() == typeof(Player);
        //     bool isPlayer = character.IsPlayer;
        //     Debug.Log("[CombatUIController] ShowActionIcon() -> " +
        //               (isPlayer ? "Player Action Icon" : "Enemy Action Icon"));
        //     GameObject targetHolder = isPlayer ? PlayerActionHolder : EnemyActionHolder;
        //     targetHolder.SetActive(isShowing);
        // }
        //
        // //TODO: currentLanguage en clase Character en vez de en los hijos
        // private Sprite GetActionSprite(Character character, bool isKnownIcon = true) {
        //     Language targetLanguage;
        //     Actions currentAction = character.thinkingAction != Actions.NONE
        //         ? character.thinkingAction
        //         : character.currentAction;
        //
        //     //if (character.GetType() == typeof(Enemy)) {
        //     if (!character.IsPlayer) {
        //         targetLanguage = ((Enemy) character).currentLanguage;
        //     } else {
        //         targetLanguage = ((Player) character).commonLanguage.data;
        //     }
        //
        //     return targetLanguage.GetActionIcon(isKnownIcon ? currentAction : Actions.NONE);
        // }
        //
        // private void SetDamageIcon(Character character, bool isShowing) {
        //     //bool isPlayer = character.GetType() == typeof(Player);
        //     bool isPlayer = character.IsPlayer;
        //     TextMeshProUGUI targetDamageText = isPlayer ? playerDamageText : enemyDamageText;
        //     targetDamageText.text = character.currentTotalDamage.ToString("D2");
        //     targetDamageText.gameObject.SetActive(isShowing);
        // }
        //
        // private void SetActionHoldersSize(int combatResult = 0) {
        //     Vector3 playerTargetScale = Vector3.one;
        //     Vector3 enemyTargetScale = Vector3.one;
        //     if (combatResult > 0) {
        //         playerTargetScale = PlayerActionHolder.transform.localScale * 2f;
        //         enemyTargetScale = EnemyActionHolder.transform.localScale * 0.5f;
        //     } else if (combatResult < 0) {
        //         playerTargetScale = PlayerActionHolder.transform.localScale * 0.5f;
        //         enemyTargetScale = EnemyActionHolder.transform.localScale * 2f;
        //     }
        //
        //     float duration = combatResult == 0 ? 0 : 2f;
        //     StartCoroutine(ChangeLocalScaleOverTime(PlayerActionHolder.transform,
        //         playerTargetScale, duration));
        //     StartCoroutine(ChangeLocalScaleOverTime(EnemyActionHolder.transform,
        //         enemyTargetScale, duration));
        // }
        //
        // private void SetHealthBar(Character character) {
        //     Debug.Log("[CombatUIController] SetHealthBar()");
        //     //bool isPlayer = character.GetType() == typeof(Player);
        //     bool isPlayer = character.IsPlayer;
        //     Scrollbar targetHealthBar = isPlayer ? playerHealthBar : enemyHealthBar;
        //     float calculatedSize = (float) (((float) character.currentHealth / (float) character.maxHealth));
        //     if (calculatedSize < targetHealthBar.size) {
        //         if (isPlayer) {
        //             PlayerAnimancer.Play(combatDamageAnim);
        //         } else {
        //             EnemyAnimancer.Play(combatDamageAnim);
        //         }
        //     }
        //
        //     StartCoroutine(ChangeScrollbarSizeOverTime(targetHealthBar, calculatedSize, 0.5f));
        // }
        //
        // private void SetEnergyBar(Character character, bool isPrevision = false) {
        //     Debug.Log("[CombatUIController] SetEnergyBar()");
        //     //bool isPlayer = character.GetType() == typeof(Player);
        //     bool isPlayer = character.IsPlayer;
        //     int actionCost = character.currentAction == Actions.NONE
        //         ? character.ActionCost(character.thinkingAction)
        //         : character.ActionCost(character.currentAction);
        //     Scrollbar targetEnergyBar = isPlayer ? playerEnergyBar : enemyEnergyBar;
        //     Scrollbar targetPrevisionEnergyBar = isPlayer ? playerPrevisionEnergyBar : enemyPrevisionEnergyBar;
        //
        //     float characterCurrentEnergy = character.currentEnergy;
        //     float characterMaxEnergy = character.maxEnergy;
        //     float calculatedSize =
        //         (float) ((characterCurrentEnergy - (float) actionCost) / characterMaxEnergy);
        //
        //     if (isPrevision) {
        //         if (actionCost > 0) {
        //             StartCoroutine(ChangeScrollbarSizeOverTime(targetEnergyBar, calculatedSize, 0.1f));
        //         } else {
        //             StartCoroutine(ChangeScrollbarSizeOverTime(targetPrevisionEnergyBar, calculatedSize, 0.1f));
        //         }
        //     } else {
        //         StartCoroutine(ChangeScrollbarSizeOverTime(targetPrevisionEnergyBar, calculatedSize, 0.5f));
        //         if (targetEnergyBar.size != calculatedSize) {
        //             StartCoroutine(ChangeScrollbarSizeOverTime(targetEnergyBar, calculatedSize, 0f));
        //         }
        //     }
        // }
        //
        // IEnumerator ChangeScrollbarSizeOverTime(Scrollbar targetScrollbar, float targetSize, float duration) {
        //     float initialSize = targetScrollbar.size;
        //     float elapsedTime = 0;
        //     while (elapsedTime < duration) {
        //         elapsedTime += Time.deltaTime;
        //         float blend = Mathf.Clamp01(elapsedTime / duration);
        //         targetScrollbar.size = Mathf.Lerp(initialSize, targetSize, blend);
        //         yield return null;
        //     }
        //
        //     targetScrollbar.size = targetSize;
        // }
        //
        // IEnumerator ChangeLocalScaleOverTime(Transform targetTransform, Vector3 targetScale, float duration) {
        //     Vector3 initialScale = targetTransform.localScale;
        //     float elapsedTime = 0;
        //     while (elapsedTime < duration) {
        //         elapsedTime += Time.deltaTime;
        //         float blend = Mathf.Clamp01(elapsedTime / duration);
        //         initialScale.x = Mathf.Lerp(initialScale.x, targetScale.x, blend);
        //         initialScale.y = Mathf.Lerp(initialScale.y, targetScale.y, blend);
        //         initialScale.z = Mathf.Lerp(initialScale.z, targetScale.z, blend);
        //         targetTransform.localScale = initialScale;
        //         yield return null;
        //     }
        //
        //     targetTransform.localScale = targetScale;
        // }
        //
        // #endregion
    }
}