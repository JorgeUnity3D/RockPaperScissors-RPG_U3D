using System;
using System.ComponentModel;
using System.Reflection;
namespace Kapibara.RPS.Extensions
{
	public static class EnumExtensions
	{
		public static string Name(this Enum enumValue)
		{
			
			Type enumType = enumValue.GetType();
			FieldInfo field = enumType.GetField(enumValue.ToString());
			object[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			return attributes.Length == 0 ? enumValue.ToString() : ((DescriptionAttribute)attributes[0]).Description;
		}
	}
}