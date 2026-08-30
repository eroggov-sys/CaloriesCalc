using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Razor.Hosting;

namespace api.Dtos
{
    public class UpdateUserProfileDto
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

    }
}