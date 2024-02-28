using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSUseExpenseManagerClient;
using WSClient.SupermarketOffers;
using WSClient.ApplicationWS;
using WS.Unit06.User.Web.Controllers;
using System.ServiceModel;

namespace Web.Mvc.Ofertas.Controllers
{
    public class SupermarketOffersController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly SupermarketOffersServicesClient _client;
         
        public SupermarketOffersController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            var configuration1 = configuration;
            var globalenv = configuration1["OFFERS_SERVICE_URL"] ??
                            configuration1["WebSettings:OffersServiceURL"];
            Console.WriteLine("Exist AUTH url or not " + globalenv); 

            _client = new SupermarketOffersServicesClient(new BasicHttpBinding(), new EndpointAddress(globalenv));
        }

        public IActionResult Index()
        {

            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Error", "SupermarketOffers");


            //var client = new SupermarketOffersServicesClient();
            var mercadonaOffertsDTO = _client.getAllMercadonaOffersAsync().Result;
            var carrefourOffertsDTO = _client.getAllCarrefourOffersAsync().Result;

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