namespace Kapibara.RPS
{
	/// <summary>
	/// Estado transient de la sesión de combate activa. No se serializa a disco.
	/// Se asigna en TravelManager antes de cargar la escena Combat y se lee en CombatManager.
	/// Phase 4 añadirá más campos (HP actual, energía, ronda...).
	/// </summary>
	public class CombatContext
	{
		public MapLevel SelectedLevel { get; set; }
	}
}
