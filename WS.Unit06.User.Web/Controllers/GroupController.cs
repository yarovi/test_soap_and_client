using Microsoft.AspNetCore.Mvc;

namespace Web.Mvc.Formulario.Gastos.Controllers
{
    public class GroupController :Controller
    {
        public IActionResult createGroup()
        {
            return View();
        }

        public IActionResult groupUser()
        {
            return View();
        }
        public IActionResult expenseGroup() {
        return View();
        }
    }
}
