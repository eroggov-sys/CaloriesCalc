using System.ComponentModel.DataAnnotations;
using api.Dtos;
using api.Models;
using Xunit;

namespace api.Tests
{
    public class UpdateUserProfileDtoTests
    {
        private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

        private static UpdateUserProfileDto CreateDto(DateOnly dateOfBirth)
        {
            return new UpdateUserProfileDto
            {
                WeightKg = 75m,
                HeightCm = 180m,
                DateOfBirth = dateOfBirth,
                BiologicalSex = BiologicalSex.Male,
                ActivityLevel = ActivityLevel.ModeratelyActive,
                NutritionGoal = NutritionGoal.MaintainWeight,
                BodyFatPercentage = null
            };
        }

        private static (bool IsValid, List<ValidationResult> Results) Validate(UpdateUserProfileDto dto)
        {
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(
                dto,
                new ValidationContext(dto),
                results,
                validateAllProperties: true);

            return (isValid, results);
        }

        [Fact]
        public void Validate_WithAdultAge_ReturnsNoErrors()
        {
            var dto = CreateDto(Today.AddYears(-30));

            var (isValid, results) = Validate(dto);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Validate_WithInfantAge_ReturnsAgeError()
        {
            var dto = CreateDto(Today.AddMonths(-6));

            var (isValid, results) = Validate(dto);

            Assert.False(isValid);

            var error = Assert.Single(results);
            Assert.Contains("Age", error.ErrorMessage);
            Assert.Contains(
                nameof(UpdateUserProfileDto.DateOfBirth),
                error.MemberNames);
        }

        [Fact]
        public void Validate_WithAgeOver120_ReturnsAgeError()
        {
            var dto = CreateDto(Today.AddYears(-121));

            var (isValid, results) = Validate(dto);

            Assert.False(isValid);
            Assert.Single(results);
        }

        [Fact]
        public void Validate_WithFutureDateOfBirth_ReturnsAgeError()
        {
            var dto = CreateDto(Today.AddYears(1));

            var (isValid, results) = Validate(dto);

            Assert.False(isValid);
            Assert.Single(results);
        }
    }
}