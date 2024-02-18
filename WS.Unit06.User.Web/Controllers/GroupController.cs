using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Xml.Linq;
using WS.Unit06.User.Application.Model;
using WS.Unit06.User.Application.Services.impl;
using WSUseExpenseManagerClient;

namespace Web.Mvc.Formulario.Gastos.Controllers
{
    public class GroupController :Controller
    {
        public IActionResult indexGroup()
        {
            var client = new UserExpenseManagerServicesClient();
            var  groupDTOs = client.getAllCroupAsync().Result;

            return View("createGroup",groupDTOs);
        }

        public IActionResult groupUser()
        {
            return View();
        }
        public IActionResult expenseGroup() {
        return View();
        }
        [HttpPost]
        public async Task<IActionResult> createGroup(string name)
        {
            var client =new UserExpenseManagerServicesClient();
            var response = await client.createGroupAsync(name);
            Debug.WriteLine("valor:"+response);
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
    }
}
