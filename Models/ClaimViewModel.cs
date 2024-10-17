namespace Contract_Monthly_Claim_System.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClaimViewModel
    {
        [Required]
        public string lecturerName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Hours worked must be greater than 0")]
        public double hoursWorked { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Hourly rate must be greater than 0")]
        public double hourlyRate { get; set; }

        public double claimAmount { get; set; }  // This is calculated, so not part of the form
    }

}
