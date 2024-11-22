using Contract_Monthly_Claim_System.Data;
using Contract_Monthly_Claim_System.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;


namespace Contract_Monthly_Claim_System.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
		{
			return View();
		}
        public IActionResult Lecturer()
        {
            var lecturers = _context.Lecturers.ToList();
            return View(lecturers);
        }
 
        public IActionResult Dashboard()
        {
            var approvedClaims = _context.Claims.Where(c => c.status == "Approved").ToList();
            return View(approvedClaims); // Render a view showing approved claims
        }

       
      
   
         ////////////////////////////////////////////////////////////////////methods for managing lecturer data/////////////////////////////////////////////////////////////////////////////////////////////
        
       
        public IActionResult CreateLecturer()
        {
            var lecturer = new Lecturer(); // Initialize a new lecturer object
            return View(lecturer);
        }

        [HttpPost]
        public IActionResult CreateLecturer(Lecturer lecturer) // Create a new lecturer
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Add(lecturer); //Add the entered detaisl for lecturer to the database
                _context.SaveChanges();
                return RedirectToAction("Lecturer"); // Redirect to the lecturer page
            }
            return View(lecturer); //if the model state is invalid, it shoudl return the lecturer view
        }


        // Edit lecturer details
        public IActionResult EditLecturer(int id)
        {
            var lecturer = _context.Lecturers.Find(id); // Find the lecturer that you want to edit by the lecturer id in the database 
            if (lecturer == null) // If the lecturer is not found
            {
                return NotFound();  // Return 404 if lecturer not found
            }
            return View(lecturer); //if the lecturer id is found in the databasse, return to the lecturer view
        }

        [HttpPost]
        public IActionResult EditLecturer(Lecturer lecturer) // Edit lecturer details
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Update(lecturer); // Update the lecturer details in the database
                _context.SaveChanges();
                return RedirectToAction("Lecturer"); // Redirect to the lecturer page
            }
            return View(lecturer); //if the model state is invalid, it shoudl return the lecturer view
        }

        // Delete lecturer
        public IActionResult Delete(int id)
        {
            var lecturer = _context.Lecturers.Find(id); // Find the lecturer that you want to delete by the lecturer id in the database
            if (lecturer == null) return NotFound(); // If the lecturer is not found, return 404
            _context.Lecturers.Remove(lecturer); // Remove the lecturer from the database
            _context.SaveChanges();
            return RedirectToAction("Lecturer"); // Redirect to the lecturer page
        }
        ////////////////////////////////////////////////////////////////////end of methods for managing lecturer data/////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////methods for generating invoice for hr/////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult GenerateInvoice(int claimId)
        {
            // Fetch the specific claim by ID
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim == null)
            {
                return NotFound("Claim not found");
            }

            // Generate the PDF invoice
            using var memoryStream = new MemoryStream();
            var document = new iTextSharp.text.Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Add Title
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            document.Add(new Paragraph("Invoice", titleFont) { Alignment = Element.ALIGN_CENTER });
            document.Add(new Paragraph(" ")); // Empty line for spacing

            // Create a table with two columns
            PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
            table.SpacingBefore = 20f;

            // Add headers with a background color
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            table.AddCell(new PdfPCell(new Phrase("Field", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Value", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });

            // Add claim details
            var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            AddTableRow(table, "Claim ID", claim.ClaimId.ToString(), regularFont);
            AddTableRow(table, "Lecturer Name", claim.lecturerName, regularFont);
            AddTableRow(table, "Hours Worked", claim.hoursWorked.ToString(), regularFont);
            AddTableRow(table, "Hourly Rate", $"{claim.hourlyRate:C}", regularFont);
            AddTableRow(table, "Claim Amount", $"{claim.claimAmount:C}", regularFont);
            AddTableRow(table, "Submission Date", claim.submissionDate.ToString("yyyy-MM-dd"), regularFont);

            // Add the table to the document
            document.Add(table);

            // Add a footer
            document.Add(new Paragraph("Thank you for your submission!", regularFont) { Alignment = Element.ALIGN_CENTER });

            document.Close();

            // Return the PDF as a file download
            return File(memoryStream.ToArray(), "application/pdf", $"Invoice_Claim_{claim.ClaimId}.pdf");
        }

        private void AddTableRow(PdfPTable table, string field, string value, Font font)
        {
            table.AddCell(new PdfPCell(new Phrase(field, font)));
            table.AddCell(new PdfPCell(new Phrase(value, font)));
        }

        ////////////////////////////////////////////////////////////////////methods for managing lecturer data/////////////////////////////////////////////////////////////////////////////////////////////

    }
}
