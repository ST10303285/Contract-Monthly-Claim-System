using System.Reflection.Metadata;
using Contract_Monthly_Claim_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;

namespace Contract_Monthly_Claim_System.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var approvedClaims = _context.Claims.Where(c => c.status == "Approved").ToList();
            return View(approvedClaims); // Render a view showing approved claims
        }

        [HttpGet]
        public IActionResult GenerateInvoice(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim == null)
            {
                return NotFound("Claim not found");
            }

            using var memoryStream = new MemoryStream();
            var document = new iTextSharp.text.Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Add content to the invoice
            document.Add(new Paragraph("Invoice"));
            document.Add(new Paragraph($"Lecturer: {claim.lecturerName}"));
            document.Add(new Paragraph($"Hours Worked: {claim.hoursWorked}"));
            document.Add(new Paragraph($"Hourly Rate: {claim.hourlyRate:C}"));
            document.Add(new Paragraph($"Claim Amount: {claim.claimAmount:C}"));
            document.Add(new Paragraph($"Submission Date: {claim.submissionDate:yyyy-MM-dd}"));

            document.Close();

            return File(memoryStream.ToArray(), "application/pdf", "ApprovedClaims.pdf");
        }
    }
}
