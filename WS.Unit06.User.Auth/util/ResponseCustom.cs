using System.Security.Claims;
using WS.Unit06.User.Auth.util;

namespace WS.Unit06.User.Application.util
{
    public class ResponseCustom
    {
        public int code {  get; set; }
        public string messageCustom { get; set; }
		public CustomClaim Claims { get; set; }
	}
}
