using System;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Par layout-prefab para ComicPlayerUIController.
	/// Evita depender del orden del array — cada entrada declara explícitamente
	/// qué ComicPageLayout representa.
	/// </summary>
	[Serializable]
	public class ComicLayoutEntry
	{
		[SerializeField] public ComicPageLayout Layout;
		[SerializeField] public GameObject Prefab;
	}
}
