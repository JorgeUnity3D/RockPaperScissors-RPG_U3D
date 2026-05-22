using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	[CreateAssetMenu(fileName = "ComicStory", menuName = "RPSRPG/Town/Theater/ComicStory")]
	public class ComicStoryScrObj : SerializedScriptableObject
	{
		[SerializeField] private int           _storyId = -1;
		[HideLabel, SerializeField] private ComicStoryData _data;

		public int            StoryId => _storyId;
		public ComicStoryData Data    => _data;
	}
}
