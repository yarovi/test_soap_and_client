using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.ServiceModel;
using WSClient.ApplicationWS;

namespace WS.Unit06.User.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationServicesClient _client;

        public UsersController(IConfiguration configuration)
        {
            var configuration1 = configuration;
            var globalenv = configuration1["APP_SERVICE_URL"] ?? configuration1["WebSettings:AppServiceURL"];
            Console.WriteLine("Global app is: " + globalenv);
            _client = new ApplicationServicesClient(new BasicHttpBinding(), new EndpointAddress(globalenv));
        }
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Error", "Home");
			var username = HttpContext.Session.GetString("username");
			if (string.IsNullOrEmpty(username))
				return RedirectToAction("Error", "Home");
			return View( _client.GetOneUserAsync(username).Result );
        }
        public IActionResult ManageUser(int? id)
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Error", "Home");
            else
            {
                if (id != null)
                {
                    var modelo = _client.GetUserByIdAsync((int)id);
                    return View("CreateUsers", modelo.Result);
                }
                return View("CreateUsers");
            }
        }

        [HttpPost]
        public IActionResult ManageUser(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                if (user.Id != 0)
                {
                    _client.UpdateUserAsync(user);
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    _client.CreateUserAsync(user);
                }
            }
			return RedirectToAction("startLogin", "Login");
		}

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            _client.DeleteUserAsync(id);
            return RedirectToAction("Index", "Users");
        }
    }
}
