using Microsoft.AspNetCore.Mvc;

namespace Muzej.UI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string role)
        {
            // Set the session role based on the button clicked
            HttpContext.Session.SetString("Role", role);

            // For demonstration purposes, redirect to a welcome page or dashboard
            return RedirectToAction("Index", "Home");
        }
    }
}
