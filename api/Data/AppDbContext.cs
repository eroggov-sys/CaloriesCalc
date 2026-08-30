using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


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