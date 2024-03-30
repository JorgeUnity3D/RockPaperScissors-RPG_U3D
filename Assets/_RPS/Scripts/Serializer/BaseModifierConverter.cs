/*
 * https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
 */

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kapibara.RPS
{
	public class BaseModifierConverter : JsonConverter
	{
		static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(BaseModifier));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);
			switch (jo["ObjType"].Value<int>())
			{
				case 0:
					return JsonConvert.DeserializeObject<TrainingModifier>(jo.ToString(), SpecifiedSubclassConversion);
				case 1:
					return JsonConvert.DeserializeObject<SkillTreeModifier>(jo.ToString(), SpecifiedSubclassConversion);
				default:
					throw new Exception();
			}
			throw new NotImplementedException();
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException(); // won't be called because CanWrite returns false
		}
	}
}