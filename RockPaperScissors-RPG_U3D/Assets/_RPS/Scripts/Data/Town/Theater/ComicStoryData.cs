using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos de una historia completa. Título, miniatura y lista de páginas.
	/// </summary>
	[Serializable]
	public class ComicStoryData
	{
		[SerializeField] private string _title;
		[SerializeField] private Sprite _thumbnail;
		[InlineEditor, SerializeField] private List<ComicPageScrObj> _pages;

		public string Title => _title;
		public Sprite Thumbnail => _thumbnail;
		public List<ComicPageScrObj> Pages => _pages;
	}
}
