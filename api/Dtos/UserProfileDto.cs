using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos
{
    public class UserProfileDto
    {
        public int Id {get; set;}
        public decimal WeightKg{ get; set;}
        public decimal HeightCm { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public BiologicalSex BiologicalSex { get; set; }

        public ActivityLevel ActivityLevel { get; set; }

        public NutritionGoal NutritionGoal { get; set; }

        public decimal? BodyFatPercentage { get; set; }
    }
}