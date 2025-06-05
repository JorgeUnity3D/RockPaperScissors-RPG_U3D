/*
 * https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
 */

using System;
using Kapibara.RPS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kapibara.Util.Serialization
{
	public class BaseModifierConverter : JsonConverter
	{
		static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings()
		{
			ContractResolver = new BaseSpecifiedConcreteClassConverter()
		};

		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(BaseModifier));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);
			switch ((ModifierType)jo["ModifierType"].Value<int>())
			{
				case ModifierType.TRAININGHOUSE_MOD:
					return JsonConvert.DeserializeObject<TrainingHouseModifier>(jo.ToString(), SpecifiedSubclassConversion);
				case ModifierType.PAPERTREE_MOD:
					return JsonConvert.DeserializeObject<PaperTreeModifier>(jo.ToString(), SpecifiedSubclassConversion);
				case ModifierType.SCISSORBONFIRE_MOD:
					return JsonConvert.DeserializeObject<ScissorBonfireModifier>(jo.ToString(), SpecifiedSubclassConversion);
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