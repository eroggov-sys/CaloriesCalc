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

        public static FoodDto ToFoodDto(this Food food)
        {
            return new FoodDto
            {
                Id = food.Id,
                Name = food.Name,
                Brand = food.Brand,
                Barcode = food.Barcode,
                Source = food.Source,
                CaloriesPer100g = food.CaloriesPer100g,
                ProteinPer100g = food.ProteinPer100g,
                FatPer100g = food.FatPer100g,
                CarbsPer100g = food.CarbsPer100g,
                SugarPer100g = food.SugarPer100g,
            };
        }
    }
}