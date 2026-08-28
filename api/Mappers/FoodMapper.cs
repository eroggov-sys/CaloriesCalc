using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class FoodMapper
    {
        public static Food ToFoodFromCreate(this CreateFoodDto dto)
        {
            return new Food
            {
                Name = dto.Name.Trim(),
                CaloriesPer100g = dto.CaloriesPer100g,
                ProteinPer100g = dto.ProteinPer100g,
                FatPer100g = dto.FatPer100g,
                CarbsPer100g = dto.CarbsPer100g,
                SugarPer100g = dto.SugarPer100g,
            };
        }
    }
}