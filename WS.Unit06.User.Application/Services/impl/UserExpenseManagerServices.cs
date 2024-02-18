using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Diagnostics;
using System.Net.Mime;
using System.Xml.Linq;
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

        public int createGroup(string name)
        {
            var request = new RestRequest("/api/groups", Method.Post);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            request.AddJsonBody(JsonConvert.SerializeObject( new { name }));
            //dynamic response = JsonConvert.DeserializeObject<dynamic>(
            //    _restClient.ExecuteAsync<dynamic>(request).Result.Content);
            dynamic response = _restClient.ExecuteAsync(request).Result;
            int codeReturn=0;
			if (response != null) {
                var locationHeaderValue = response.Headers[3].Value;
                codeReturn = getLocationUrl(locationHeaderValue);
			}
            return codeReturn;
            //Debug.WriteLine("valor en UserExpenseManager:" + response);
        }

		public int deleteGroup(int id)
		{
			var request = new RestRequest("/api/groups/{id}", Method.Delete);
            request.AddParameter("id", id,ParameterType.UrlSegment);
			dynamic response = _restClient.ExecuteAsync(request).Result;
			int codeReturn = 0;
			if (response != null)
			{
				//var locationHeaderValue = response.Headers[3].Value;
				//codeReturn = getLocationUrl(locationHeaderValue);
                codeReturn= id;
			}
			return codeReturn;
		}

		public List<GroupDTO> getAllCroup()
        {
            var request = new RestRequest("/api/groups", Method.Get);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            dynamic response = _restClient.ExecuteAsync(request).Result;
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var groupDTOs = JsonConvert.DeserializeObject<List<GroupDTO>>(content);
				return groupDTOs;
			}
			else
			{
				return null;
			}
		}

        private int getLocationUrl(string headerLocation)
        {
			if (!string.IsNullOrEmpty(headerLocation))
			{
				int lastSlashIndex = headerLocation.LastIndexOf('/') + 1;
				if (lastSlashIndex != -1)
				{
					string codeLocation = headerLocation.Substring(lastSlashIndex, headerLocation.Length - lastSlashIndex);
					if (int.TryParse(codeLocation, out int number))
						return number;
				}
			}
            return 0;
		}
    }
}
