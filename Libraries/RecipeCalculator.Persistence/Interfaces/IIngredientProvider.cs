using RecipeCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeCalculator.Persistence.Interfaces
{
    public interface IIngredientProvider : IDisposable
    {
        List<Ingredient> GetAllIngredients();
        List<Ingredient> GetIngredientsByType(IngredientType type);

        Ingredient GetIngredientById(int id);
    }
}
