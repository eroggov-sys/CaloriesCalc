using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class UserProfileMapper
    {
        public static UserProfileDto ToUserProfileDto(this UserProfile profile)
        {
            return new UserProfileDto
            {
                Id = profile.Id,
                WeightKg = profile.WeightKg,
                HeightCm = profile.HeightCm,
                DateOfBirth = profile.DateOfBirth,
                BiologicalSex = profile.BiologicalSex,
                ActivityLevel = profile.ActivityLevel,
                NutritionGoal = profile.NutritionGoal,
                BodyFatPercentage = profile.BodyFatPercentage
            };
        }
        public static void UpdateFromDto( this UserProfile profile, UpdateUserProfileDto dto)
        {
            profile.WeightKg = dto.WeightKg!.Value;
            profile.HeightCm = dto.HeightCm!.Value;
            profile.DateOfBirth = dto.DateOfBirth!.Value;
            profile.BiologicalSex = dto.BiologicalSex!.Value;
            profile.ActivityLevel = dto.ActivityLevel!.Value;
            profile.NutritionGoal = dto.NutritionGoal!.Value;
            profile.BodyFatPercentage = dto.BodyFatPercentage;

        }
    }
}