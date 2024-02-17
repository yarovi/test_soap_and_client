using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Diagnostics;
using System.Net.Mime;
using WS.Unit06.User.Application.Model;
using WS.Unit06.User.Application.util;

namespace WS.Unit06.User.Application.Services.impl
{
    public class UserExpenseManagerServices : IUserExpenseManagerServices
    {
        private RestClient _restClient;

        public UserExpenseManagerServices()
        {
            // TODO: esto hago porque no me este jalando por defecto.
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
            GlobalSetting.ApiUrl = config.GetValue<string>("WebSettings:AppEndPoint");
            /*var options = new RestClientOptions
            (_configuration.GetValue<string>("WebSettings:AppEndPoint"));
            options.RemoteCertificateValidationCallback =
                               (sender, certificate, chain, sslPolicyErrors) => true;
            _restClient = new RestClient(options);*/
            _restClient= new RestClient(GlobalSetting.ApiUrl);
        }

        public void createGroup(string name)
        {
            var request = new RestRequest("/api/groups", Method.Post);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            request.AddJsonBody(JsonConvert.SerializeObject( new { name }));
            dynamic response = JsonConvert.DeserializeObject<dynamic>(
                _restClient.ExecuteAsync<dynamic>(request).Result.Content);
            Debug.WriteLine("valor en UserExpenseManager:" + response);
        }

        public List<GroupDTO> getAllCroup()
        {
            var request = new RestRequest("/api/groups", Method.Get);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            dynamic response = _restClient.ExecuteAsync(request).Result;
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;

				// Deserializar la respuesta a una lista de objetos GroupDTO
				var groupDTOs = JsonConvert.DeserializeObject<List<GroupDTO>>(content);

				return groupDTOs;
			}
			else
			{
				// Manejar el caso en que la llamada no sea exitosa
				return null;
			}
		}
    }
}
