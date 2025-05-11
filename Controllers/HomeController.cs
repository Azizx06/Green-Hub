using System.Diagnostics;
using JeddahGreenHub.Data;
using JeddahGreenHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeddahGreenHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        // ---- CREATE THE COOKIE WHEN THE USER VISITS THE WEBSITE ...
        public IActionResult SetCookies(string cookieName, string cookieValue)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(15), // Cookie expires in 14 days
                HttpOnly = true,                    // Prevent JavaScript access to the cookies
                Secure = true,                      // Use Secure flage
                SameSite = SameSiteMode.Strict      // Prevent CSRF attacks
            };
            Response.Cookies.Append(cookieName, cookieValue, options);
            return Ok("Cookies has been set.");
        }





        // ---- SESSION FUNCTION to CREATE the SESSION ....
        public IActionResult SetSession(string key, string value)
        {
            // Set session value
            HttpContext.Session.SetString(key, value);
            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            // ---- FUNCTIONS TO CREATE SESSION KEYS AND VALUES ....
            // create a session id
            SetSession("id", Guid.NewGuid().ToString());

            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // User is logged in, get their username
                SetCookies("userName", User.Identity.Name);
                // create a session with username
                SetSession("username", User.Identity.Name);
            }
            else
            {
                // User is not logged in, set a cookie with "Guest"
                SetCookies("userName", "guest");
                // create a session with username
                SetSession("username", "guest");
            }
            // Get the broswer type
            SetCookies("broswerName", Request.Headers["User-Agent"].ToString());

            return View();
        }



        public IActionResult Resources()
        {
            return View();
        }
        public IActionResult Articles()
        {
            return View();
        }
        public IActionResult Events()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
