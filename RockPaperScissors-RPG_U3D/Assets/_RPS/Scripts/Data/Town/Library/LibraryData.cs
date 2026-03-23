using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos de configuración completos de la Biblioteca.
	/// Contiene todas las páginas y sus quests. Asignado en LibraryScrObj.
	/// </summary>
	[Serializable]
	public class LibraryData
	{
		[SerializeField] private List<LibraryPageData> _pages;

		public List<LibraryPageData> Pages => _pages;
	}
}
