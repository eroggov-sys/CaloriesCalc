
using api.Models;

namespace api.Dtos
{
    public class MealGroupDto
    {
        public MealType MealType {get;set;}
        public NutritionTotalsDto Totals { get; set; } = new();
        public List<FoodEntryDto> Entries { get; set; } = [];
    }
}