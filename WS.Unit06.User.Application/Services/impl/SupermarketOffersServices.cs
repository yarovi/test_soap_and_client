using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Mime;
using WS.Unit06.User.Application.Model;
using WS.Unit06.User.Application.util;

namespace WS.Unit06.User.Application.Services.impl
{
    public class SupermarketOffersServices : ISupermarketOffersServices
    {
        private RestClient _restClient;

        public SupermarketOffersServices(IConfiguration configuration)
        {
            var configuration1 = configuration;
            var globalenv = configuration1["OFFERS_SERVICE_URL"] ?? configuration1.GetValue<string>("WebSettings:OffersServiceURL");
            Console.WriteLine("Offers URL constructor: " + globalenv);

            GlobalSetting.ExternalApiUrl = globalenv;

            _restClient = new RestClient(GlobalSetting.ExternalApiUrl);

        }

        public List<MercadonaDTO> getAllMercadonaOffers()
        {
            var request = new RestRequest("/offers-mercadona", Method.Get);

            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            dynamic response = _restClient.ExecuteAsync(request).Result;



            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                var mercadonaOffertsDTO = JsonConvert.DeserializeObject<List<MercadonaDTO>>(content);


                return mercadonaOffertsDTO;
            }
            else
            {

                return new List<MercadonaDTO>();
            }
        }
        public List<CarrefourDTO> getAllCarrefourOffers()
        {
            var request = new RestRequest("/offers-carrefour", Method.Get);

            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            dynamic response = _restClient.ExecuteAsync(request).Result;



            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                var carrefourOffertsDTO = JsonConvert.DeserializeObject<List<CarrefourDTO>>(content);


                return carrefourOffertsDTO;
            }
            else
            {

                return new List<CarrefourDTO>();
            }
        }


    }
}