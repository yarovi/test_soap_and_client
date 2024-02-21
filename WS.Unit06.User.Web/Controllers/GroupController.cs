using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WS.Unit06.User.Application.Model;
using WS.Unit06.User.Web.Models;
using WS.Unit06.User.Web.Util;
using wsClientApplication=WSClient.ApplicationWS;
using wsClientExpense=WSUseExpenseManagerClient;


namespace Web.Mvc.Formulario.Gastos.Controllers
{
	public class GroupController : Controller
	{
		private SelectedUsers? selectedUsers;
		List<GroupDTO> groupDTOs;
		List<UserDTO> userDTOs;

		public GroupController()
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			groupDTOs = client.getAllCroupAsync().Result.
				Select(g => new GroupDTO { Id = g.Id, Name = g.Name }).ToList();
			var clientUser = new wsClientApplication.ApplicationServicesClient();
			userDTOs = clientUser.getUsersAsync().Result.Select(u => new UserDTO { Id = u.Id, Name = u.Name }).ToList();
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
			//CreateGroup
		}
		public IActionResult deleteGroup(int id)
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			var response = client.deleteGroupAsync(id).Result;
			Debug.WriteLine("valor:" + response);
			TempData["idGroup"] = response;
			return RedirectToAction("indexGroup", "Group");
		}

		//----------------------------GROUP-USER
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
				var item = userDTOs.Find(u => u.Id == idUser);
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
				
				return Json(response);
			}
			/*var CustomDtos = new
			{
				userDTOs,
				groupDTOs,
			};*/
			return null;
		}
		[HttpDelete]
		public IActionResult removeUserGroup(int id)
		{
			selectedUsers.RemoveUser(id);
			return View("groupUser", selectedUsers);
		}
		//------------------------------------ TRANSACTIONS
		public IActionResult indexTransaction()
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			List<GroupDTO> groupByUser = client.getAllGroupByUserAsync(4).Result.
				Select(g => new GroupDTO { Id = g.Id, Name = g.Name }).ToList();
			var CustomGroupByUser = new
			{
				groupByUser
			};
			return View("expenseGroup", CustomGroupByUser);
		}
		[HttpPost]
		public IActionResult saveTransaction(int idGroup, string description, float expenses)
		{
			//TODO: Aquie retorna el id pero esta con el path se debe mejorar
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			int result = client.createTransactionAsync(idGroup, 4, description, expenses).Result;
			return RedirectToAction("expenseGroup", "Group");
		}

		[HttpGet]
		public IActionResult getHistory(int idGroup)
		{
			var client = new wsClientExpense.UserExpenseManagerServicesClient();
			List<wsClientExpense.HistoryDTO> historyDTOs = client.getHistoryTransactionAsync(idGroup).Result.ToList();
			return Json(null);
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
