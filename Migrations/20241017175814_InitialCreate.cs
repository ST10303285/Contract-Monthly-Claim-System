using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contract_Monthly_Claim_System.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lecturerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hoursWorked = table.Column<double>(type: "float", nullable: false),
                    hourlyRate = table.Column<double>(type: "float", nullable: false),
                    claimAmount = table.Column<double>(type: "float", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    submissionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.ClaimId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claims");
        }
    }
}
