﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using WS.Unit06.User.Web.Models;
using wsClientApplication=WSClient.ApplicationWS;
using wsClientExpense=WSUseExpenseManagerClient;
using WSUseExpenseManagerClient;
using WSClient.ApplicationWS;


namespace Web.Mvc.Formulario.Gastos.Controllers
{
	public class GroupController : Controller
	{
		private SelectedUsers? selectedUsers;
		GroupDTO[] groupDTOs;
		UserDTO[] userDTOs;

        private IConfiguration _configuration;
        private readonly UserExpenseManagerServicesClient _clientExpense;


        public GroupController(IConfiguration configuration)
        {
            _configuration = configuration;
            var globalenv = _configuration["EXPENSE_SERVICE_URL"] ?? _configuration["WebSettings:ExpenseServiceURL"];
            Console.WriteLine("Global api env:" + globalenv);

            var globalenv2 = _configuration["APP_SERVICE_URL"] ?? _configuration["WebSettings:AppServiceURL"];
            Console.WriteLine("Global api env:" + globalenv2);


            _clientExpense = new UserExpenseManagerServicesClient(new BasicHttpBinding(), new EndpointAddress(globalenv));

            //var client = new wsClientExpense.UserExpenseManagerServicesClient();
			
            groupDTOs = _clientExpense.getAllCroupAsync().Result;

            //var clientUser = new wsClientApplication.ApplicationServicesClient();
            var clientUser = new ApplicationServicesClient(new BasicHttpBinding(), new EndpointAddress(globalenv2));

            userDTOs = clientUser.getUsersAsync().Result;

		}
		public IActionResult indexGroup()
		{
			//var client = new wsClientExpense.UserExpenseManagerServicesClient();
			var groupDTOs = _clientExpense.getAllCroupAsync().Result;

			return View("createGroup", groupDTOs);
		}
		public IActionResult groupUser()
		{
			return View();
		}
		public IActionResult expenseGroup()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> createGroup(string name)
		{
			//var client = new wsClientExpense.UserExpenseManagerServicesClient();
			var response = await _clientExpense.createGroupAsync(name);
			Debug.WriteLine("valor:" + response);
			return RedirectToAction("indexGroup", "Group");
		}
		public IActionResult deleteGroup(int id)
		{
			//var client = new wsClientExpense.UserExpenseManagerServicesClient();
			var response = _clientExpense.deleteGroupAsync(id).Result;
			Debug.WriteLine("valor:" + response);
			TempData["idGroup"] = response;
			return RedirectToAction("indexGroup", "Group");
		}
		//----------------------------------------------------------------------
		//----------------------------GROUP-USER
		//----------------------------------------------------------------------
		public IActionResult indexGroupUser()
		{
			string username = HttpContext.Session.GetString("username");
			bool isOwner = BitConverter.ToBoolean(HttpContext.Session.Get("isOwner"));
			userDTOs = userDTOs.Where(user => user.Name != username).ToArray();
			var CustomDtos = new
			{
				userDTOs,
				groupDTOs,
				isOwner
			};
			return View("groupUser", CustomDtos);
		}

