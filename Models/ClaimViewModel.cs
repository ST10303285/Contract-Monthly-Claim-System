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

        public double claimAmount { get; set; }  
    }

}
