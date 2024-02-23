using Newtonsoft.Json;
using RestSharp;
using System.Net.Mime;
using System.ServiceModel.Channels;
using System.ServiceModel;
using WS.Unit06.User.Application.util;
using WS.Unit06.User.Application.Model;

namespace WS.Unit06.User.Application.Services.impl
{
    public class UserExpenseManagerServices : IUserExpenseManagerServices
    {
        private RestClient _restClient;
		public HttpContext httpContext { get; set; }
        int userId { get; set; }
        string nameFull { get; set; }
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

        public GroupDTO[] getAllCroup()
        {
            var request = new RestRequest("/api/groups", Method.Get);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            dynamic response = _restClient.ExecuteAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
				var settings = new JsonSerializerSettings();
				settings.Converters.Add(new CustomGroupDTOConverter());
				var content = response.Content;
                var groupDTOs = JsonConvert.DeserializeObject<GroupDTO[]>(content, settings);
                return groupDTOs;
            }
                return null;
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
					}
				}
			}
			return codesList.ToArray();
		}

		public UserGroupDTO[] getUserGroups()
		{
            if (validateToken())
            {
				var request = new RestRequest("/api/user-groups", Method.Get);
				dynamic response = _restClient.ExecuteAsync(request).Result;
				if (response.IsSuccessStatusCode)
				{
					var content = response.Content;
					var varItems = JsonConvert.DeserializeObject<UserGroupDTO[]>(content);
					var userGroups = new UserGroupDTO[varItems.Count];
					for (int i = 0; i < varItems.Count; i++)
					{
						dynamic obj = varItems[i];
						UserGroupDTO userGroupDTO = new UserGroupDTO
						{
							Id = obj.id,
							NameGroup = obj.group.name,
							NameUser = obj.user.name,
							totalAmmount = obj.user.totalAmount
						};
						userGroups[i] = userGroupDTO;
					}
					/*foreach (var obj in varItems)
					{
						UserGroupDTO userGroupDTO = new UserGroupDTO
						{
							Id = obj.id,
							NameGroup = obj.group.name,
							NameUser = obj.user.name,
							totalAmmount = obj.user.totalAmount
						};
						userGroups.Add(userGroupDTO);
					}*/
					return userGroups;
				}
                
			}
            return null;
			
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
				lastSlashIndex = headerLocation.LastIndexOf('?') ;
                if(lastSlashIndex != -1)
                {
                    int secondSlash= headerLocation.LastIndexOf('/') + 1;
					string codeLocation = headerLocation.Substring(secondSlash, (lastSlashIndex-secondSlash));
					if (int.TryParse(codeLocation, out int number))
						return number;
				}
			}
            return 0;
		}

		public GroupDTO[] getAllGroupByUser()
		{
			var userDTOs = new List<GroupDTO>();
			if (validateToken())
            {
				var request = new RestRequest("/api/user-groups/{userId}/users", Method.Get);
				request.AddParameter("userId", userId, ParameterType.UrlSegment);
				dynamic response = _restClient.ExecuteAsync(request).Result;
				
				if (response.IsSuccessStatusCode)
				{
					var content = response.Content;
					var groupDataList = JsonConvert.DeserializeObject<List<dynamic>>(content);
					

					foreach (var groupData in groupDataList)
					{
						var groupDTO = new GroupDTO
						{
							Id= groupData.idUserGroup,
                            Name=groupData.groupCategory.name,
						};

						userDTOs.Add(groupDTO);
					}
					return userDTOs.ToArray();
				}
			}
			return userDTOs.ToArray();
		}

        public int createTransaction(int idGroup,string description,float expense)
        {
            //TODO SE verificar bien esto 
            if (validateToken()) {
				var request = new RestRequest("/api/transactions", Method.Post);
				request.AddHeader("Accept", MediaTypeNames.Application.Json);
				request.AddParameter("idGroup", idGroup, ParameterType.QueryString);
				request.AddParameter("idUser", userId, ParameterType.QueryString);
				request.AddJsonBody(new { description = description, expense = expense });
				dynamic response = _restClient.ExecuteAsync(request).Result;
				if (response != null && response.Headers != null)
				{
					foreach (var header in response.Headers)
					{
						if (header.Name.Equals("Location", StringComparison.OrdinalIgnoreCase))
						{
							string locationHeaderValue = header.Value;
							int code = getLocationUrl(locationHeaderValue);
							return code;
						}
					}
				}
			}
				return 0;
        }

        public TransactionDTO[] getAllTransaction()
        {
            var request = new RestRequest("/api/transactions", Method.Get);
            request.AddHeader("Accept", MediaTypeNames.Application.Json);
            dynamic response = _restClient.ExecuteAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                var transactionDTOs = JsonConvert.DeserializeObject<TransactionDTO[]>(content);
                return transactionDTOs.ToArray();
            }
            else
            {
                return null;
            }
        }

        public HistoryDTO[] getHistoryTransaction(int idGroup)
        {
            var request = new RestRequest("/api/transactions/{groupId}/history", Method.Get);
            request.AddParameter("groupId", idGroup, ParameterType.UrlSegment);
            dynamic response = _restClient.ExecuteAsync(request).Result;
			List<HistoryDTO> mappedHistoryList = new List<HistoryDTO>();
			if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
				List<dynamic> historyList = JsonConvert.DeserializeObject<List<dynamic>>(content);

				foreach (var history in historyList)
				{
					HistoryDTO historyDTO = new HistoryDTO
					{
						idHistory = history.idHistory,
						nameGroup = history.details.userGroup.groupCategory.name,
						nameUser = history.details.userGroup.userId.ToString(),
						individualTotal = history.total.ToString(),
						expense = history.details.transaction.expense.ToString()
					};

					mappedHistoryList.Add(historyDTO);
				}
            }
            return mappedHistoryList.ToArray();
        }

        private bool validateToken()
        {
            var clientAuth = new WSAuthClientSOAP.AuthServicesClient();
			using (var scope = new OperationContextScope(clientAuth.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers["token"] = httpContext.Request.Headers["token"];
                OperationContext.Current.
                    OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                    httpRequestProperty;
                var responseGroupByUser = clientAuth.validateAsync().Result;
                if (responseGroupByUser != null && responseGroupByUser.code == 200)
                {
                    userId = Int32.Parse( responseGroupByUser.Claims.Type);
					nameFull = responseGroupByUser.Claims.Value;
					return true;
				}
                   
                else
                    return false;
            }
        }
	}

    
}
