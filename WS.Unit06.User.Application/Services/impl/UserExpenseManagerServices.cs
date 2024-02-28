using Newtonsoft.Json;
using RestSharp;
using System.Net.Mime;
using System.ServiceModel.Channels;
using System.ServiceModel;
using WS.Unit06.User.Application.util;
using WS.Unit06.User.Application.Model;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace WS.Unit06.User.Application.Services.impl
{
	public class UserExpenseManagerServices : IUserExpenseManagerServices
	{
		private RestClient _restClient;
		public HttpContext httpContext { get; set; }
		int userId { get; set; }
		string nameFull { get; set; }
        private readonly IConfiguration _configuration;
        public UserExpenseManagerServices(IConfiguration configuration)
        {
            _configuration = configuration;
            var globalenv = _configuration["EXPENSE_SERVICE_URL"] ?? _configuration["WebSettings:ExpenseServiceURL"];
			Console.WriteLine("Expense URL constructor: " + globalenv);

            GlobalSetting.ApiUrl = globalenv;


            Console.WriteLine("URL constructor: "+GlobalSetting.ApiUrl);
            /*var options = new RestClientOptions
            (_configuration.GetValue<string>("WebSettings:ExpenseServiceURL"));
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
			dynamic response = _restClient.ExecuteAsync(request).Result;
			int codeReturn = 0;
			if (response != null)
			{
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
			List<GroupDTO> userGroups = new List<GroupDTO>();
			if (response.IsSuccessStatusCode)
			{
				var content = response.Content;
				List<dynamic> gropus = JsonConvert.DeserializeObject<List<dynamic>>(content);
				foreach (var g in gropus)
				{
					GroupDTO groupDTO = new GroupDTO
					{
						Id = g.idGroupCategory,
						Name = g.name,
					};
					userGroups.Add(groupDTO);
				}
			}
			return userGroups.ToArray();
		}

		public int[] associateUserWithGroup(int[] ids, int groupId)
		{
			List<int> codesList = new List<int>();
			if (validateToken())
			{
				var request = new RestRequest("/api/user-groups/{groupId}/users", Method.Post);
				request.AddHeader("Accept", MediaTypeNames.Application.Json);
				request.AddParameter("groupId", groupId, ParameterType.UrlSegment);
				List<int> idsLocal = ids.ToList();
				idsLocal.Add(userId);
				request.AddJsonBody(idsLocal);
				dynamic response = _restClient.ExecuteAsync(request).Result;
				Console.WriteLine("RESPONSE: "+response);

				if (!response.IsSuccessStatusCode)
				{
					
					Console.WriteLine($"Error: {response.StatusCode}");
					return Array.Empty<int>(); 
				}

				if (response != null && response.Content != null)
				{
					List<string> urls = JsonConvert.DeserializeObject<List<string>>(response.Content);
					foreach (var pathUrl in urls)
					{
						int code = getLocationUrl(pathUrl);
						codesList.Add(code);
					}
					var requestUpdate = new RestRequest("/api/user-groups/{groupId}/ownermaster/{newMasterId}", Method.Put);
					requestUpdate.AddParameter("groupId", groupId, ParameterType.UrlSegment);
					requestUpdate.AddParameter("newMasterId", userId, ParameterType.UrlSegment);
					var responseUpdate = _restClient.ExecuteAsync(requestUpdate).Result;
				}
			}

			return codesList.ToArray();
		}
		public UserGroupDTO[] getUserGroupsByUserId()
		{
			List<UserGroupDTO> userGroups = new List<UserGroupDTO>();
			if (validateToken())
			{
				var request = new RestRequest("/api/user-groups", Method.Get);
				request.AddParameter("idUser", userId, ParameterType.UrlSegment);
				dynamic response = _restClient.ExecuteAsync(request).Result;
				if (response.IsSuccessStatusCode)
				{
					var content = response.Content;
					List<dynamic> groups = JsonConvert.DeserializeObject<List<dynamic>>(content);
					foreach (var g in groups)
					{
						UserGroupDTO userGroupDTO = new UserGroupDTO
						{
							Id = g.idUserGroup,
							NameGroup = g.groupCategory.name,
							fullNameUser = (g.userId),

						};
						userGroups.Add(userGroupDTO);
					}
				}
			}
			return userGroups.ToArray();
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
				lastSlashIndex = headerLocation.LastIndexOf('?');
				if (lastSlashIndex != -1)
				{
					int secondSlash = headerLocation.LastIndexOf('/') + 1;
					string codeLocation = headerLocation.Substring(secondSlash, (lastSlashIndex - secondSlash));
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
							Id = groupData.idUserGroup,
							Name = groupData.groupCategory.name,
						};

						userDTOs.Add(groupDTO);
					}
					return userDTOs.ToArray();
				}
			}
			return userDTOs.ToArray();
		}

		public int createTransaction(int idGroup, string description, float expense)
		{
			//TODO se debe verificar con mas pruebas 
			if (validateToken())
			{
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
		/*
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
		}*/

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


		public bool isOwner()
		{
			bool result = false;
			if (validateToken())
			{
				var request = new RestRequest("/api/user-groups/owner/{userId}", Method.Get);
				request.AddHeader("Accept", MediaTypeNames.Application.Json);
				request.AddParameter("userId", userId, ParameterType.UrlSegment);
				dynamic response = _restClient.ExecuteAsync(request).Result;
				if (response.IsSuccessStatusCode)
				{
					var content = response.Content;
					var responseObject = JsonConvert.DeserializeObject(content);
					result = responseObject.owner;
				}
			}
			return result;
		}

		private bool validateToken()
		{
            //var clientAuth = new WSAuthClientSOAP.AuthServicesClient(); 
            var globalenv = _configuration["AUTH_SERVICE_URL"] ?? _configuration.GetValue<string>("WebSettings:AuthServiceURL");
            Console.WriteLine("Auth URL validateToken(): " + globalenv);
			 
			var clientAuth = new WSAuthClientSOAP.AuthServicesClient(new BasicHttpBinding(), new EndpointAddress(globalenv));
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
					userId = Int32.Parse(responseGroupByUser.Claims.Type);
					nameFull = responseGroupByUser.Claims.Value;
					return true;
				}

				else
					return false;
			}
		}
		


	}


}
