using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Cryptography;
#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            var passwordLecturer = HashPassword("password123");
            var passwordCoordinator = HashPassword("password456");
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, passwordLecturer, "Lecturer", "lecturer1" },
                    { 2,  passwordCoordinator, "Coordinator", "coordinator1" }
                });
        }

        // The method to hash the password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);
        }
    }
}
