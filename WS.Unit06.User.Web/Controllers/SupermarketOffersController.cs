using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSUseExpenseManagerClient;
using WSClient.SupermarketOffers;
using WSClient.ApplicationWS;
using WS.Unit06.User.Web.Controllers;

namespace Web.Mvc.Ofertas.Controllers
{
    public class SupermarketOffersController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public SupermarketOffersController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Error", "SupermarketOffers");


            var client = new SupermarketOffersServicesClient();
            var mercadonaOffertsDTO = client.getAllMercadonaOffersAsync().Result;
            var carrefourOffertsDTO = client.getAllCarrefourOffersAsync().Result;

            var viewModel = new OffersViewModel
            {
                MercadonaOffers = mercadonaOffertsDTO,
                CarrefourOffers = carrefourOffertsDTO
            };

            return View(viewModel);


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        public class OffersViewModel
        {
            public MercadonaDTO[] MercadonaOffers { get; set; }
            public CarrefourDTO[] CarrefourOffers { get; set; }
        }
    }
}