
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace api.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Food> Foods { get; set; }
    public DbSet<FoodEntry> FoodEntries { get; set; }
    public DbSet<UserProfile> UserProfiles {get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("pg_trgm");

            
            builder.Entity<FoodEntry>(entity =>
            {
                entity.HasIndex(entry => new { entry.UserId, entry.Date });
                entity.Property(entry => entry.MealType).HasConversion<string>();
                entity.HasOne(entry => entry.User)
                    .WithMany(user => user.FoodEntries)
                    .HasForeignKey(entry => entry.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Food>(entity =>
            {
                entity.Property(food => food.Name).HasMaxLength(200);
                entity.Property(food => food.Brand).HasMaxLength(200);
                entity.Property(food => food.Barcode).HasMaxLength(50);
                entity.Property(food => food.ExternalId).HasMaxLength(100);

                entity.Property(food => food.Source).HasConversion<string>();

                entity.HasIndex(food => new { food.Source, food.ExternalId })
                    .IsUnique()
                    .HasFilter("\"ExternalId\" IS NOT NULL");

                entity.HasIndex(food => food.Barcode)
                    .HasFilter("\"Barcode\" IS NOT NULL");
                    
                entity.HasIndex(food => food.Name)
                    .HasMethod("gin")
                    .HasOperators("gin_trgm_ops");

            });

            builder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(profile => profile.Id);
                entity.HasIndex(profile => profile.UserId).IsUnique();
                entity.Property(profile => profile.UserId).IsRequired();
                entity.Property(profile => profile.WeightKg).HasPrecision(6,2);
                entity.Property(profile => profile.HeightCm).HasPrecision(5,2);
                entity.Property(profile => profile.BodyFatPercentage).HasPrecision(5,2);
                entity.HasOne(profile => profile.User)
                    .WithOne(user => user.Profile)
                    .HasForeignKey<UserProfile>(profile => profile.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("CK_UserProfile_WeightKg", 
                    "\"WeightKg\" >= 20 AND \"WeightKg\" <= 500");

                    table.HasCheckConstraint(
                        "CK_UserProfiles_HeightCm",
                        "\"HeightCm\" >= 50 AND \"HeightCm\" <= 300");

                    table.HasCheckConstraint(
                        "CK_UserProfiles_BodyFatPercentage",
                        "\"BodyFatPercentage\" IS NULL OR " +
                        "(\"BodyFatPercentage\" >= 1 AND \"BodyFatPercentage\" <= 75)");


                });

            });
        }
    }

}