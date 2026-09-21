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

    [Required(ErrorMessage = "Date is required")]
    public DateOnly? Date { get; set; }


    [Required(ErrorMessage = "MealType is required")]
    [EnumDataType(typeof(MealType), ErrorMessage = "Invalid meal type")]
    public MealType? MealType { get; set; }


    }
}