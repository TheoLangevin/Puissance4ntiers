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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de Game
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(g => g.Id);

            entity.Property(g => g.Status)
                  .IsRequired();

            // Relation entre Game et Host
            entity.HasOne(g => g.Host)
                  .WithMany(p => p.GamesAsHost) // Relation inverse
                  .HasForeignKey(g => g.HostId)
                  .OnDelete(DeleteBehavior.Restrict); // Restriction pour éviter la suppression en cascade

            // Relation entre Game et Guest
            entity.HasOne(g => g.Guest)
                  .WithMany(p => p.GamesAsGuest) // Relation inverse
                  .HasForeignKey(g => g.GuestId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relation avec Grid
            entity.HasOne(g => g.Grid)
                  .WithOne()
                  .HasForeignKey<Game>(g => g.Id); // Suppose que Grid a un lien 1:1 avec Game
        });

        // Configuration de Grid
        modelBuilder.Entity<Grid>(entity =>
        {
            entity.HasKey(gr => gr.Id);

            entity.Property(gr => gr.Rows).IsRequired();
            entity.Property(gr => gr.Columns).IsRequired();

            entity.HasMany(gr => gr.Cells)
                  .WithOne()
                  .HasForeignKey(c => c.Id); // Clé étrangère pour les cellules
        });

        // Configuration des joueurs (Player)
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Login).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Password).IsRequired();
            entity.HasIndex(p => p.Login).IsUnique();
        });
    }
}
