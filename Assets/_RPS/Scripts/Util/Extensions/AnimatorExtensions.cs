using UnityEngine;

namespace Kapibara.RPS.Extensions
{
	public static class AnimatorExtensions
	{
		public static bool IsTargetAnimationPlaying(this Animator animator)
		{
			return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= animator.GetCurrentAnimatorStateInfo(0).length;
		}

		public static bool IsTargetAnimationPlaying(this Animator animator, string animationName)
		{
			AnimatorClipInfo[] aci = animator.GetCurrentAnimatorClipInfo(0);
			foreach (AnimatorClipInfo acii in aci)
			{
				Debug.Log("IsTargetAnimationPlaying -> " + acii.clip);
			}

			return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
		}

		public static bool IsAnyAnimationPlaying(this Animator animator)
		{
			return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		}
	}
}