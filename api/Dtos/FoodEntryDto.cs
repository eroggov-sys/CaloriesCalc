using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos
{
    public class FoodEntryDto
    {
    public int Id { get; set; }
    public int FoodId { get; set; }
    public decimal QuantityGrams { get; set; }
    public DateTime EatenAt { get; set; }
    public string MealType { get; set; } = string.Empty;
    public string FoodName {get; set;} = string.Empty;
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Fat { get; set; }
    public decimal Carbs { get; set; }
    public decimal Sugar {get; set;}
    }
}