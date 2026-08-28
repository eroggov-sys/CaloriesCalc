
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class CreateFoodDto
    {
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name is too long")]
    public string Name { get; set; } = string.Empty;

    
    [Range(typeof(decimal), "0", "1000", ParseLimitsInInvariantCulture = true)]
    public decimal CaloriesPer100g { get; set; }

    
    [Range(typeof(decimal), "0", "100", ParseLimitsInInvariantCulture = true)]
    public decimal ProteinPer100g { get; set; }

    
    [Range(typeof(decimal), "0", "100", ParseLimitsInInvariantCulture = true)]
    public decimal FatPer100g { get; set; }

    
    [Range(typeof(decimal), "0", "100", ParseLimitsInInvariantCulture = true)]
    public decimal CarbsPer100g { get; set; }

    
    [Range(typeof(decimal), "0", "100", ParseLimitsInInvariantCulture = true)]
    public decimal SugarPer100g { get; set; }
    }
}