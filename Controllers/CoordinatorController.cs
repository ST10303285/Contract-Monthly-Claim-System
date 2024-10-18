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
                claim.status = "Approved";  // Update the claim status to "Approved"
                _context.SaveChanges();
            }

            // Redirect to the dashboard or claim status tracker to show the updated status
            return RedirectToAction("Dashboard","Lecturer");  // or RedirectToAction("ClaimStatusTracker");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);

            if (claim != null)
            {
                claim.status = "Rejected";  // Update the claim status to "Rejected"
                _context.SaveChanges();
            }

            // Redirect to the dashboard or claim status tracker to show the updated status
            return RedirectToAction("Dashboard","Lecturer");  // or RedirectToAction("ClaimStatusTracker");
        }

    }
}
