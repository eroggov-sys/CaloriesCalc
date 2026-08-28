using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class FoodEntry
    {
    public int Id { get; set; }
    public int FoodId { get; set; }
    public Food Food { get; set; } = null!;
    public decimal QuantityGrams { get; set; }
    public DateTime EatenAt { get; set; }
    public string MealType { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public AppUser? User { get; set; }
    }
}