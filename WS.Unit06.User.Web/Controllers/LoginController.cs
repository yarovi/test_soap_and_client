using Microsoft.AspNetCore.Mvc;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WSAuthClient;
using WSUseExpenseManagerClient;

namespace WS.Unit06.User.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthServicesClient _clientAuth;
        private readonly UserExpenseManagerServicesClient _clientExpense;
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
              _configuration = configuration;
            var globalenv = _configuration["AUTH_SERVICE_URL"] ??
                            _configuration["WebSettings:AuthServiceURL"];
            Console.WriteLine("Exist AUTH url or not " + globalenv);

            var globalenv2 = _configuration["EXPENSE_SERVICE_URL"] ??
                             _configuration["WebSettings:ExpenseServiceURL"];
            Console.WriteLine("Exist AUTH url or not " + globalenv);

            _clientAuth = new AuthServicesClient(new BasicHttpBinding(), new EndpointAddress(globalenv));
            _clientExpense = new UserExpenseManagerServicesClient(new BasicHttpBinding(),
                new EndpointAddress(globalenv2));

        }
        public IActionResult startLogin()
        {
            return View();
        }
		[HttpPost]
		public IActionResult startLogin(string username,string pwd)
		{

            var globalenv = _configuration["AUTH_SERVICE_URL"] ??
                            _configuration["WebSettings:AuthServiceURL"];
            Console.WriteLine("Exist AUTH startLogin url or not " + globalenv);

            var _clientAuth2 = new AuthServicesClient(new BasicHttpBinding(), new EndpointAddress(globalenv));
            //var client = new AuthServicesClient();
            var client = _clientAuth2;

            Console.WriteLine("HEREEE!" + client);
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
                    setOwner();
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

        private void setOwner()
        {
			string token = HttpContext.Session.GetString("token");
			//var clientExpense = new UserExpenseManagerServicesClient();
            var clientExpense = _clientExpense;
            Console.WriteLine("TEst is owner: ");


            using (var scope = new OperationContextScope(clientExpense.InnerChannel))
			{
				HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
				httpRequestProperty.Headers["token"] = token;
				OperationContext.Current.
					OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
				httpRequestProperty;
				var isOwner = clientExpense.isOwnerAsync().Result;
				byte[] isOwnerBytes = BitConverter.GetBytes(isOwner);
				HttpContext.Session.Set("isOwner", isOwnerBytes);

			}
		}
    }
	
}
