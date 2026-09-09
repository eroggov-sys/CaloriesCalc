using api.Dtos.OpenFoodFacts;
using api.Mappers;

namespace api.Tests;

public class OpenFoodFactsMapperTests
{
    [Fact]
    public void ToExternalFoodDto_WithValidProduct_MapsValues()
    {
        var product = new OpenFoodFactsProduct
        {
            Name = "  Coca-Cola Zero  ",
            Barcode = "5449000131805",
            Brand = "  Coca-Cola  ",
            Nutriments = new OpenFoodFactsNutriments
            {
                CaloriesPer100g = 0m,
                ProteinPer100g = 0m,
                FatPer100g = 0m,
                CarbsPer100g = 0m,
                SugarPer100g = 0m
            }
        };

        var result = product.ToExternalFoodDto();

        Assert.NotNull(result);
        Assert.Equal("Coca-Cola Zero", result.Name);
        Assert.Equal("Coca-Cola", result.Brand);
        Assert.Equal("OpenFoodFacts", result.Source);
        Assert.Equal("5449000131805", result.ExternalIdentifier);
        Assert.Equal("5449000131805", result.Barcode);
        Assert.Equal(0m, result.CaloriesPer100g);
    }

    [Fact]
    public void ToExternalFoodDto_WithNegativeNutrient_ConvertsItToNull()
    {
        var product = new OpenFoodFactsProduct
        {
            Name = "Test product",
            Barcode = "123456789",
            Nutriments = new OpenFoodFactsNutriments
            {
                CaloriesPer100g = 100m,
                ProteinPer100g = -5m
            }
        };

        var result = product.ToExternalFoodDto();

        Assert.NotNull(result);
        Assert.Null(result.ProteinPer100g);
    }

    [Theory]
    [InlineData(null, "123", 100)]
    [InlineData("", "123", 100)]
    [InlineData("Food", null, 100)]
    [InlineData("Food", "", 100)]
    [InlineData("Food", "123", null)]
    [InlineData("Food", "123", -1)]
    public void ToExternalFoodDto_WithInvalidRequiredData_ReturnsNull(
        string? name,
        string? barcode,
        int? calories)
    {
        var product = new OpenFoodFactsProduct
        {
            Name = name,
            Barcode = barcode,
            Nutriments = new OpenFoodFactsNutriments
            {
                CaloriesPer100g = calories
            }
        };

        var result = product.ToExternalFoodDto();

        Assert.Null(result);
    }
}