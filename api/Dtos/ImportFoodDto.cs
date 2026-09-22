using System.ComponentModel.DataAnnotations;
using api.Models;

namespace api.Dtos
{
    public class ImportFoodDto
    {
        [Required]
        [EnumDataType(typeof(FoodSource))]
        public FoodSource? Source { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string ExternalId { get; set; } = string.Empty;
    }
}