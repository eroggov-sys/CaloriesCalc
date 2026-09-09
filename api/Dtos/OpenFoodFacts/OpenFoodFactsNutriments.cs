using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Dtos.OpenFoodFacts
{
    public class OpenFoodFactsNutriments
    {
    [JsonPropertyName("energy-kcal_100g")]
    public decimal? CaloriesPer100g { get; set; }

    [JsonPropertyName("proteins_100g")]
    public decimal? ProteinPer100g { get; set; }

    [JsonPropertyName("fat_100g")]
    public decimal? FatPer100g { get; set; }

    [JsonPropertyName("carbohydrates_100g")]
    public decimal? CarbsPer100g { get; set; }

    [JsonPropertyName("sugars_100g")]
    public decimal? SugarPer100g {get; set;}
    }
}