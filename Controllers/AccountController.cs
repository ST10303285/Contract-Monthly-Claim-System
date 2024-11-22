using System.Text;
using Contract_Monthly_Claim_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Contract_Monthly_Claim_System.Models;


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
