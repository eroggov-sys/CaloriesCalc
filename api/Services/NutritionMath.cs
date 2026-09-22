using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public static class NutritionMath
    {
        public static decimal ForQuantity(decimal per100g, decimal grams) => 
            per100g * grams / 100m;

        public static decimal Round(decimal value) => 
            Math.Round(value, 1, MidpointRounding.AwayFromZero);
    }
}