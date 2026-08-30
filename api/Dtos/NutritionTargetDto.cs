using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class NutritionTargetDto
    {
        public int Age { get; set; }

        public decimal Bmr { get; set; }

        public decimal Tdee { get; set; }

        public decimal Calories { get; set; }

        public decimal Protein { get; set; }

        public decimal Fat { get; set; }

        public decimal Carbs { get; set; }
        public decimal SugarLimit { get; set; }

    }
}