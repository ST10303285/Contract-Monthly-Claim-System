using Contract_Monthly_Claim_System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [Authorize(Policy = "CoordinatorOnly")]
    public class CoordinatorController : Controller
    {

        private readonly ApplicationDbContext _context;

        // Constructor to inject the ApplicationDbContext
        public CoordinatorController(ApplicationDbContext context)
        {
            _context = context;  // This sets up _context for use in the class
        }
        public IActionResult Index() // Index action
        {
            return View();
        }

        public IActionResult Dashboard()
        {

            var pendingClaims = _context.Claims.Where(c => c.status == "Pending").ToList();
            return View(pendingClaims); // Pass the pending claims to the view
        }
        
        public IActionResult LecturerDashboard()
        {
            var claims = _context.Claims.ToList();  // Fetch all claims from the database
            return View(claims);  // Pass claims to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);

            if (claim != null)
            {
                // Automated verification logic
                if (claim.hoursWorked > 160)  // Example check: max allowable hours (adjust as needed)
                {
                    claim.status = "Rejected";
                    Console.WriteLine("Claim rejected due to exceeding hours.");// Reject the claim if hours exceed 160
                }
                else if (claim.hourlyRate > 50)  // Example check: max hourly rate (adjust as needed)
                {
                    claim.status = "Rejected";  // Reject the claim if hourly rate exceeds $50
                }
                else
                {
                    // If all checks pass, approve the claim
                    claim.status = "Approved";
                }

                _context.SaveChanges();
            }

            // Redirect to the dashboard or claim status tracker
            return RedirectToAction("Dashboard", "Coordinator");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);

            if (claim != null)
            {
                
                claim.status = "Rejected";  
                _context.SaveChanges();
            }

            // Redirect to the dashboard or claim status tracker
            return RedirectToAction("Dashboard", "Coordinator");
        }

    }
}
