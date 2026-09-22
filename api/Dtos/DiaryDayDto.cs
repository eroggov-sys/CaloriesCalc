using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class DiaryDayDto
    {
        public DateOnly Date { get; set; }
        public NutritionTotalsDto Totals { get; set; } = new();
        public List<MealGroupDto> Meals { get; set; } = [];
    }
}