		[HttpPost]
		public IActionResult AddUserToGroup(int idUser)
		{
			selectedUsers = GetSelectedUsersFromSession();
			if (!selectedUsers.ContainsUser(idUser))
			{
				var item = userDTOs.FirstOrDefault(u => u.Id == idUser);
				selectedUsers.AddUser(item.Id, item.Name, "Por Asignar ..!");
				SaveSelectedUsersToSession(selectedUsers);
			}
			return Json(selectedUsers.GetAllUsers()); 
		}
		[HttpPost]
		public async Task<IActionResult> saveGroupUserForm(int idGroup)
		{

			string token = HttpContext.Session.GetString("token");
			//var client = new wsClientExpense.UserExpenseManagerServicesClient();
            var client = _clientExpense;

            using (var scope = new OperationContextScope(client.InnerChannel))
			{

				HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
				httpRequestProperty.Headers["token"] = token;
				OperationContext.Current.
					OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
					httpRequestProperty;

				selectedUsers = GetSelectedUsersFromSession();
				if (selectedUsers.GetAllUsers().Count > 0)
				{
					int[] ids = selectedUsers.GetAllUsers().Select(i => i.Id).ToArray();
					var response = await client.
						associateUserWithGroupAsync(ids,
						idGroup);
					if (response.Length > 0)
					{
						HttpContext.Session.Remove("SelectedUsers");
					}
					
					return Json(response);
				}
				return Json("No se han seleccionado usuarios para asociar al grupo");
			}
		}
		[HttpDelete]
		public IActionResult removeUserGroup(int id)
		{
			selectedUsers.RemoveUser(id);
			return View("groupUser", selectedUsers);
		}
		[HttpGet]
		public async Task<IActionResult> getAllGroupUserByUserId()
		{
			//var client = new wsClientExpense.UserExpenseManagerServicesClient();
            var client = _clientExpense;

            string token = HttpContext.Session.GetString("token");
			using (var scope = new OperationContextScope(client.InnerChannel))
			{

				HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
				httpRequestProperty.Headers["token"] = token;
				OperationContext.Current.
					OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
					httpRequestProperty;
				UserGroupDTO[] userGroupDTOsRegister = await client.getUserGroupsByUserIdAsync();
				if (userGroupDTOsRegister.Length == 0)
				{
					Console.WriteLine("No userGroupDTOsRegister ");
				}
				if (userGroupDTOsRegister != null)
				{
					Debug.Print("imprimiendo repuesta:" + userGroupDTOsRegister);
					foreach (var g in userGroupDTOsRegister)
					{
						UserDTO itemFound = userDTOs.FirstOrDefault(f => f.Id == int.Parse(g.fullNameUser));
						if (itemFound!=null) {
							g.userId = itemFound.Id;
							g.fullNameUser = itemFound.Name;
						}
					}
					var nameUser= HttpContext.Session.GetString("username");
					UserDTO ownUser = userDTOs.FirstOrDefault(f => f.Name == nameUser);
					if (ownUser != null)
					{
						int id= ownUser.Id;
						UserGroupDTO groupUser = userGroupDTOsRegister.FirstOrDefault(f => f.userId == id);
						if (groupUser != null)
						{
							groupUser.fullNameUser = ownUser.Name;
							groupUser.userId = ownUser.Id;
						}
						//groupUser.fullNameUser = ownUser.Name;
						//groupUser.userId = ownUser.Id;
					}
						return  Json(userGroupDTOsRegister);
				}
			}
			return RedirectToAction("accessRestrict", "Group");

		}
		//-----------------------------------------------------
		//------------------------------------ TRANSACTIONS
		//-----------------------------------------------------
		public async Task<IActionResult> indexTransaction()
		{
			//var client = new wsClientExpense.UserExpenseManagerServicesClient();
            var client = _clientExpense;

            string token = HttpContext.Session.GetString("token");
			using (var scope = new OperationContextScope(client.InnerChannel))
			{

				HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
				httpRequestProperty.Headers["token"] = token;
				OperationContext.Current.
					OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
					httpRequestProperty;

				GroupDTO[] groupDTOs = await client.getAllGroupByUserAsync();
				if (groupDTOs != null  	)
				{
					var CustomGroupByUser = new
					{
						groupDTOs
					};
					return View("expenseGroup", CustomGroupByUser);
				}
				else
				{
					return View("expenseGroup", null);
				}
			}

		}
		[HttpPost]
		public IActionResult saveTransaction(int idGroup, string description, float expenses)
		{
			//TODO: Aquie retorna el id pero esta con el path se debe mejorar
			string token = HttpContext.Session.GetString("token");
			//var client = new wsClientExpense.UserExpenseManagerServicesClient();
            var client = _clientExpense;

            using (var scope = new OperationContextScope(client.InnerChannel))
			{

				HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
				httpRequestProperty.Headers["token"] = token;
				OperationContext.Current.
					OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
					httpRequestProperty;

				int result = client.createTransactionAsync(idGroup, description, expenses).Result;

				if (result != null)
				{
					return Json(result);
				}
				else
				{
					return Json("No se puedo realizar la solicitud de creación.");
				}
			}
		}

		[HttpGet]
		public async Task<IActionResult> getHistory(int idGroup)
		{
			//var client = new wsClientExpense.UserExpenseManagerServicesClient();
            var client = _clientExpense;

            HistoryDTO[] historyDTOs =await client.getHistoryTransactionAsync(idGroup);
			foreach (var h in historyDTOs)
			{
				UserDTO itemFound = userDTOs.FirstOrDefault(f => f.Id == int.Parse(h.nameUser));
				if (itemFound != null)
					h.nameUser = itemFound.Name;
			}
			var nameUser = HttpContext.Session.GetString("username");
			UserDTO ownUser = userDTOs.FirstOrDefault(f => f.Name == nameUser);

			return Json(historyDTOs);
		}

		private SelectedUsers GetSelectedUsersFromSession()
		{
			var data = HttpContext.Session.GetString("SelectedUsers");
			if (data != null)
			{
				try
				{
					SelectedUsers result = JsonConvert.DeserializeObject<SelectedUsers>(data);
					return result;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error al deserializar los datos de la sesión: " + ex.Message);
					throw;
				}
			}
			return new SelectedUsers();
		}

		private void SaveSelectedUsersToSession(SelectedUsers selectedUsers)
		{
			HttpContext.Session.SetString("SelectedUsers", JsonConvert.SerializeObject(selectedUsers));
		}

        [HttpGet]
        public IActionResult accessRestrict()
        {
            return View();
        }
    }
}
