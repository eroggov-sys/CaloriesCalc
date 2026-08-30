using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class NutritionCalculator : INutritionCalculator
    {
        public NutritionTargetDto Calculate(UserProfile profile, DateOnly calculationDate)
        {
            var age = CalculateAge(profile.DateOfBirth, calculationDate);

            if (age < 1) throw new ArgumentException("Date of birth in the past");
            
            var leanBodyMass = CalculateLeanBodyMass(profile);

            var bmr = profile.BodyFatPercentage.HasValue
                ? CalculateKatchMcArdleBmr(leanBodyMass!.Value)
                : CalculateMifflinStJeorBmr(profile, age);
            
            var activityCoefficent = GetActivityCoefficient(profile.ActivityLevel);

            var tdee = bmr * activityCoefficent;
            
            var targetCalories = tdee * GetGoalCoefficient(profile.NutritionGoal);
            
            var referenceWeight = leanBodyMass ?? profile.WeightKg;

            var protein = referenceWeight * GetProteinCoefficient(profile.NutritionGoal);

            var fat = profile.WeightKg * 0.8m;


            var caloriesFromProtein = protein * 4m;
            var caloriesFromFat = fat * 9m;

            var remainingCalories = targetCalories - caloriesFromProtein - caloriesFromFat;
            
            var carbs = Math.Max(0m, remainingCalories / 4m);
            
            var sugarLimit = targetCalories * 0.10m / 4m;

            return new NutritionTargetDto
            {
                Age = age,
                Bmr = Math.Round(bmr, 0),
                Tdee = Math.Round(tdee, 0),
                Calories = Math.Round(targetCalories, 0),
                Protein = Math.Round(protein, 1),
                Fat = Math.Round(fat, 1),
                Carbs = Math.Round(carbs, 1),
                SugarLimit = Math.Round(sugarLimit, 1)

            };

        }

        private static int CalculateAge(DateOnly dateOfBirth, DateOnly calculationDate)
        {
            var age = calculationDate.Year - dateOfBirth.Year;

            if (dateOfBirth > calculationDate.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public static decimal? CalculateLeanBodyMass(UserProfile profile)
        {
            if (!profile.BodyFatPercentage.HasValue) return null;

            return profile.WeightKg * (1m- profile.BodyFatPercentage.Value /100m);
        }
    
        private static decimal CalculateKatchMcArdleBmr(decimal leanBodyMass) 
        {
            return 370m + 21.6m * leanBodyMass;
        }

        private static decimal CalculateMifflinStJeorBmr(UserProfile profile, int age)
        {
            var baseBmr = 10m * profile.WeightKg + 6.25m * profile.HeightCm - 5m * age;

            if (profile.BiologicalSex == BiologicalSex.Male) return baseBmr + 5m;

            if (profile.BiologicalSex == BiologicalSex.Female) return baseBmr - 161m;

            throw new ArgumentOutOfRangeException(nameof(profile.BiologicalSex), "Invalid biological sex");
        }


        private static decimal GetActivityCoefficient(ActivityLevel activityLevel)
        {
            return activityLevel switch
            {
                ActivityLevel.Sedentary => 1.2m,
                ActivityLevel.LightlyActive => 1.375m,
                ActivityLevel.ModeratelyActive => 1.55m,
                ActivityLevel.VeryActive => 1.725m,
                ActivityLevel.ExtraActive => 1.9m,

                _ => throw new ArgumentOutOfRangeException(nameof(activityLevel), "Invalid activity level")
            };
        }
        private static decimal GetGoalCoefficient(NutritionGoal goal)
        {
            return goal switch
            {
                NutritionGoal.LoseWeight => 0.85m,
                NutritionGoal.MaintainWeight => 1m,
                NutritionGoal.GainWeight => 1.1m,

                _ => throw new ArgumentOutOfRangeException(nameof(goal),"Invalid nutrition goal")
            };
        }
        private static decimal GetProteinCoefficient(
            NutritionGoal goal)
        {
            return goal switch
            {
                NutritionGoal.LoseWeight => 2m,
                NutritionGoal.MaintainWeight => 1.6m,
                NutritionGoal.GainWeight => 1.8m,

                _ => throw new ArgumentOutOfRangeException( nameof(goal), "Invalid nutrition goal")
            };
        }
    
    }
}