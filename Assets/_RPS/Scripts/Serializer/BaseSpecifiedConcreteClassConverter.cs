/*
 * https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
 */

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kapibara.RPS
{
	public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
	{
		protected override JsonConverter ResolveContractConverter(Type objectType)
		{
			if (typeof(BaseModifier).IsAssignableFrom(objectType) && !objectType.IsAbstract)
				return null;
			return base.ResolveContractConverter(objectType);
		}
	}
}