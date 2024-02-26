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

        public SupermarketOffersServices()
        {
            // TODO: esto hago porque no me este jalando por defecto.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            GlobalSetting.ExternalApiUrl = config.GetValue<string>("WebSettings:AppExternalEndPoint"); 

            _restClient = new RestClient(GlobalSetting.ExternalApiUrl);

        }

        public List<MercadonaDTO> getAllMercadonaOffers()
        {
            var request = new RestRequest("/offert-mercadona", Method.Get);

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
            var request = new RestRequest("/offert-carrefour", Method.Get);

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