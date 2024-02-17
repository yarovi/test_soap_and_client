using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
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
        public IActionResult createGroup(string name)
        {
            var client =new UserExpenseManagerServicesClient();
            var response = client.createGroupAsync(name).Status;
            Debug.WriteLine("valor:"+response);
            return View();
            //CreateGroup
        }
        [HttpPost]
        public IActionResult deleteGroup(int id)
        {
			Debug.WriteLine("eliminando:" + id);
			return RedirectToAction("indexGroup", "Group");
		}
    }
}
