using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Una página de la Biblioteca. Agrupa un conjunto de quests.
	/// Se desbloquea la siguiente página cuando todas las quests de ésta están completadas (Phase 6).
	/// </summary>
	[Serializable]
	public class LibraryPageData
	{
		[SerializeField] private List<LibraryQuestData> _quests;

		public List<LibraryQuestData> Quests => _quests;
	}
}
