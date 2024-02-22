using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WS.Unit06.User.Application.Model;

namespace WS.Unit06.User.Application.util
{
	public class CustomGroupDTOConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(GroupDTO);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject item = JObject.Load(reader);
			GroupDTO groupDTO = new GroupDTO();

			if (item.TryGetValue("idGroupCategory", out var idGroupCategory))
			{
				groupDTO.Id = (int)idGroupCategory;
			}
			else
			{
				if (item.TryGetValue("Id", out var id))
				{
					groupDTO.Id = (int)id;
				}
				else
				{
					throw new JsonSerializationException("No se pudo encontrar la propiedad 'idGroupCategory' o 'Id'.");
				}
			}
			if (item.TryGetValue("name", out var name))
			{
				groupDTO.Name = (string)name;
			}
			

			return groupDTO;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
