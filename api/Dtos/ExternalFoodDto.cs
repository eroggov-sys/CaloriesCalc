using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class ExternalFoodDto
    {
    public required string Name {get; set;} = string.Empty;
    public required string Source { get; set; }
    public required string ExternalIdentifier { get; set; }
    public string? Barcode {get; set;} 
    public string? Brand { get; set; }
    public decimal? CaloriesPer100g { get; set; }
    public decimal? ProteinPer100g { get; set; }
    public decimal? FatPer100g { get; set; }
    public decimal? CarbsPer100g { get; set; }
    public decimal? SugarPer100g {get; set;}
    }
}