using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;
using api.Services;

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
                Date = foodEntryModel.Date,
                MealType = foodEntryModel.MealType,
                FoodName = foodEntryModel.Food.Name,
                Calories = NutritionMath.Round(NutritionMath.ForQuantity(foodEntryModel.Food.CaloriesPer100g, foodEntryModel.QuantityGrams)),
                Protein = NutritionMath.Round(NutritionMath.ForQuantity(foodEntryModel.Food.ProteinPer100g, foodEntryModel.QuantityGrams)),
                Fat = NutritionMath.Round(NutritionMath.ForQuantity(foodEntryModel.Food.FatPer100g, foodEntryModel.QuantityGrams)),
                Carbs = NutritionMath.Round(NutritionMath.ForQuantity(foodEntryModel.Food.CarbsPer100g, foodEntryModel.QuantityGrams)),
                Sugar = NutritionMath.Round(NutritionMath.ForQuantity(foodEntryModel.Food.SugarPer100g, foodEntryModel.QuantityGrams)),

            };
        }

        public static NutritionTotalsDto ToTotalsDto(this IEnumerable<FoodEntry> entries)
        {
            var list = entries.ToList();

            return new NutritionTotalsDto
            {
                Calories = NutritionMath.Round(list.Sum(e => NutritionMath.ForQuantity(e.Food.CaloriesPer100g, e.QuantityGrams))),
                Protein  = NutritionMath.Round(list.Sum(e => NutritionMath.ForQuantity(e.Food.ProteinPer100g,  e.QuantityGrams))),
                Fat      = NutritionMath.Round(list.Sum(e => NutritionMath.ForQuantity(e.Food.FatPer100g,      e.QuantityGrams))),
                Carbs    = NutritionMath.Round(list.Sum(e => NutritionMath.ForQuantity(e.Food.CarbsPer100g,    e.QuantityGrams))),
                Sugar    = NutritionMath.Round(list.Sum(e => NutritionMath.ForQuantity(e.Food.SugarPer100g,    e.QuantityGrams))),
            };
        }


        public static FoodEntry ToFoodEntryFromCreate(this CreateFoodEntryDto entryFoodDto,string userId)
        {
            return new FoodEntry
            {
                Date = entryFoodDto.Date!.Value,
                MealType = entryFoodDto.MealType!.Value,
                QuantityGrams = entryFoodDto.QuantityGrams,
                FoodId  = entryFoodDto.FoodId!.Value,
                UserId = userId,
            };
        }

    }
}