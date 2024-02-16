using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WS.Unit06.User.Web.Models;

namespace WS.Unit06.User.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
        public IActionResult Index()
		{
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
				return RedirectToAction("Error", "Home");
            return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View();
		}
	}
}
