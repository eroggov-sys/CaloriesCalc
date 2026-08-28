using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class MealGroupDto
    {
        public string MealType {get;set;} = string.Empty;
        public decimal TotalCalories {get; set;}
        public List<FoodEntryDto> Entries { get; set; } = null!;
    }
}