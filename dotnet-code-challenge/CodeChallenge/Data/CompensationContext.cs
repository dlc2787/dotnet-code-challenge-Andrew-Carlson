using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Data
{
    public class CompensationContext : DbContext
    {
        //constructor
        public CompensationContext(DbContextOptions<CompensationContext> options) : base(options) { }

        //property to store compensations
        public DbSet<Compensation> Compensations { get; set; }
    }
}
