using UnityEngine;

namespace Kapibara.RPS.Extensions
{
	public static class RendererExtensions
	{
		public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
		}

		public static bool IsVisibleFrom(this Renderer[] renderers, Camera camera)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			bool isVisible = false;
			foreach (var renderer in renderers)
			{
				if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
				{
					isVisible = true;
				}
			}

			return isVisible;
		}

		public static bool IsVisibleFrom(this Bounds bounds, Camera camera)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, bounds);
		}
	}
}