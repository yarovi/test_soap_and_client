using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WSAuthClient;

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

            var client = new AuthServicesClient();
            using (var scope = new OperationContextScope(client.InnerChannel))
            {

                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers["username"] = username;
                httpRequestProperty.Headers["password"] = pwd;
                OperationContext.Current.
                    OutgoingMessageProperties[HttpRequestMessageProperty.Name] = 
                    httpRequestProperty;

                var responseAuth = client.authenticateAsync().Result;

                if (responseAuth != null && responseAuth.code==201 && OperationContext.Current.IncomingMessageProperties.TryGetValue(HttpResponseMessageProperty.Name, out var responseMessage) &&
		responseMessage is HttpResponseMessageProperty httpResponse)
                {

					string token = httpResponse.Headers["token"];
					HttpContext.Session.SetString("token", token);
					HttpContext.Session.SetString("username", username);
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
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
			HttpContext.Session.Remove("SelectedUsers");
			return RedirectToAction("StartLogin", "Login"); 
        }
    }
	
}
