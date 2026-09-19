using System.Text.Json.Serialization;

namespace api.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter<FoodSource>))]
    public enum FoodSource
    {
        Manual = 1,
        OpenFoodFacts = 2,
        Usda = 3,
    }
}