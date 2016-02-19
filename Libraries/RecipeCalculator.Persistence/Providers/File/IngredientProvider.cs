using RecipeCalculator.Models;
using RecipeCalculator.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace RecipeCalculator.Persistence.Providers.File
{
    public class IngredientProvider : IIngredientProvider
    {
        private List<Ingredient> _ingredients;

        public IngredientProvider(string file)
        {
            _ingredients = new List<Ingredient>();
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            if (System.IO.File.Exists(file))
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    _ingredients = serializer.Deserialize<List<Ingredient>>(reader.ReadToEnd());
                }
            }
        }

        #region Methods

        public List<Ingredient> GetAllIngredients()
        {
            return _ingredients;
        }

        public List<Ingredient> GetIngredientsByType(IngredientType type)
        {
            return _ingredients.Where(x => x.Type == type).ToList();
        }

        public Ingredient GetIngredientById(int id)
        {
            return _ingredients.FirstOrDefault(x => x.Id == id);
        }

        public void Dispose()
        {
            _ingredients = null;
        }

        #endregion
    }
}
