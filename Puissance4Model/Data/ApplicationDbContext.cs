using Puissance4Model.Models;

namespace Puissance4Model.Data;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    // Constructeur attendu par AddDbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=puissance4.db"); // Valeur par défaut si non configurée
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de Game
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(g => g.Id);

            entity.Property(g => g.Status).IsRequired();

            entity.HasOne(g => g.Host)
                  .WithMany(p => p.GamesAsHost)
                  .HasForeignKey(g => g.HostId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.Guest)
                  .WithMany(p => p.GamesAsGuest)
                  .HasForeignKey(g => g.GuestId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.Grid)
                  .WithOne()
                  .HasForeignKey<Grid>(g => g.GameId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration de Grid
        modelBuilder.Entity<Grid>(entity =>
        {
            entity.HasKey(gr => gr.Id);

            entity.Property(gr => gr.Rows).IsRequired();
            entity.Property(gr => gr.Columns).IsRequired();

            entity.HasMany(gr => gr.Cells)
                  .WithOne()
                  .HasForeignKey(c => c.GridId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration de Cell
        modelBuilder.Entity<Cell>(entity =>
        {
            entity.HasKey(c => c.Id); // Clé primaire
            entity.Property(c => c.Id).ValueGeneratedOnAdd(); // Génération automatique
        });

        // Configuration de Player
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Login).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Password).IsRequired();
            entity.HasIndex(p => p.Login).IsUnique();
        });
    }
}
