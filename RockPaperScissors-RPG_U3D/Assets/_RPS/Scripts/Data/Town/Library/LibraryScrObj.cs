using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// ScriptableObject de configuración de la Biblioteca.
	/// Contiene todas las páginas y quests. Asignar en el Inspector del LibraryManager.
	/// Crear desde: RPSRPG/Library/LibraryData
	/// </summary>
	[CreateAssetMenu(fileName = "Library", menuName = "RPSRPG/Town/Library")]
	public class LibraryScrObj : SerializedScriptableObject
	{
		[HideLabel, SerializeField] private LibraryData _data;

		public LibraryData Data => _data;
	}
}
