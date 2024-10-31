using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace prjMVC_API_Marvel.Controllers
{
    public class UserController : Controller
    {
        [Authorize(Roles = "User")] //Only allow users with user roles

        public IActionResult UserDashboard()
        {
            return View();
        }
    }
}
