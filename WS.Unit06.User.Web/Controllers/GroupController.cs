using Microsoft.AspNetCore.Mvc;
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

		public GroupController()
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			groupDTOs = client.getAllCroupAsync().Result;
			var clientUser = new wsClientApplication.ApplicationServicesClient();
			userDTOs = clientUser.getUsersAsync().Result;
		}

		public IActionResult indexGroup()
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			var groupDTOs = client.getAllCroupAsync().Result;

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
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			var response = await client.createGroupAsync(name);
			Debug.WriteLine("valor:" + response);
			TempData["idGroup"] = response;
			return RedirectToAction("indexGroup", "Group");
		}
		public IActionResult deleteGroup(int id)
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			var response = client.deleteGroupAsync(id).Result;
			Debug.WriteLine("valor:" + response);
			TempData["idGroup"] = response;
			return RedirectToAction("indexGroup", "Group");
		}
		//----------------------------------------------------------------------
		//----------------------------GROUP-USER
		//----------------------------------------------------------------------
		public IActionResult indexGroupUser()
		{
			var CustomDtos = new
			{
				userDTOs,
				groupDTOs
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
			selectedUsers = GetSelectedUsersFromSession();
			if (selectedUsers != null)
			{
				var client = new wsClientExpense.UserExpenseManagerServicesClient();
				int[] ids =  selectedUsers.GetAllUsers().Select(i => i.Id).ToArray();
				var response =await client.
					associateUserWithGroupAsync(ids, 
					idGroup);
				HttpContext.Session.Remove("SelectedUsers");
				return Json(response);
			}
			return null;
		}
		[HttpDelete]
		public IActionResult removeUserGroup(int id)
		{
			selectedUsers.RemoveUser(id);
			return View("groupUser", selectedUsers);
		}
		//-----------------------------------------------------
		//------------------------------------ TRANSACTIONS
		//-----------------------------------------------------
		public async Task<IActionResult> indexTransaction()
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			string token = HttpContext.Session.GetString("token");
			using (var scope = new OperationContextScope(client.InnerChannel))
			{

				HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
				httpRequestProperty.Headers["token"] = token;
				OperationContext.Current.
					OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
					httpRequestProperty;

				GroupDTO[] groupDTOs = client.getAllGroupByUserAsync().Result;
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
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			
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
					return RedirectToAction("indexTransaction", "Group");
				}
			}
		}

		[HttpGet]
		public IActionResult getHistory(int idGroup)
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			HistoryDTO[] historyDTOs = client.getHistoryTransactionAsync(idGroup).Result;
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
	}
}
