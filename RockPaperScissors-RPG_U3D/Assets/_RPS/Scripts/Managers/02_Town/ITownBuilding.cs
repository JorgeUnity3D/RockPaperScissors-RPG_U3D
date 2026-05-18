namespace Kapibara.RPS
{
	/// <summary>
	/// Contrato para managers de Town que se abren al navegar a un edificio.
	/// TownManager lo usa para notificar la apertura sin llamar a Initialize() directamente.
	/// </summary>
	public interface ITownBuilding
	{
		void OnMenuOpen();
	}
}
