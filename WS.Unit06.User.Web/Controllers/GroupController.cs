using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
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
		public IActionResult AddUserToGroup(int userId  )
		{
			selectedUsers = TempData["SelectedUsers"] as SelectedUsers ?? new SelectedUsers();
			if (selectedUsers == null)
				selectedUsers = new SelectedUsers();
			if (!selectedUsers.ContainsUser(userId))
			{
				var item = userDTOs.Find(u => u.Id == userId);
				selectedUsers.AddUser(item.Id, item.Name);
				TempData["SelectedUserIds"] = selectedUsers.GetAllUsers().Select(u => u.Id).ToList();
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
		public IActionResult saveGroupUser(int groupId)
		{
			var selectedUserIds = TempData["SelectedUserIds"] as int[];
			if (selectedUserIds != null)
			{
				var client = new UserExpenseManagerServicesClient();
				var response =  client.associateUserWithGroupAsync(selectedUserIds.ToArray(), groupId);
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
	}
}
