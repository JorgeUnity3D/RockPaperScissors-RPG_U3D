using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "ComicPage", menuName = "RPSRPG/Theater/ComicPage")]
	public class ComicPageScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private ComicPageData _data;
		public ComicPageData Data => _data;
	}
}
