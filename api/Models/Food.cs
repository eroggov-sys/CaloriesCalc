using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Food
    {
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal CaloriesPer100g { get; set; }

    public decimal ProteinPer100g { get; set; }

    public decimal FatPer100g { get; set; }

    public decimal CarbsPer100g { get; set; }
    public decimal SugarPer100g {get; set;}

    }
}