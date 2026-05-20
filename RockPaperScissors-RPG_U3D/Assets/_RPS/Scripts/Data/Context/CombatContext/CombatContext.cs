namespace Kapibara.RPS
{
	/// <summary>
	/// Estado transient de la sesión de combate activa. No se serializa a disco.
	/// TravelManager lo crea antes de cargar la escena Combat; CombatStepManager lo consume y avanza CurrentStepIndex.
	/// </summary>
	public class CombatContext
	{
		public MapLevel SelectedLevel    { get; private set; }
		public int      CurrentStepIndex { get; set; }
		public MapStep  CurrentStep      => SelectedLevel.Steps[CurrentStepIndex];

		public CombatContext(MapLevel level)
		{
			SelectedLevel    = level;
			CurrentStepIndex = 0;
		}
	}
}
