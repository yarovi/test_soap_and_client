using Newtonsoft.Json;
using RestSharp;
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
            _restClient = new RestClient(GlobalSetting.ApiUrl);
        }



        public int createGroup(string name)
        {
            var request = new RestRequest("/api/groups", Method.Post);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            request.AddJsonBody(JsonConvert.SerializeObject(new { name }));
            //dynamic response = JsonConvert.DeserializeObject<dynamic>(
            //    _restClient.ExecuteAsync<dynamic>(request).Result.Content);
            dynamic response = _restClient.ExecuteAsync(request).Result;
            int codeReturn = 0;
            if (response != null) {
                var locationHeaderValue = response.Headers[3].Value;
                codeReturn = getLocationUrl(locationHeaderValue);
            }
            return codeReturn;
        }

        public int deleteGroup(int id)
        {
            var request = new RestRequest("/api/groups/{id}", Method.Delete);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            dynamic response = _restClient.ExecuteAsync(request).Result;
            int codeReturn = 0;
            if (response != null)
            {
                //var locationHeaderValue = response.Headers[3].Value;
                //codeReturn = getLocationUrl(locationHeaderValue);
                codeReturn = id;
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

        public int[] associateUserWithGroup(int[] ids,int groupId)
		{
			var request = new RestRequest("/api/user-groups/{groupId}/users", Method.Post);
			request.AddHeader("Accept", MediaTypeNames.Application.Json);
			request.AddParameter("groupId", groupId, ParameterType.UrlSegment);
			request.AddJsonBody( ids );
			dynamic response = _restClient.ExecuteAsync(request).Result;
			List<int> codesList = new List<int>();
			if (response != null && response.Headers != null)
			{
				foreach (var header in response.Headers)
				{
					if (header.Name.Equals("Location", StringComparison.OrdinalIgnoreCase))
					{
						string locationHeaderValue = header.Value;
						int code = getLocationUrl(locationHeaderValue);
						codesList.Add(code);
						break;
					}
				}
			}
			return codesList.ToArray();
		}

		public List<UserGroupDTO> getUserGroups()
		{
			var request = new RestRequest("/api/user-groups", Method.Get);
			dynamic response = _restClient.ExecuteAsync(request).Result;
			var userGroups = new List<UserGroupDTO>();
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var varItems = JsonConvert.DeserializeObject<List<dynamic>>(content);
				foreach (var obj in varItems)
				{
					UserGroupDTO userGroupDTO = new UserGroupDTO
					{
						Id = obj.id,
						NameGroup = obj.group.name,
						NameUser = obj.user.name,
						totalAmmount = obj.user.totalAmount
					};
					userGroups.Add(userGroupDTO);
				}
			}
			return userGroups;
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
				lastSlashIndex = headerLocation.LastIndexOf('?') - 1;
                if(lastSlashIndex != -1)
                {
                    int secondSlash= headerLocation.LastIndexOf('/') + 1;
					string codeLocation = headerLocation.Substring(lastSlashIndex,secondSlash);
					if (int.TryParse(codeLocation, out int number))
						return number;
				}
			}
            return 0;
		}

		public List<GroupDTO> getAllGroupByUser(int id)
		{
			var request = new RestRequest("/api/users/{userId}/groups", Method.Get);
			request.AddParameter("userId", id, ParameterType.UrlSegment);
			dynamic response = _restClient.ExecuteAsync(request).Result;
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var groupDTOs = JsonConvert.DeserializeObject<List<GroupDTO>>(content);
				return groupDTOs;
			}
			return null;
		}

        public int createTransaction(int idGroup, int idUser,string description,float expense)
        {
            //TODO SE va cambiar ya no se necesita el iduser
            var request = new RestRequest("/api/transactions", Method.Post);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            request.AddParameter("idGroup", idGroup, ParameterType.QueryString);
            request.AddParameter("idUser", idUser, ParameterType.QueryString);
            request.AddJsonBody(new  { description = description, expense=expense});
            dynamic response = _restClient.ExecuteAsync(request).Result;
            List<int> codesList = new List<int>();
            if (response != null && response.Headers != null)
            {
                foreach (var header in response.Headers)
                {
                    if (header.Name.Equals("Location", StringComparison.OrdinalIgnoreCase))
                    {
                        string locationHeaderValue = header.Value;
                        int code = getLocationUrl(locationHeaderValue);
                        codesList.Add(code);
                        break;
                    }
                }
            }
			return 0;//codesList.ToArray();
        }

        public List<TransactionDTO> getAllTransaction()
        {
            var request = new RestRequest("/api/transactions", Method.Get);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            dynamic response = _restClient.ExecuteAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                var transactionDTOs = JsonConvert.DeserializeObject<List<TransactionDTO>>(content);
                return transactionDTOs;
            }
            else
            {
                return null;
            }
        }

        public List<HistoryDTO> getHistoryTransaction(int idGroup)
        {
            var request = new RestRequest("/api/groups/{groupId}/history", Method.Get);
            request.AddParameter("groupId", idGroup, ParameterType.UrlSegment);
            dynamic response = _restClient.ExecuteAsync(request).Result;
            var historyDTOs = new List<HistoryDTO>();
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                var varItems = JsonConvert.DeserializeObject<List<dynamic>>(content);
                foreach (var obj in varItems)
                {
                    HistoryDTO userGroupDTO = new HistoryDTO
                    {
                        Id = obj.details.id,
                        nameGroup = obj.details.userGroup.group.name,
                        nameUser = obj.details.userGroup.user.name,
                        individualTotal = obj.details.userGroup.totalIndividual,
                        expense = obj.details.transaction.expense
                    };
                    historyDTOs.Add(userGroupDTO);
                }
            }
            return historyDTOs;
        }
    }
}
