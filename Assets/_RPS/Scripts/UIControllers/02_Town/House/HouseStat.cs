using System.Collections.Generic;
using Kapibara.Util.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	public class HouseStat : MonoBehaviour
	{
		[SerializeField] private List<Image> _icons;
		[SerializeField] private TextMeshProUGUI _baseText;
		[SerializeField] private TextMeshProUGUI _scissorBonfireModText;
		[SerializeField] private TextMeshProUGUI _trainingHouseModText;
		[SerializeField] private TextMeshProUGUI _paperTreeModText;
		[SerializeField] private TextMeshProUGUI _totalText;

		#region CONTROL

		public void SetData(StatAttribute attribute, Sprite statIcon)
		{
			UpdateAttributeText(attribute);
			UpdateModifierText<ScissorBonfireModifier>(attribute, _scissorBonfireModText);
			UpdateModifierText<TrainingHouseModifier>(attribute, _trainingHouseModText);
			UpdateModifierText<PaperTreeModifier>(attribute, _paperTreeModText);
			_icons.ForEach(icon => icon.sprite = statIcon);
		}

		private void UpdateAttributeText(StatAttribute attribute)
		{
			string statName = attribute.Stat.Name();
			//Base
			_baseText.text = attribute.AttributeValue.ToString();
			Transform baseParent = _baseText.transform.parent;
			string baseName = statName + "_" + baseParent.name.Replace("Label_Text", "");
			baseParent.GetComponent<TextMeshProUGUI>().text = baseName;
			//Total
			_totalText.text = attribute.TotalValue.ToString();
			Transform totalParent = _totalText.transform.parent;
			string totalName = statName + "_" + totalParent.name.Replace("Label_Text", "");
			totalParent.GetComponent<TextMeshProUGUI>().text = totalName;
		}

		private void UpdateModifierText<T>(StatAttribute attribute, TextMeshProUGUI textMesh) where T : BaseModifier
		{
			if (textMesh == null)
			{
				return;
			}
			T modifier = attribute.GetModifier<T>();
			if (modifier == null)
			{
				GameObject parentTextMesh = textMesh.transform.parent.gameObject;
				if (parentTextMesh != null)
				{
					Destroy(parentTextMesh);
				}
			}
			else
			{
				textMesh.text = modifier.TotaModifier.ToString();
				string statName = modifier.Stat.Name();
				var modParent = textMesh.transform.parent;
				string modName = statName + "_" + modParent.name.Replace("Label_Text", "");
				modParent.GetComponent<TextMeshProUGUI>().text = modName;
			}
		}

		#endregion
	}
}