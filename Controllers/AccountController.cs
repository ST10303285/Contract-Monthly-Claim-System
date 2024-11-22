using System.Text;
using Contract_Monthly_Claim_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Contract_Monthly_Claim_System.Models;

//Wadiha Boat
//ST10303285
// Group 2

// References:
// https://learn.microsoft.com/en-us/ef/core/get-started/overview/install
// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0051?f1url=%3FappId%3Droslyn%26k%3Dk(CS0051)
//https://www.c-sharpcorner.com/article/installing-entity-framework-core/
//https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16
//https://learn.microsoft.com/en-us/answers/questions/1295037/ssms-cannot-connect-to-(localdb)mssqllocaldb-2019
//https://stackoverflow.com/questions/72353705/visual-studio-2022-localdb-localdb-mssqllocaldb-asp-net-core 


namespace Contract_Monthly_Claim_System.Controllers
{
   
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;  // This sets up _context for use in the class
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        // Handle Login Form Submission
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string role)
        {
            var hashedPassword = HashPassword(password);

            // Look for user in the database
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.PasswordHash == hashedPassword && u.Role == role);

            if (user != null)
            {
                // Sign in the user using Cookie Authentication
                var claims = new List<System.Security.Claims.Claim>
                {
                 new System.Security.Claims.Claim(ClaimTypes.Name, user.Username),
                 new System.Security.Claims.Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Redirect to the respective dashboard
                return RedirectToRoleDashboard(user.Role);
            }
            else
            {
                // Show error message if login fails
                ViewBag.Error = "Invalid username or password.";
                return View();
            }
        }

        // Show Register Form
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handle Register Form Submission
        [HttpPost]
        public IActionResult Register(string username, string password, string role)
        {
            var hashedPassword = HashPassword(password);

            // Check if the user already exists
            if (_context.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username already exists!";
                return View();
            }

            // Add user to database
            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Role = role
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Redirect to login page after successful registration
            return RedirectToAction("Login");
        }


        
        private IActionResult RedirectToRoleDashboard(string role) //method to redirect user to respective dashboard based on their role
        {
            if (role == "Lecturer")
                return RedirectToAction("Dashboard", "Lecturer");
            else if (role == "Coordinator")
                return RedirectToAction("Dashboard", "Coordinator");
            else
                return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password) //method to hash the password
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Log out user
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear session data
            return RedirectToAction("Login");
        }




    }
}
