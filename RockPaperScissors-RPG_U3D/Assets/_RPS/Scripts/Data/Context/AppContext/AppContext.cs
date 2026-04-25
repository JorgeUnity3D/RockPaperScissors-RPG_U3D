using System.Collections.Generic;

namespace Kapibara.RPS
{
    /// <summary>
    /// Punto de acceso global al contexto de juego activo. Todos los sistemas leen datos de aquí.
    /// </summary>
    public static class AppContext
    {
        private static GameContext _gameContext;
        /// <summary>
        /// Contexto completo de la partida activa. Asignarlo reemplaza todos los datos de juego.
        /// </summary>
        public static GameContext GameContext
        {
	        get
	        {
		        return _gameContext;
	        }
            set
            {
                _gameContext = value;
            }
        }

        /// <summary>Marca de tiempo de la partida activa.</summary>
        public static string TimeStamp => _gameContext.Timestamp;
        /// <summary>Datos del jugador de la partida activa.</summary>
        public static Player Player => _gameContext.Player;
        /// <summary>Lista de todos los atributos del jugador.</summary>
        public static List<StatAttribute> Attributes => _gameContext.Player.Attributes;
        /// <summary>Contexto de la ciudad (edificios, estado de desbloqueo).</summary>
        public static TownContext TownContext => _gameContext.TownContext;
        /// <summary>Lista de datos de cada edificio de la ciudad.</summary>
        public static List<TownData> TownData => _gameContext.TownContext.TownData;
        /// <summary>Contexto del combate activo. Asignado por TravelManager antes de cargar la escena Combat; null fuera de combate.</summary>
        public static CombatContext CombatContext { get; set; }
    }
}