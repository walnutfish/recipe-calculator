using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeCalculator.Models
{
    public class Ingredient
    {
        public int Id {get;set;}

        public string Name { get; set; }

        public double UnitPrice { get; set; }

        public IngredientType Type { get; set; }

        public bool IsOrganic { get; set; }
    }
}
