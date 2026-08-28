using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class FoodEntryMapper
    {
        public static FoodEntryDto ToFoodEntryDto(this FoodEntry foodEntryModel)
        {
            return new FoodEntryDto
            {
                Id = foodEntryModel.Id,
                FoodId = foodEntryModel.FoodId,
                QuantityGrams = foodEntryModel.QuantityGrams,
                EatenAt = foodEntryModel.EatenAt,
                MealType = foodEntryModel.MealType,
                FoodName = foodEntryModel.Food.Name,
                Calories = foodEntryModel.Food.CaloriesPer100g * foodEntryModel.QuantityGrams / 100,
                Protein = foodEntryModel.Food.ProteinPer100g * foodEntryModel.QuantityGrams / 100,
                Fat = foodEntryModel.Food.FatPer100g * foodEntryModel.QuantityGrams / 100,
                Carbs = foodEntryModel.Food.CarbsPer100g * foodEntryModel.QuantityGrams / 100,
                Sugar = foodEntryModel.Food.SugarPer100g * foodEntryModel.QuantityGrams / 100,

            };
        }

        public static FoodEntry ToFoodEntryFromCreate(this CreateEntryFoodDto entryFoodDto, int foodId, string userId)
        {
            return new FoodEntry
            {
                EatenAt = entryFoodDto.EatenAt!.Value,
                MealType = entryFoodDto.MealType,
                QuantityGrams = entryFoodDto.QuantityGrams,
                FoodId  = foodId,
                UserId = userId,
            };
        }

        public static FoodEntry ToFoodEntrytFromUpdate(this UpdateFoodEntryRequestDto foodEntrytDto)
        {
            return new FoodEntry
            {
                QuantityGrams = foodEntrytDto.QuantityGrams,
                
            };
        }
    }
}