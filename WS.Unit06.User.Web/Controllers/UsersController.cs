using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WSClient.ApplicationWS;

namespace WS.Unit06.User.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationServicesClient _client;
        public UsersController() { _client = new ApplicationServicesClient(); }
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Error", "Home");
            return View(_client.getUsersAsync().Result);
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
    }
}
