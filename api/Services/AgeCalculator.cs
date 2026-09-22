using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public static class AgeCalculator
    {
        public static int CalculateAge(DateOnly dateOfBirth, DateOnly onDate)
        {
            var age = onDate.Year - dateOfBirth.Year;

            if (dateOfBirth > onDate.AddYears(-age)) age--;

            return age;
        }
    }
}