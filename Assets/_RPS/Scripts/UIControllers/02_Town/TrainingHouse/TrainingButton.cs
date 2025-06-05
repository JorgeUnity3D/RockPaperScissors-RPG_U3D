using System.Collections.Generic;
using Kapibara.Util.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class TrainingButton : MonoBehaviour
	{

		[SerializeField] private Image icon;
		[SerializeField] private List<Image> levels;
		[SerializeField] private Image selectionOverlay;

		public void SetButtonData(TrainingHouseModifier trainingHouseModifier, Sprite icon, UnityAction<TrainingHouseModifier> OnButtonClick)
		{
			Debug.Log($"[TrainingButton] SetButtonData() -> ");
			SetImage(trainingHouseModifier.IsUnlocked ? icon : null);
			SetLevel(trainingHouseModifier.Level, trainingHouseModifier.IsUnlocked);
			SetSelectionOverlay(trainingHouseModifier.IsTraining);
			GetComponent<Button>().AddListener(() =>
			{
				OnButtonClick?.Invoke(trainingHouseModifier);
			}, true);
		}

		public void SetImage(Sprite newSprite)
		{
			icon.sprite = newSprite != null ? newSprite : icon.sprite;
		}

		public void SetLevel(int level, bool isUnlocked)
		{
			if (!isUnlocked)
			{
				levels.ForEach(levelImg => levelImg.color = Color.white);
				return;
			}
			for (int i = 0; i < levels.Count; i++)
			{
				levels[i].color = i < level ? Color.green : Color.white;
			}
		}

		public void SetSelectionOverlay(bool isSelected)
		{
			selectionOverlay.enabled = isSelected;
		}
	}
}