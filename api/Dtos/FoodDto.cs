using api.Models;

namespace api.Dtos
{
    public class FoodDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Barcode { get; set; }
        public FoodSource Source { get; set; }
        public decimal CaloriesPer100g { get; set; }
        public decimal ProteinPer100g { get; set; }
        public decimal FatPer100g { get; set; }
        public decimal CarbsPer100g { get; set; }
        public decimal SugarPer100g { get; set; }
        public string? ExternalId { get; set; }

    }
}