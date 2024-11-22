using System.ComponentModel.DataAnnotations;

namespace Contract_Monthly_Claim_System.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        public string lecturerName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive number for Hours Worked")] 
        public double hoursWorked { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive number for Hourly Rate")]
        public double hourlyRate { get; set; }

        [Required]
        public double claimAmount { get; set; }

        [Required]
        public string status { get; set; }

        public DateTime submissionDate { get; set; }

        public string UserId { get; set; }

        
    }
}
