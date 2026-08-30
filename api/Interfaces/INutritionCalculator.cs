using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Interfaces
{
    public interface INutritionCalculator
    {
        NutritionTargetDto Calculate(UserProfile profile, DateOnly calculationDate);
    }
}