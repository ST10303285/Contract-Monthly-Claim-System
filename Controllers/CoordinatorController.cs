using Contract_Monthly_Claim_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contract_Monthly_Claim_System.Controllers
{
    public class CoordinatorController : Controller
    {

        private readonly ApplicationDbContext _context;

        // Constructor to inject the ApplicationDbContext
        public CoordinatorController(ApplicationDbContext context)
        {
            _context = context;  // This sets up _context for use in the class
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult LecturerDashboard()
        {
            var claims = _context.Claims.ToList();  // Fetch all claims from the database
            return View(claims);  // Pass claims to the view
        }

    }
}
