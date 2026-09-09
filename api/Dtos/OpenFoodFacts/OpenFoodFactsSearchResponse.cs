using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Dtos.OpenFoodFacts
{
    public class OpenFoodFactsSearchResponse
    {
        [JsonPropertyName("products")]
        public List<OpenFoodFactsProduct> Products { get; set; } = new List<OpenFoodFactsProduct>();
    }
}