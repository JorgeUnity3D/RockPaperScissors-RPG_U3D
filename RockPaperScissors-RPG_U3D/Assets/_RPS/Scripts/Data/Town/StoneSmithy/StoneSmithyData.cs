using System;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos de configuración de los tres consumibles de la Herrería de Piedra.
	/// Los iconos de cada Item se asignan en el ScriptableObject y se inyectan en runtime.
	/// </summary>
	[Serializable]
	public class StoneSmithyData
	{
		public Item attackItem;
		public Item healItem;
		public Item energyItem;
	}
}
