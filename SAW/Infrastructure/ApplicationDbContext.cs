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

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        
        modelBuilder.Entity<Event>()
            .HasIndex(u => u.Title)
            .IsUnique();
    }
}