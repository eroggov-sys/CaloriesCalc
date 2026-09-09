using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Dtos.OpenFoodFacts
{
    public class OpenFoodFactsProduct
    {

    [JsonPropertyName("nutriments")]
    public OpenFoodFactsNutriments? Nutriments { get; set; }

    [JsonPropertyName("product_name")]
    public string? Name {get; set;} = string.Empty;

    [JsonPropertyName("code")]
    public string? Barcode {get; set;} 

    [JsonPropertyName("brands")]
    public string? Brand { get; set; }
    }
}