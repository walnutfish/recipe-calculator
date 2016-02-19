using RecipeCalculator.Models;
using RecipeCalculator.Persistence.Providers.File;
using RecipeCalculator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RecipeCalculator
{
    class Program
    {
        private static string _ingredientsFile = null;
        private static string _recipeFile = null;

        static void Main(string[] args)
        {
            args = new List<string>() { "-i", @"c:\ingredients.txt", "-r", @"c:\recipe2.txt" }.ToArray();

            if (ParseCommandArguments(args))
            {
                if (File.Exists(_ingredientsFile) && File.Exists(_recipeFile))
                {
                    RecipeBuilder builder = new RecipeBuilder(new IngredientProvider(_ingredientsFile));

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    using (StreamReader reader = new StreamReader(_recipeFile))
                    {
                        List<RecipeRequirement> requirements = serializer.Deserialize<List<RecipeRequirement>>(reader.ReadToEnd());

                        foreach (RecipeRequirement req in requirements)
                        {
                            bool success = builder.AddIngredient(req.Id, req.Quantity);

                            if (success == false)
                            {
                                Console.WriteLine(String.Format("Ingredient (ID:{0}) could not be found. The recipe total will not include that ingredient.", req.Id));
                            }
                        }

                        RecipeTotals results = builder.GetRecipeTotals();

                        Console.WriteLine(String.Format("Tax = ${0}", results.Tax.ToString("0.00")));
                        Console.WriteLine(String.Format("Discount = (${0})", results.Discount.ToString("0.00")));
                        Console.WriteLine(String.Format("Total = ${0}", results.Total.ToString("0.00")));
                    }
                }
                else
                {
                    Console.WriteLine("Could not read input files. Please verify their location is correct and the file format is correct.");
                }
            }

            Console.ReadLine();
        }

        #region Private Methods

        private static bool ParseCommandArguments(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No parameters passed. Use -i for ingredients file and -r for recipe file.");

                return false;
            }

            //Fetch the arguments and apply them to input and output properties
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-i")
                {
                    _ingredientsFile = args[i + 1];
                }

                if (args[i] == "-r")
                {
                    _recipeFile = args[i + 1];
                }
            }

            if (String.IsNullOrEmpty(_ingredientsFile) == true || String.IsNullOrEmpty(_recipeFile))
            {
                Console.WriteLine("Missing parameters. Use -i for ingredients file and -r for recipe file.");

                return false;
            }

            return true;
        }

        #endregion
    }
}
