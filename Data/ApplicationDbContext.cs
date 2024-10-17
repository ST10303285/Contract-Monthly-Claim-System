using Microsoft.EntityFrameworkCore;
using Contract_Monthly_Claim_System.Models;

namespace Contract_Monthly_Claim_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Claim> Claims { get; set; }
    }
}