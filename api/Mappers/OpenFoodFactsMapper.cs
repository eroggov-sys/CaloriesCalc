using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.OpenFoodFacts;

namespace api.Mappers
{
    public static class OpenFoodFactsMapper
    {
        public static ExternalFoodDto? ToExternalFoodDto(
        this OpenFoodFactsProduct product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) ||
                string.IsNullOrWhiteSpace(product.Barcode) ||
                product.Nutriments?.CaloriesPer100g is not >= 0m)
            {
                return null;
            }

            var nutrients = product.Nutriments;

            return new ExternalFoodDto
            {
                Name = product.Name.Trim(),
                Source = "OpenFoodFacts",
                ExternalIdentifier = product.Barcode,
                Barcode = product.Barcode,
                Brand = NormalizeText(product.Brand),
                CaloriesPer100g = NormalizeNutrient(
                    nutrients.CaloriesPer100g),
                ProteinPer100g = NormalizeNutrient(
                    nutrients.ProteinPer100g),
                FatPer100g = NormalizeNutrient(
                    nutrients.FatPer100g),
                CarbsPer100g = NormalizeNutrient(
                    nutrients.CarbsPer100g),
                SugarPer100g = NormalizeNutrient(
                    nutrients.SugarPer100g)
            };
        }

        private static string? NormalizeText(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private static decimal? NormalizeNutrient(decimal? value)
        {
            return value is >= 0m ? value : null;
        }
    }
}