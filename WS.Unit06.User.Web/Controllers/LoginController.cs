using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WS.Unit06.User.Application;
using WSClientToken;

namespace WS.Unit06.User.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult startLogin()
        {
            return View();
        }
		[HttpPost]
		public IActionResult startLogin(string username,string pwd)
		{
            /*
			var clientApp = new AuthServices();
			var responseAuth = clientApp.authenticate();
			if (responseAuth != null) {
				var token = OperationContext.Current.OutgoingMessageHeaders.GetHeader<string>("token","");
				HttpContext.Session.Set("token", Encoding.UTF8.GetBytes(token));
                Console.WriteLine("iniciando loginc on los datos");
                return RedirectToAction("Index", "Home");
            }
            else
			{
                return RedirectToAction("LoginFailed", "Login");
            }*/
            var client = new AuthServicesClient();
            using (var scope = new OperationContextScope(client.InnerChannel))
            {
                //var emailHeader = MessageHeader.CreateHeader("username", "", username);
               // var passwordHeader = MessageHeader.CreateHeader("password", "", pwd);

                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers["username"] = username;
                httpRequestProperty.Headers["password"] = pwd;
                OperationContext.Current.
                    OutgoingMessageProperties[HttpRequestMessageProperty.Name] = 
                    httpRequestProperty;

                var responseAuth = client.authenticateAsync().Result;

                if (responseAuth != null && !string.IsNullOrEmpty(responseAuth))
                {
                    HttpContext.Session.SetString("token", responseAuth);

                    Console.WriteLine("iniciando login ..!");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Mensaje"] = "Credenciales no validas";
                    return View();
                }
            }
        }
		public IActionResult createAccount()
		{

			return View();
		}
	}
	
}
