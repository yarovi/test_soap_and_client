using Newtonsoft.Json;
using System.Text.Json;

namespace WS.Unit06.User.Web.Util
{
	/*public static  class SessionExtensions
	{
		public static void SetObject(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}

		public static T GetObject<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}*/
}
