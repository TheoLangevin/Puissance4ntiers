using Puissance4Model.Models;

namespace Puissance4Model.Data;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Grid> Grids { get; set; }
    public DbSet<Cell> Cells { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=puissance4.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.Host)
            .WithMany(p => p.GamesAsHost) 
            .HasForeignKey(g => g.HostId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Game>()
            .HasOne(g => g.Guest)
            .WithMany(p => p.GamesAsGuest) 
            .HasForeignKey(g => g.GuestId)
            .OnDelete(DeleteBehavior.Restrict); 
    }

}
