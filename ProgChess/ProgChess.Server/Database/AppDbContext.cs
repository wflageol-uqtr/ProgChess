using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;

namespace ProChess.Server.Database;

public class AppDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Exercice> Exercices { get; set; }
    public DbSet<UnitTest> UnitTests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("progchess");
        
        // Insert default user
        modelBuilder.Entity<User>().HasData(new User { Id = 1, Email = "test@test.com", Password ="test123" });
        
        // Defining the one-to-many relationship
        modelBuilder.Entity<Exercice>().HasMany(e => e.UnitTests).WithOne(ut => ut.Exercice).HasForeignKey(ut => ut.ExerciceId).IsRequired(false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
    }
}