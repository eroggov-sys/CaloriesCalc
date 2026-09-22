
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class ExternalFoodMapper
    {
        public static Food ToFood(this ExternalFoodDto dto)
        {
            
            return new Food
            {
                Name = dto.Name.Trim(),
                Brand = NormalizeBrand(dto.Brand),
                Barcode = dto.Barcode,
                Source = FoodSource.OpenFoodFacts,
                ExternalId = dto.ExternalIdentifier,
                CaloriesPer100g = dto.CaloriesPer100g ?? 0,
                ProteinPer100g = dto.ProteinPer100g ?? 0,
                FatPer100g = dto.FatPer100g ?? 0,
                CarbsPer100g = dto.CarbsPer100g ?? 0,
                SugarPer100g = dto.SugarPer100g ?? 0,
            };
        }
        internal static string? NormalizeBrand(string? brand)
        {
            if (string.IsNullOrWhiteSpace(brand)) return null;

            var firstBrand = brand.Split(',')[0].Trim();

            return firstBrand.Length == 0 ? null : firstBrand;
        }

        public static FoodDto ToCandidateDto(this ExternalFoodDto dto)
        {
            return new FoodDto
            {
                Id = 0,
                Name = dto.Name.Trim(),
                Brand = NormalizeBrand(dto.Brand),
                Barcode = dto.Barcode,
                Source = FoodSource.OpenFoodFacts,
                ExternalId = dto.ExternalIdentifier,
                CaloriesPer100g = dto.CaloriesPer100g ?? 0,
                ProteinPer100g = dto.ProteinPer100g ?? 0,
                FatPer100g = dto.FatPer100g ?? 0,
                CarbsPer100g = dto.CarbsPer100g ?? 0,
                SugarPer100g = dto.SugarPer100g ?? 0,
            };
        }
    }
}