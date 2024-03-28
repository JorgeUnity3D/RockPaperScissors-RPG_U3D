using UnityEngine;

namespace Kapibara.RPS.Extensions
{
	public static class AudioExtensions
	{
		public static bool IsPlayingAndFinishing(this AudioSource audioSource)
		{
			return (audioSource.isPlaying && ((audioSource.time) / audioSource.clip.length > 90f / 100f));
		}
	}
}