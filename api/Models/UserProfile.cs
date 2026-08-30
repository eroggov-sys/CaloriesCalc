using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class UserProfile
    {
        public int Id {get; set;}
        public decimal WeightKg {get; set;}
        public decimal HeightCm {get; set;}
        public DateOnly DateOfBirth { get; set;}
        public BiologicalSex BiologicalSex {get; set;}
        public ActivityLevel ActivityLevel {get; set;}
        public NutritionGoal NutritionGoal{get; set;}
        public decimal? BodyFatPercentage {get; set;}
        public string UserId { get; set;} = string.Empty;
        public AppUser User {get; set;} = null!;
    
    }
}