using Microsoft.AspNetCore.Mvc;
using prjMVC_API_Marvel.Models;
using prjMVC_API_Marvel.Services;

using Microsoft.AspNetCore.Authorization;

namespace prjMVC_API_Marvel.Controllers
{
    [Authorize(Roles = "Admin")] //This will enforce that all admin
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var avengers = await _apiService.GetAvengersAsync();
            return View(avengers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TblAvenger avenger)
        {
            await _apiService.CreateAvengersAsync(avenger);
            return RedirectToAction("Index");
        }
        //Ensure this is a POST request
        [HttpPost]
        public async Task<IActionResult> Delete(string username)
        {
            try
            {
                await _apiService.DeleteAvengersAsync(username);
                return RedirectToAction("Index");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                ModelState.AddModelError(string.Empty, "The avenger cannot be deleted because there are contacts associated with this user.");
                var avengers = await _apiService.GetAvengersAsync();
                return View("Index", avengers); //Return to the Index view with the current list
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while trying to delete the avenger.");
                var avengers = await _apiService.GetAvengersAsync();
                return View("Index", avengers);//Return to the index view with the current list
            }
        }
    }
    }
