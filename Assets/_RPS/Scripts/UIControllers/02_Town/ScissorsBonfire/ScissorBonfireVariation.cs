using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{

	public class ScissorBonfireVariation : MonoBehaviour
	{

		[SerializeField] private Image _variationImage;
		[SerializeField] private Image _backgroundImage;
		
		public void SetData(Sprite variationSprite, Color variationColor)
		{
			_variationImage.sprite = variationSprite;
			_backgroundImage.color = variationColor;
		}
		
	}
}