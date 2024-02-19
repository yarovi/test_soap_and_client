using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WS.Unit06.User.Web.Models;
using WSClient.ApplicationWS;
using WSUseExpenseManagerClient;

namespace Web.Mvc.Formulario.Gastos.Controllers
{
	public class GroupController : Controller
	{
		private SelectedUsers? selectedUsers { get; set; }
		List<GroupDTO> groupDTOs;
		List<UserDTO> userDTOs;

		public GroupController()
		{
			var client = new UserExpenseManagerServicesClient();
			groupDTOs = client.getAllCroupAsync().Result.
				Select(g => new GroupDTO { Id = g.Id, Name = g.Name }).ToList();
			var clientUser = new ApplicationServicesClient();
			userDTOs = clientUser.getUsersAsync().Result.Select(u => new UserDTO { Id = u.Id, Name = u.Name }).ToList();
		}

		public IActionResult indexGroup()
		{
			var client = new UserExpenseManagerServicesClient();
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
			var client = new UserExpenseManagerServicesClient();
			var response = await client.createGroupAsync(name);
			Debug.WriteLine("valor:" + response);
			TempData["idGroup"] = response;
			return RedirectToAction("indexGroup", "Group");
			//CreateGroup
		}
		public IActionResult deleteGroup(int id)
		{
			var client = new UserExpenseManagerServicesClient();
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
				groupDTOs,
				selectedUsers
			};

			return View("groupUser", CustomDtos);
		}

		[HttpPost]
		public IActionResult AddUserToGroup(int userId, int groupid)
		{
			selectedUsers = TempData["SelectedUsers"] as SelectedUsers ?? new SelectedUsers();
			if (selectedUsers == null)
				selectedUsers = new SelectedUsers();
			if (!selectedUsers.ContainsUser(userId))
			{
				var grupoName = groupDTOs.Find(f => f.Id == groupid).Name;
				var item = userDTOs.Find(u => u.Id == userId);
				selectedUsers.AddUser(item.Id, item.Name, grupoName);
				TempData["SelectedUserIds"] = selectedUsers.GetAllUsers().Select(u => u.Id).ToList();
				TempData["groupid"] = groupid;
			}
			var CustomDtos = new
			{
				userDTOs,
				groupDTOs,
				selectedUsers
			};
			return View("groupUser", CustomDtos);
		}
		[HttpPost]
		public IActionResult saveGroupUserForm()
		{
			var selectedUserIds = TempData["SelectedUserIds"] as int[];
			if (selectedUserIds != null)
			{
				var client = new UserExpenseManagerServicesClient();
				var groupId = TempData["groupid"];
				var response = client.associateUserWithGroupAsync(selectedUserIds.ToArray(), 1);
				Debug.WriteLine("valor:" + response);
			}
			var CustomDtos = new
			{
				userDTOs,
				groupDTOs,
				selectedUsers
			};
			return View("groupUser", CustomDtos);
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

			var client = new UserExpenseManagerServicesClient();
			List<GroupDTO> groupByUser = client.getAllGroupByUserAsync(4).Result.
                Select(g => new GroupDTO { Id = g.Id, Name = g.Name }).ToList();
            var CustomGroupByUser = new
            {
                groupByUser
            };
            return View("expenseGroup", CustomGroupByUser);
		}
		[HttpPost]
		public IActionResult saveTransaction(int idGroup,string description, float expenses)
		{
			//TODO: Aquie retorna el id pero esta con el path del uerystring mejorar
            var client = new UserExpenseManagerServicesClient();
			int result = client.createTransactionAsync(idGroup,4,description,expenses).Result;
            //return View("expenseGroup");
            return RedirectToAction("expenseGroup", "Group");
        }

        [HttpGet]
        public IActionResult getHistory(int idGroup)
        {
            var client = new UserExpenseManagerServicesClient();
			List<HistoryDTO> historyDTOs = client.getHistoryTransactionAsync(idGroup).Result.ToList();

            // Devolvemos los datos como JSON
            return Json(historyDTOs);
        }
    }
}
