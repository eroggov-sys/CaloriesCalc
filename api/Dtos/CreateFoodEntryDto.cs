using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class CreateFoodEntryDto
    {
    [Range(typeof(decimal), "0.01", "5000", ParseLimitsInInvariantCulture = true, ErrorMessage = "Quantity must be between 0.01 and 5000 g")]
    public decimal QuantityGrams { get; set; }

    [Required(ErrorMessage = "EatenAt is required")]
    public DateTime? EatenAt { get; set; }

    [Required(ErrorMessage = "MealType is required")]
    [RegularExpression("^(Breakfast|Lunch|Dinner|Snacks)$", ErrorMessage = "Invalid meal type")]
    public string MealType { get; set; } = string.Empty;

    }
}