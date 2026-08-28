using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace api.Dtos
{
    public class UpdateFoodEntryRequestDto
    {
        [Range(typeof(decimal), "0.01", "100000", ParseLimitsInInvariantCulture = true, ErrorMessage = "Quantity must be greater than zero")]
        public decimal QuantityGrams { get; set; }

    }
}