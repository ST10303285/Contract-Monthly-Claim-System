using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims; // This is for user identity claims, which you can still keep
using Contract_Monthly_Claim_System.Models;
using Contract_Monthly_Claim_System.Data;

namespace Contract_Monthly_Claim_System.Controllers
{
    public class LecturerController : Controller
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
            var allClaims = _context.Claims.ToList();
            return View(allClaims);
        }

        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // For CSRF protection
        public IActionResult SubmitClaim(ClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Calculate the claim amount
                double claimAmount = model.hoursWorked * model.hourlyRate;

                // Use the fully qualified name for your custom Claim entity
                var newClaim = new Contract_Monthly_Claim_System.Models.Claim
                {
                    lecturerName = model.lecturerName,
                    hoursWorked = model.hoursWorked,
                    hourlyRate = model.hourlyRate,
                    claimAmount = claimAmount,
                    status = "Pending",  // Default status when a new claim is submitted
                    submissionDate = DateTime.Now
                };

                // Save the new claim to the database
                _context.Claims.Add(newClaim);
                int rowsSaved = _context.SaveChanges();
                if (rowsSaved > 0)
                {
                    Console.WriteLine("Claim successfully saved!");
                }
                else
                {
                    Console.WriteLine("Failed to save claim.");
                }
                

                // Redirect to the dashboard or a confirmation page
                return RedirectToAction("Dashboard");
            }

            // If the model is invalid, return the form with validation errors
            return View(model);
        }

        

    }
}

