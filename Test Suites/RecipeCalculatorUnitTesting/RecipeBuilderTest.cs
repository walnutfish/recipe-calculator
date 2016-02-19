using System;
using System.Linq;
using NUnit.Framework;
using RecipeCalculator.Persistence.Interfaces;
using RecipeCalculator.Persistence.Providers.Static;
using RecipeCalculator.Services;
using RecipeCalculator.Models;
using System.Collections.Generic;

namespace RecipeCalculatorUnitTesting
{
    [TestFixture]
    public class RecipeBuilderTest
    {
        private IIngredientProvider _provider = new IngredientProvider();

        private const double _salesTax = 0.086;
        private const double _salesTaxRoundUp = 0.07;
        private const double _discount = 0.05;

        [Test]
        public void OnlyProduceHasNoSalesTax()
        {
            RecipeBuilder builder = new RecipeBuilder(_provider);

            List<Ingredient> ingredients = _provider.GetIngredientsByType(IngredientType.Produce);

            foreach (Ingredient ingredient in ingredients)
            {
                builder.AddIngredient(ingredient.Id, 1);
            }

            RecipeTotals totals = builder.GetRecipeTotals();

            Assert.AreEqual(0, totals.Tax);
        }

        [Test]
        public void OnlyOrganicGetsDiscount()
        {
            RecipeBuilder builder = new RecipeBuilder(_provider);

            List<Ingredient> ingredients = _provider.GetAllIngredients().Where(x => x.IsOrganic == false).ToList();

            foreach (Ingredient ingredient in ingredients)
            {
                builder.AddIngredient(ingredient.Id, 1);
            }

            RecipeTotals totals = builder.GetRecipeTotals();

            Assert.AreEqual(0, totals.Discount);
        }

        [Test]
        public void SalesTaxIsCorrect()
        {
            RecipeBuilder builder = new RecipeBuilder(_provider);

            Ingredient ingredient = _provider.GetAllIngredients().FirstOrDefault(x => x.Type == IngredientType.Meat && x.IsOrganic == false);

            builder.AddIngredient(ingredient.Id, 1);

            //Get the sales tax of the ingredient
            double salesTax = ingredient.UnitPrice * _salesTax;

            //Round the sales tax to the nearest 7 cents
            salesTax = (double)(Math.Ceiling(salesTax / _salesTaxRoundUp) * _salesTaxRoundUp);

            RecipeTotals totals = builder.GetRecipeTotals();

            Assert.AreEqual(salesTax, totals.Tax);
        }

        [Test]
        public void OrganicDiscountIsCorrect()
        {
            RecipeBuilder builder = new RecipeBuilder(_provider);

            Ingredient ingredient = _provider.GetAllIngredients().FirstOrDefault(x => x.IsOrganic == true);

            builder.AddIngredient(ingredient.Id, 1);

            //Get the discount cost of the ingredient
            double discount = ingredient.UnitPrice * _discount;

            //Round the discount up to the nearest cent
            discount = (double)(Math.Ceiling(discount * 100) / 100);

            RecipeTotals totals = builder.GetRecipeTotals();

            Assert.AreEqual(discount, totals.Discount);
        }

        [Test]
        public void TotalCostIsCorrect()
        {
            RecipeBuilder builder = new RecipeBuilder(_provider);

            List<Ingredient> ingredients = _provider.GetIngredientsByType(IngredientType.Produce).Where(x => x.IsOrganic == false).ToList();

            double cost = 0;

            foreach (Ingredient ingredient in ingredients)
            {
                builder.AddIngredient(ingredient.Id, 1);

                cost += ingredient.UnitPrice;
            }

            RecipeTotals totals = builder.GetRecipeTotals();

            Assert.AreEqual(cost, totals.Total);
        }

        [Test]
        public void IngredientSkippedIfNotFound()
        {
            RecipeBuilder builder = new RecipeBuilder(_provider);

            //Add ingredient Id that does not exist
            bool success = builder.AddIngredient(0, 1);

            Assert.IsFalse(success);
        }
    }
}
