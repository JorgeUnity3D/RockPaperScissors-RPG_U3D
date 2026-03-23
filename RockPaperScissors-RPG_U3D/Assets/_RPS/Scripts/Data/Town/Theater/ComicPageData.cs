using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos de una página de comic. Layout y lista de viñetas inline.
	/// El número de viñetas debe coincidir con los slots del prefab de layout correspondiente.
	/// </summary>
	[Serializable]
	public class ComicPageData
	{
		[SerializeField] private ComicPageLayout _layout;
		[SerializeField] private List<ComicVignetteData> _vignettes;

		public ComicPageLayout Layout => _layout;
		public List<ComicVignetteData> Vignettes => _vignettes;
	}
}
