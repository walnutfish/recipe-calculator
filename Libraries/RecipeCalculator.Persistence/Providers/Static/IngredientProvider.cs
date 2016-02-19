using RecipeCalculator.Models;
using RecipeCalculator.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeCalculator.Persistence.Providers.Static
{
    public class IngredientProvider : IIngredientProvider
    {
        private List<Ingredient> _ingredients;

        public IngredientProvider()
        {
            _ingredients = new List<Ingredient>();

            _ingredients.Add(new Ingredient() { Id = 1, Name = "Garlic", IsOrganic = true, Type = IngredientType.Produce, UnitPrice = 0.67 });
            _ingredients.Add(new Ingredient() { Id = 2, Name = "Lemon", IsOrganic = false, Type = IngredientType.Produce, UnitPrice = 2.03 });
            _ingredients.Add(new Ingredient() { Id = 3, Name = "Corn", IsOrganic = false, Type = IngredientType.Produce, UnitPrice = 0.87 });
            _ingredients.Add(new Ingredient() { Id = 4, Name = "Chicken Breast", IsOrganic = false, Type = IngredientType.Meat, UnitPrice = 2.19 });
            _ingredients.Add(new Ingredient() { Id = 5, Name = "Bacon", IsOrganic = false, Type = IngredientType.Meat, UnitPrice = 0.24 });
            _ingredients.Add(new Ingredient() { Id = 6, Name = "Pasta", IsOrganic = false, Type = IngredientType.Pantry, UnitPrice = 0.31 });
            _ingredients.Add(new Ingredient() { Id = 7, Name = "Olive Oil", IsOrganic = true, Type = IngredientType.Pantry, UnitPrice = 1.92 });
            _ingredients.Add(new Ingredient() { Id = 8, Name = "Vinegar", IsOrganic = false, Type = IngredientType.Pantry, UnitPrice = 1.26 });
            _ingredients.Add(new Ingredient() { Id = 9, Name = "Salt", IsOrganic = false, Type = IngredientType.Pantry, UnitPrice = 0.16 });
            _ingredients.Add(new Ingredient() { Id = 10, Name = "Pepper", IsOrganic = false, Type = IngredientType.Pantry, UnitPrice = 0.17 });
        }

        public List<Ingredient> GetAllIngredients()
        {
            return _ingredients;
        }

        public List<Ingredient> GetIngredientsByType(Models.IngredientType type)
        {
            return _ingredients.Where(x => x.Type == type).ToList();
        }

        public Ingredient GetIngredientById(int id)
        {
            return _ingredients.FirstOrDefault(x => x.Id == id);
        }

        public void Dispose()
        {
            
        }
    }
}
