using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// ScriptableObject maestro del Theater. Lista ordenada de historias del juego.
	/// Historia 0 = se muestra al iniciar nueva partida; 1-N se desbloquean al derrotar jefes.
	/// Crear el asset desde: RPSRPG/Theater/TheaterData.
	/// </summary>
	[CreateAssetMenu(fileName = "TheaterData", menuName = "RPSRPG/Theater/TheaterData")]
	public class TheaterScrObj : SerializedScriptableObject
	{
		[HideLabel, InlineEditor, SerializeField] private List<ComicStoryScrObj> _data;
		public List<ComicStoryScrObj> Data => _data;
	}
}
