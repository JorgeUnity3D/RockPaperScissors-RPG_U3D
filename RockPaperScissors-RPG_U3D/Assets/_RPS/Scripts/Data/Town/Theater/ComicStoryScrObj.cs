using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "ComicStory", menuName = "RPSRPG/Town/Theater/ComicStory")]
	public class ComicStoryScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private ComicStoryData _data;
		public ComicStoryData Data => _data;
	}
}
