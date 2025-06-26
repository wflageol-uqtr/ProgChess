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
     public DbSet<Student> Students { get; set; }
     public DbSet<StudentExercise> StudentExercises { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         base.OnModelCreating(modelBuilder);
         modelBuilder.HasDefaultSchema("progchess");
         SeedUsers(modelBuilder);
         
         // Defining the one-to-many relationship
         modelBuilder.Entity<Exercise>().HasMany(e => e.UnitTests).WithOne(ut => ut.Exercise).HasForeignKey(ut => ut.ExerciseId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

         // Defining the many-to-many relationship
         modelBuilder.Entity<StudentExercise>()
             .HasKey(se => new { se.StudentId, se.ExerciseId });
         
         modelBuilder.Entity<Exercise>()
             .HasMany(e => e.StudentExercises)
             .WithOne(s => s.Exercise)
             .IsRequired();
         
         modelBuilder.Entity<Student>()
             .HasMany(s => s.StudentExercises)
             .WithOne(s => s.Student)
             .IsRequired();
         
         modelBuilder.Entity<StudentExercise>()
             .Property(se => se.IsComplete)
             .HasDefaultValue(false);
     }

     public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ())
     {
         var entries = ChangeTracker
             .Entries()
             .Where(e => e.Entity is DateEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

         foreach (var entityEntry in entries)
         {
             ((DateEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
             if (entityEntry.State == EntityState.Added)
             {
                 ((DateEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;

             }
         }
         return base.SaveChangesAsync(cancellationToken);
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