using api.Models;
using api.Services;
using System.Globalization;
using Xunit;

namespace api.Tests
{
    public class NutritionCalculatorTests
    {
        [Fact]
        public void Calculate_WithBodyFat_UsesKatchMcArdle()
        {
            var calculator = new NutritionCalculator();

            var profile = new UserProfile
            {
                WeightKg = 75m,
                HeightCm = 180m,
                DateOfBirth = new DateOnly(2000, 5, 15),
                BiologicalSex = BiologicalSex.Male,
                ActivityLevel = ActivityLevel.ModeratelyActive,
                NutritionGoal = NutritionGoal.MaintainWeight,
                BodyFatPercentage = 18m
            };


            var result = calculator.Calculate(profile, new DateOnly(2026, 8, 29));    
            
            
            Assert.Equal(26, result.Age);
            Assert.Equal(1698m, result.Bmr);
            Assert.Equal(2633m, result.Tdee);
            Assert.Equal(2633m, result.Calories);
            Assert.Equal(98.4m, result.Protein);
            Assert.Equal(60m, result.Fat);
            Assert.Equal(424.7m, result.Carbs);
            Assert.Equal(65.8m, result.SugarLimit);

        }
        [Fact]
        public void Calculate_WithoutBodyFat_UsesMifflinStJeor()
        {
            var calculator = new NutritionCalculator();

            var profile = new UserProfile
            {
                WeightKg = 80m,
                HeightCm = 180m,
                DateOfBirth = new DateOnly(1996, 8, 30),
                BiologicalSex = BiologicalSex.Male,
                ActivityLevel = ActivityLevel.Sedentary,
                NutritionGoal = NutritionGoal.MaintainWeight,
                BodyFatPercentage = null
            };

            var result = calculator.Calculate(profile, new DateOnly(2026, 8, 30));

            Assert.Equal(30, result.Age);
            Assert.Equal(1780m, result.Bmr);
            Assert.Equal(2136m, result.Tdee);
            Assert.Equal(2136m, result.Calories);
            Assert.Equal(128m, result.Protein);
            Assert.Equal(64m, result.Fat);
            Assert.Equal(262m, result.Carbs);
            Assert.Equal(53.4m, result.SugarLimit);
        }

        [Fact]
        public void Calculate_ForFemaleBeforeBirthday_CalculatesCorrectly()
        {
            var calculator = new NutritionCalculator();

            var profile = new UserProfile
            {
                WeightKg = 60m,
                HeightCm = 165m,
                DateOfBirth = new DateOnly(1996, 9, 15),
                BiologicalSex = BiologicalSex.Female,
                ActivityLevel = ActivityLevel.LightlyActive,
                NutritionGoal = NutritionGoal.MaintainWeight,
                BodyFatPercentage = null
            };

            var result = calculator.Calculate(profile, new DateOnly(2026,8,30));

            Assert.Equal(29, result.Age);
            Assert.Equal(1325m, result.Bmr);
            Assert.Equal(1822m, result.Tdee);
            Assert.Equal(1822m, result.Calories);
            Assert.Equal(96m, result.Protein);
            Assert.Equal(48m, result.Fat);
            Assert.Equal(251.6m, result.Carbs);
            Assert.Equal(45.6m, result.SugarLimit);
        }

        [Theory]
        [InlineData(NutritionGoal.LoseWeight, "1816", "160", "149.9", "45.4")]
        [InlineData(NutritionGoal.MaintainWeight, "2136", "128", "262", "53.4")]
        [InlineData(NutritionGoal.GainWeight, "2350", "144", "299.4", "58.7")]
        public void Calculate_ForEachGoal_ReturnsCorrectTargets(NutritionGoal goal, 
        string expectedCalories, string expectedProtein, string expectedCarbs, string expectedSugarLimit)
        {
            var calculator = new NutritionCalculator();

            var profile = new UserProfile
            {
                WeightKg = 80m,
                HeightCm = 180m,
                DateOfBirth = new DateOnly(1996, 8, 30),
                BiologicalSex = BiologicalSex.Male,
                ActivityLevel = ActivityLevel.Sedentary,
                NutritionGoal = goal,
                BodyFatPercentage = null
            };
            var result = calculator.Calculate(profile, new DateOnly(2026, 8 , 30));

            Assert.Equal(decimal.Parse(expectedCalories, CultureInfo.InvariantCulture), result.Calories);
            Assert.Equal(decimal.Parse(expectedProtein, CultureInfo.InvariantCulture), result.Protein);
            Assert.Equal(decimal.Parse(expectedCarbs, CultureInfo.InvariantCulture), result.Carbs);
            Assert.Equal(decimal.Parse(expectedSugarLimit, CultureInfo.InvariantCulture), result.SugarLimit);
        }
        [Fact]
        public void Calculate_WhenAgeIsLessThanOne_ThrowsArgumentException()
        {
            var calculator = new NutritionCalculator();

            var profile = new UserProfile
            {
                WeightKg = 75m,
                HeightCm = 180m,
                DateOfBirth = new DateOnly(2026, 8, 30),
                BiologicalSex = BiologicalSex.Male,
                ActivityLevel = ActivityLevel.Sedentary,
                NutritionGoal = NutritionGoal.MaintainWeight,
                BodyFatPercentage = null
            };

            Assert.Throws<ArgumentException>(() => calculator.Calculate(profile, new DateOnly(2026, 8, 30)));
        }
    }

}