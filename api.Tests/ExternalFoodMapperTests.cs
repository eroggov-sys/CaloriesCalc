using api.Dtos;
using api.Mappers;
using api.Models;

namespace api.Tests;

public class ExternalFoodMapperTests
{
    private static ExternalFoodDto CreateDto() => new()
    {
        Name = "  Nutella  ",
        Source = "OpenFoodFacts",
        ExternalIdentifier = "3017620422003",
        Barcode = "3017620422003",
        Brand = "Ferrero",
        CaloriesPer100g = 539m,
        ProteinPer100g = 6.3m,
        FatPer100g = 30.9m,
        CarbsPer100g = 57.5m,
        SugarPer100g = 56.3m
    };

    [Fact]
    public void ToFood_WithFullData_MapsAllFields()
    {
        var dto = CreateDto();

        var food = dto.ToFood();

        Assert.Equal("Nutella", food.Name);
        Assert.Equal("Ferrero", food.Brand);
        Assert.Equal("3017620422003", food.Barcode);
        Assert.Equal("3017620422003", food.ExternalId);
        Assert.Equal(FoodSource.OpenFoodFacts, food.Source);
        Assert.Equal(539m, food.CaloriesPer100g);
        Assert.Equal(6.3m, food.ProteinPer100g);
        Assert.Equal(30.9m, food.FatPer100g);
        Assert.Equal(57.5m, food.CarbsPer100g);
        Assert.Equal(56.3m, food.SugarPer100g);
    }

    [Fact]
    public void ToFood_WithMissingNutrients_UsesZero()
    {
        var dto = CreateDto();
        dto.ProteinPer100g = null;
        dto.FatPer100g = null;
        dto.CarbsPer100g = null;
        dto.SugarPer100g = null;

        var food = dto.ToFood();

        Assert.Equal(0m, food.ProteinPer100g);
        Assert.Equal(0m, food.FatPer100g);
        Assert.Equal(0m, food.CarbsPer100g);
        Assert.Equal(0m, food.SugarPer100g);
        Assert.Equal(539m, food.CaloriesPer100g);
    }

    [Fact]
    public void ToFood_WithoutBrandAndBarcode_KeepsThemNull()
    {
        var dto = CreateDto();
        dto.Brand = null;
        dto.Barcode = null;

        var food = dto.ToFood();

        Assert.Null(food.Brand);
        Assert.Null(food.Barcode);
        Assert.Equal("3017620422003", food.ExternalId);
    }

    [Fact]
    public void ToFood_AlwaysSetsExternalIdFromIdentifier()
    {
        var dto = CreateDto();
        dto.ExternalIdentifier = "999";
        dto.Barcode = "3017620422003";

        var food = dto.ToFood();

        Assert.Equal("999", food.ExternalId);
        Assert.Equal("3017620422003", food.Barcode);
    }

    [Theory]
    [InlineData("Nutella, Ferrero", "Nutella")]
    [InlineData("  Ferrero  ", "Ferrero")]
    [InlineData("", null)]
    [InlineData(null, null)]
    public void ToFood_NormalizesBrand(string? brand, string? expected)
    {
        var dto = CreateDto();
        dto.Brand = brand;

        var food = dto.ToFood();

        Assert.Equal(expected, food.Brand);
    }
}
