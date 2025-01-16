// DAL/Context/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Puissance4API.Context
{
    public class ApplicationDbContext : <IdentityDbContext> // ou DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet<Game> ou autres tables
        public DbSet<Game> Games { get; set; }
    }
}