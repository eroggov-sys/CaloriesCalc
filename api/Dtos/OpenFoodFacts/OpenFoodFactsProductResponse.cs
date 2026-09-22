
using System.Text.Json.Serialization;

namespace api.Dtos.OpenFoodFacts
{
    public class OpenFoodFactsProductResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("product")]
        public OpenFoodFactsProduct? Product { get; set; }
    }
}