using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace prjMVC_API_Marvel.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")] //Only allow users with the Admin roles
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
