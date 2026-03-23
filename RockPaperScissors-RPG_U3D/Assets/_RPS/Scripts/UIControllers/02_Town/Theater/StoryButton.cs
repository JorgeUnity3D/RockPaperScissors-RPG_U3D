using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kapibara.RPS
{
	/// <summary>
	/// Componente del botón de historia en la galería del Theater.
	/// Muestra miniatura, título y estado bloqueado/desbloqueado.
	/// La acción de selección la inyecta TheaterUIController.
	/// </summary>
	public class StoryButton : MonoBehaviour
	{
		[SerializeField] private Button _button;
		[SerializeField] private Image _thumbnail;
		[SerializeField] private TMP_Text _titleText;
		[SerializeField] private GameObject _lockOverlay;

		/// <summary>
		/// Configura el botón con los datos de la historia y su estado de desbloqueo.
		/// </summary>
		public void SetUp(ComicStoryScrObj story, bool isUnlocked, UnityAction onSelect)
		{
			_thumbnail.sprite = story.Data.Thumbnail;
			_titleText.text = story.Data.Title;
			_titleText.gameObject.SetActive(isUnlocked);
			_lockOverlay.SetActive(!isUnlocked);
			_button.interactable = isUnlocked;
			_button.onClick.RemoveAllListeners();
			if (isUnlocked)
			{
				_button.onClick.AddListener(onSelect);
			}
		}
	}
}
