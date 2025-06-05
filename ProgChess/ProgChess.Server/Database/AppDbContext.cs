using Microsoft.AspNetCore.Identity;
 using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
 using Microsoft.EntityFrameworkCore;
 using ProChess.Server.Entities;
 
 namespace ProgChess.Server.Database;
 
 public class AppDbContext: IdentityDbContext<User>
 {
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
     {
     }
     
     public DbSet<Exercise> Exercises { get; set; }
     public DbSet<UnitTest> UnitTests { get; set; }
     public DbSet<Score> Scores { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         base.OnModelCreating(modelBuilder);
         modelBuilder.HasDefaultSchema("progchess");
         SeedUsers(modelBuilder);
         
         // Defining the one-to-many relationship
         modelBuilder.Entity<Exercise>().HasMany(e => e.UnitTests).WithOne(ut => ut.Exercise).HasForeignKey(ut => ut.ExerciseId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
     }

     private void SeedUsers(ModelBuilder builder)
     {
         var hasher = new PasswordHasher<User>();

         var user = new User
         {
             Id = "test",
             UserName = "math",
             NormalizedUserName = "math",
             Email = "mathy@gmail.com",
             NormalizedEmail = "mathy@gmail.com",
             EmailConfirmed = true,
             SecurityStamp = Guid.NewGuid().ToString("D"),
             ConcurrencyStamp = Guid.NewGuid().ToString("D"),
         };
         
         user.PasswordHash = hasher.HashPassword(user, "test123");

         builder.Entity<User>().HasData(user);
     }
 }