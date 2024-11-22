using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Contract_Monthly_Claim_System.Models;
using Contract_Monthly_Claim_System.Data;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Policy = "LecturerOnly")]
    public class LecturerController : Controller //Lecturer Controller for the lecturer to submit claims
    {
        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Dashboard()
        {
            var userId = User.Identity.Name;  // Get the logged-in user's ID (or username)
            var lecturerClaims = _context.Claims
                                         .Where(c => c.UserId == userId)  // Filter claims by user ID
                                         .ToList();  // Get the list of claims for the current user

            
            return View(lecturerClaims); // Pass the filtered claims to the view
        }

        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult SubmitClaim(ClaimViewModel model) // method to create a new claim
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.Name;
                // Calculate the claim amount
                double claimAmount = model.hoursWorked * model.hourlyRate; // Calculate the claim amount

                string status;

                // Automated rejection logic
                if (model.hoursWorked > 100) //if the hours worked exceed 100, automatically reject claim
                {
                    status = "Rejected";
                    Console.WriteLine("Claim rejected: hours worked exceeded the limit.");
                }
                else if (model.hourlyRate > 50) // if the hourly rate exceeds 50, automatically reject claim
                {
                    status = "Rejected";
                    Console.WriteLine("Claim rejected: hourly rate exceeded the limit.");
                }
                else
                {
                    status = "Pending"; // Default status for valid claims
                }

                
                var newClaim = new Contract_Monthly_Claim_System.Models.Claim
                {
                    lecturerName = model.lecturerName,
                    hoursWorked = model.hoursWorked,
                    hourlyRate = model.hourlyRate,
                    claimAmount = claimAmount,
                    status = status,
                    submissionDate = DateTime.Now,
                     UserId = userId
                };

                
                _context.Claims.Add(newClaim); //save the claim created to the database
                int rowsSaved = _context.SaveChanges();
                if (rowsSaved > 0)
                {
                    Console.WriteLine("Claim successfully saved!");
                }
                else
                {
                    Console.WriteLine("Failed to save claim.");
                }
                

                
                return RedirectToAction("Dashboard"); // Redirect to the dashboard
            }

            
            return View(model);
        }

        
        public IActionResult MyClaims()
        {
            var userId = User.Identity.Name;  // Get the logged-in user's ID (or username)
            var lecturerClaims = _context.Claims.Where(c => c.UserId == userId).ToList();  // Filter claims by user ID
            return View(lecturerClaims);  // Pass the filtered claims to the view
        }



    }
}

