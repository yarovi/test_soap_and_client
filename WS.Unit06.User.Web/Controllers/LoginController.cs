using Microsoft.AspNetCore.Mvc;

namespace WS.Unit06.User.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult startLogin()
        {
            return View();
        }
		[HttpPost]
		public IActionResult startLogin(string email,string pwd)
		{
			Console.WriteLine("iniciando loginc on los datos");
			return RedirectToAction("Index", "Home");
		}
		public IActionResult createAccount()
		{

			return View();
		}
	}
	
}
