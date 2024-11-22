namespace Contract_Monthly_Claim_System.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; } // Primary Key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfHire { get; set; }
    }
}
