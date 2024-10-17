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
         public double hoursWorked { get; set; }

        [Required]
        public double hourlyRate { get; set; }

        [Required]
        public double claimAmount { get; set; }

        [Required]
        public string status { get; set; }

        public DateTime submissionDate { get; set; }
    }
}
