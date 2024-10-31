using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using prjMVC_API_Marvel.Models;
using prjMVC_API_Marvel.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace prjMVC_API_Marvel.Controllers
{

    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        private string GetUserRole(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == "role" || c.Type == "roles" ||
                c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

                return roleClaim.Value;
            }
            return null;
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var token = await _apiService.LoginAsync(model);
                    HttpContext.Session.SetString("JWToken", token); //Store token in session

                    //Get userrole from the token
                    var userRole = GetUserRole(token);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username), //Add username claim
                        new Claim(ClaimTypes.Role, userRole), //Add role claim
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    //Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    if (userRole == "Admin")
                    {
                        return RedirectToAction("AdminDashboard", "admin");
                    }
                    else
                    {
                        return RedirectToAction("UserDashboard", "user");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Login failed: " + ex.Message);
                }
            }
            return View(model);

        }

        public async Task<IActionResult> Logout()
        {
            //Clear the session token
            HttpContext.Session.Remove("JWToken"); //Clear the token
            //Clear the authentication cookie
            Response.Cookies.Delete("JWToken"); //Remove the JWT cookie
            //Sign out the user if using cookie authenticatoin
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to the login page
            return RedirectToAction("Login", "Account");
        }
    }
}
