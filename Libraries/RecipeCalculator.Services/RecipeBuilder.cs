using RecipeCalculator.Models;
using RecipeCalculator.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeCalculator.Services
{
    public class RecipeBuilder
    {
        private IIngredientProvider _provider;
        private Dictionary<Ingredient, double> _ingredients;

        private const double _salesTax = 0.086;
        private const double _salesTaxRoundUp = 0.07;
        private const double _discount = 0.05;

        public RecipeBuilder(IIngredientProvider provider)
        {
            _provider = provider;

            _ingredients = new Dictionary<Ingredient, double>();
        }

        #region Methods

        public bool AddIngredient(int id, double quantity)
        {
            Ingredient ingredient = _provider.GetIngredientById(id);

            if (ingredient != null)
            {
                _ingredients.Add(ingredient, quantity);

                return true;
            }

            return false;
        }

        public RecipeTotals GetRecipeTotals()
        {
            RecipeTotals totals = new RecipeTotals();

            foreach(KeyValuePair<Ingredient, double> kvp in _ingredients)
            {
                double cost = kvp.Key.UnitPrice * kvp.Value;

                if (kvp.Key.Type != IngredientType.Produce)
                {
                    double tax = cost * _salesTax;

                    totals.Tax += tax;
                }

                if (kvp.Key.IsOrganic)
                {
                    double discount = cost * _discount;

                    totals.Discount += discount;
                }

                totals.Total += cost;
            }

            totals.Tax = (double)(Math.Ceiling(totals.Tax / _salesTaxRoundUp) * _salesTaxRoundUp);
            totals.Discount = (double)(Math.Ceiling(totals.Discount * 100) / 100);

            totals.Total += totals.Tax;
            totals.Total -= totals.Discount;

            return totals;
        }

        #endregion
    }
}
