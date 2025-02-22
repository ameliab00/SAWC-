using Microsoft.EntityFrameworkCore;
using SAW.Models;

namespace SAW.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Review> Review { get; set; }

    // Konfiguracja modelu i indeksów
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unikalny indeks dla UserName
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        // Unikalny indeks dla Email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Unikalny indeks dla Email
        modelBuilder.Entity<Event>()
            .HasIndex(u => u.Title)
            .IsUnique();
    }
}