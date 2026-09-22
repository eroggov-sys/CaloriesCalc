using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Razor.Hosting;

namespace api.Dtos
{
    public class UpdateUserProfileDto : IValidatableObject
    {
        [Required(ErrorMessage ="Weight is required")]
        [Range(typeof(decimal), "20", "500", ParseLimitsInInvariantCulture = true,
            ErrorMessage = "Weight must be between 20 and 500 kg" )]
        public decimal? WeightKg {get; set;}

        [Required(ErrorMessage = "Height is required")]
        [Range(typeof(decimal), "50", "300", ParseLimitsInInvariantCulture = true,
            ErrorMessage = "Height must be between 50 and 300 cm")]
        public decimal? HeightCm { get; set; }
        
        [Required(ErrorMessage = "Date of birth is required")]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Biological sex is required")]
        [EnumDataType(typeof(BiologicalSex), ErrorMessage = "Invalid biological sex")]
        public BiologicalSex? BiologicalSex { get; set; }

        [Required(ErrorMessage = "Activity level is required")]
        [EnumDataType(typeof(ActivityLevel), ErrorMessage = "Invalid activity level")]
        public ActivityLevel? ActivityLevel { get; set; }

        [Required(ErrorMessage = "Nutrition goal is required")]
        [EnumDataType(typeof(NutritionGoal), ErrorMessage = "Invalid nutrition goal")]
        public NutritionGoal? NutritionGoal { get; set; }

        [Range(typeof(decimal), "1", "75", ParseLimitsInInvariantCulture = true,
            ErrorMessage = "Body fat percentage must be between 1 and 75")]
        public decimal? BodyFatPercentage { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth is null) yield break;

            var age = AgeCalculator.CalculateAge(
                DateOfBirth.Value,
                DateOnly.FromDateTime(DateTime.UtcNow));

            if (age < 13 || age > 120)
            {
                yield return new ValidationResult(
                    "Age must be between 13 and 120 years",
                    [nameof(DateOfBirth)]);
            }
        }

    }
}