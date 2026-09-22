using System.Text.Json.Serialization;

namespace api.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter<MealType>))]
    public enum MealType
    {
        Breakfast = 1,
        Lunch = 2,
        Dinner = 3,
        Snacks = 4,
    }
}