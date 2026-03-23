using UnityEngine;
using UnityEngine.UI;

namespace Kapibara.RPS
{

	/// <summary>
	/// Componente visual que muestra el icono y color de fondo de la variación de una estadística al subir de nivel.
	/// </summary>
	public class ScissorBonfireVariation : MonoBehaviour
	{

		[SerializeField] private Image _variationImage;
		[SerializeField] private Image _backgroundImage;
		
		/// <summary>Actualiza el icono y el color de fondo según la variación positiva, neutra o negativa de la estadística.</summary>
		public void SetData(Sprite variationSprite, Color variationColor)
		{
			_variationImage.sprite = variationSprite;
			_backgroundImage.color = variationColor;
		}
		
	}
}