using System;
using System.Linq;
using VeganPlanner.Models;

namespace VeganPlanner.Data
{
    public class DbInitializer
    {
        public static void Initialize(VeganPlannerContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Item.Any())
            {

                var recipes = new Recipe[]
               {
                new Recipe{Notes = "Easy, protein-packed pizza crust!", Servings = 5}
               };
                foreach (Recipe r in recipes)
                {
                    context.Recipe.Add(r);
                }
                context.SaveChanges();

                var items = new Item[]
                {
                new Item {Name = "Red Lentil Pizza Crust",
                         IsRecipe = true,
                         RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID,
                         recipe = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!"),
                         ServingSize = 1,
                         ServingUnits = "Crust",
                         Category = "Protein",
                         UserID = "lvolta@umich.edu",
                         CaloriesPerServing = 100,
                         ProteinPerServing = 17,
                         IsGF = true,
                         IsPantryItem = false,
                },
                new Item {Name = "Red Lentils",
                    IsRecipe = false,
                    ServingSize = 0.5M,
                    ServingUnits = "Cup",
                    Category = "Raw Protein",
                    UserID = "lvolta@umich.edu",
                    CaloriesPerServing = 20,
                    ProteinPerServing = 12,
                    IsGF = true,
                    IsPantryItem = true
                },
                new Item {Name = "Rice Milk",
                    IsRecipe = false,
                    ServingSize = 1,
                    ServingUnits = "Cup",
                    Category = "Milk/Creamer",
                    UserID = "lvolta@umich.edu",
                    CaloriesPerServing = 10,
                    ProteinPerServing = 2,
                    IsGF = true,
                    IsPantryItem = false
                }
                };

                foreach (Item i in items)
                {
                    context.Item.Add(i);
                }
                context.SaveChanges();

                var ingredients = new Ingredient[]
                 {
                new Ingredient{ItemID = items.Single(i => i.Name == "Red Lentils").ItemID,
                    item = items.Single(i => i.Name == "Red Lentils"),
                    RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID,
                    Quantity = 2,
                    Units = "Cup"
                },
                new Ingredient{ItemID = items.Single(i => i.Name == "Rice Milk").ItemID,
                    item = items.Single(i => i.Name == "Rice Milk"),
                    RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID,
                    Quantity = 2.5M,
                    Units = "Cup"
                }
                 };

                foreach (Ingredient ing in ingredients)
                {
                    context.Ingredient.Add(ing);
                }
                context.SaveChanges();

                var steps = new Step[]
                {
                new Step{Description="Preheat large non-stick pan over medium heat.", Order = 0, RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID},
                new Step{Description="Place ingredients in a high-speed blender", Order = 1, RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID},
                new Step{Description="Begin blending on lowest setting and slowly turn up to highest setting. Blend on high for 40 seconds.", Order = 2, RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID},
                new Step{Description="Pour 2/3 cup batter at a time into heated pan.", Order = 3, RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID},
                new Step{Description="Cover pan with lid slightly askew and cook for about 3 minutes.", Order = 4, RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID},
                new Step{Description="Flip crust and cook for another 2 minutes.", Order = 5, RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID},
                new Step{Description="Repeat until batter is finished", Order = 6,RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID},
                new Step{Description="Preheat oven to 450 and prepare pizzas with your favorite toppings", Order = 7, RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID},
                new Step{Description="Bake about 8 minutes or until crust edges begin to brown. Enjoy!", Order = 8, RecipeID = recipes.Single(r => r.Notes == "Easy, protein-packed pizza crust!").RecipeID}
                };

                foreach (Step s in steps)
                {
                    context.Step.Add(s);
                }

                context.SaveChanges();
            }
            if (context.Meal.Any()) { return;}

            //Seed Meal Data

            var meals = new Meal[]
            {
                //new Meal{NickName = "Thai", CaloriesPerServing = 500, ProteinPerServing = 20, Notes = "Recipes from Mai Kaidee's Cooking Class in Bangkok!", UserID = "lvolta@umich.edu"},
                new Meal{NickName = "Guilt-Free Pizza", CaloriesPerServing = 600, ProteinPerServing = 30, Notes = "Good for a quick and easy weeknight dinner.", UserID = "lvolta@umich.edu"}
            };

            foreach(Meal m in meals)
            {
                context.Meal.Add(m);
            }

            context.SaveChanges();

            var meal_components = new MealComponent[]
            {
                new MealComponent{MealID = meals.Single(m => m.NickName == "Guilt-Free Pizza").MealID, FoodItemID = context.Item.Single(i => i.Name == "Red Lentil Pizza Crust").ItemID, FoodItem = context.Item.Single(i => i.Name == "Red Lentil Pizza Crust")},
            };

            foreach(MealComponent m in meal_components)
            {
                context.MealComponent.Add(m);
            }

            context.SaveChanges();

        }
    }
}
