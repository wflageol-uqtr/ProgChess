using Microsoft.AspNetCore.Identity;
 using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
 using Microsoft.EntityFrameworkCore;
 using ProChess.Server.Entities;
 using ProChess.Server.Entities.Interface;

 namespace ProgChess.Server.Database;
 
 public class AppDbContext: IdentityDbContext<User>
 {
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
     {
     }
     
     public DbSet<Exercise> Exercises { get; set; }
     public DbSet<UnitTest> UnitTests { get; set; }
     public DbSet<Score> Scores { get; set; }
     public DbSet<ScoreTest> ScoreTest { get; set; }
     public DbSet<StudentExercise> StudentExercises { get; set; }
     public DbSet<Image> Images { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         base.OnModelCreating(modelBuilder);
         modelBuilder.HasDefaultSchema("progchess");
         SeedUsers(modelBuilder);
         
         // Defining the one-to-many relationship
         modelBuilder.Entity<Exercise>().HasMany(e => e.UnitTests).WithOne(ut => ut.Exercise).HasForeignKey(ut => ut.ExerciseId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

         // Defining the many-to-many relationship
         modelBuilder.Entity<StudentExercise>()
             .HasIndex(se => new { se.StudentPermanentCode, se.ExerciseId })
             .IsUnique();
         
         modelBuilder.Entity<Exercise>()
             .HasMany(e => e.StudentExercises)
             .WithOne(s => s.Exercise)
             .IsRequired();
         
         modelBuilder.Entity<StudentExercise>()
             .Property(se => se.IsComplete)
             .HasDefaultValue(false);
         
         modelBuilder.Entity<Exercise>()
             .HasOne(e => e.User)
             .WithMany(u => u.Exercises)
             .HasForeignKey(e => e.UserId);
         
         modelBuilder.Entity<Image>()
             .HasOne(i => i.User)
             .WithMany(u => u.Images)
             .IsRequired();
         
         // Soft delete not in query
         modelBuilder.Entity<Exercise>().HasQueryFilter(p => !p.IsDeleted);
         modelBuilder.Entity<Score>().HasQueryFilter(p => !p.IsDeleted);
         modelBuilder.Entity<ScoreTest>().HasQueryFilter(p => !p.IsDeleted);
         modelBuilder.Entity<StudentExercise>().HasQueryFilter(p => !p.IsDeleted);
         modelBuilder.Entity<UnitTest>().HasQueryFilter(p => !p.IsDeleted);
     }

     public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ())
     {
         // Soft Delete
         var softDeleteEntries = ChangeTracker.Entries<ISoftDeletable>()
             .Where(e => e.State == EntityState.Deleted);

         foreach (var entry in softDeleteEntries)
         {
             entry.State = EntityState.Modified;
             entry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
             entry.Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = DateTime.UtcNow;
         }
         // Add CreateAt and ModifiedAt
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

         var user1 = new User
         {
             Id = Guid.NewGuid().ToString(),
             UserName = "math",
             NormalizedUserName = "math",
             Email = "mathy@gmail.com",
             NormalizedEmail = "mathy@gmail.com",
             EmailConfirmed = true,
             SecurityStamp = Guid.NewGuid().ToString("D"),
             ConcurrencyStamp = Guid.NewGuid().ToString("D"),
         };
         user1.PasswordHash = hasher.HashPassword(user1, "test123");
         
         var user2 = new User
         {
             Id = Guid.NewGuid().ToString(),
             UserName = "test",
             NormalizedUserName = "test",
             Email = "test@gmail.com",
             NormalizedEmail = "test@gmail.com",
             EmailConfirmed = true,
             SecurityStamp = Guid.NewGuid().ToString("D"),
             ConcurrencyStamp = Guid.NewGuid().ToString("D"),
         };
         user2.PasswordHash = hasher.HashPassword(user2, "testtest");

         builder.Entity<User>().HasData(user1, user2);
     }
 }