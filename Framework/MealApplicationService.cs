using FoodAndMeals.Domain;
using FoodAndMeals.Domain.Values;
using Functional;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace FoodAndMeals.Framework
{
    public class MealApplicationService : IMealApplicationService
    {
        private static ConcurrentDictionary<int, Meal> _meals;
        private static ConcurrentDictionary<string, Ingredient> _ingredients;

        private static int _mealIdCounter;
        private static int _numMeals;
        private static int _numIngredients;

        static MealApplicationService()
        {
            _meals = new ConcurrentDictionary<int, Meal>();
            _ingredients = new ConcurrentDictionary<string, Ingredient>();

            var flour = new Ingredient(new IngredientId("flour"), "bake it", new ImageUri("https://upload.wikimedia.org/wikipedia/commons/6/64/All-Purpose_Flour_%284107895947%29.jpg"));
            var water = new Ingredient(new IngredientId("water"), "drink it", new ImageUri("https://upload.wikimedia.org/wikipedia/commons/2/24/Cat_drinking_water_%28ubt%29.jpeg"));
            var sugar = new Ingredient(new IngredientId("sugar"), "eat it");
            var yeast = new Ingredient(new IngredientId("yeast"), "bake it");
            var chiliPeppers = new Ingredient(new IngredientId("chili peppers"), "cook it");
            var beans = new Ingredient(new IngredientId("beans"), "cook it");
            var tomatoes = new Ingredient(new IngredientId("tomatoes"), "cook it");
            var groundBeef = new Ingredient(new IngredientId("ground beef"), "85% lean");

            _ingredients.TryAdd("flour", flour);
            _ingredients.TryAdd("water", water);
            _ingredients.TryAdd("sugar", sugar);
            _ingredients.TryAdd("yeast", yeast);
            _ingredients.TryAdd("chili peppers", chiliPeppers);
            _ingredients.TryAdd("beans", beans);
            _ingredients.TryAdd("tomatoes", tomatoes);
            _ingredients.TryAdd("ground beef", groundBeef);

            _numIngredients = _ingredients.Count;

            var breadIngredients = new List<MealIngredient>
            {
                new MealIngredient(new IngredientId("flour"), new Quantity(Unit.Grams, 500), "fluffed"),
                new MealIngredient(new IngredientId("water"), new Quantity(Unit.Milliliters, 100)),
                new MealIngredient(new IngredientId("yeast"), new Quantity(Unit.Grams, 10))
            };
            var bread = new Meal(1, new MealName("Bread"), breadIngredients, new CookingInstructions("bake it"), new ServingSize(4));

            var chiliIngredients = new List<MealIngredient>
            {
                new MealIngredient(new IngredientId("chili peppers"), new Quantity(Unit.Kilograms, 10), "chopped"),
                new MealIngredient(new IngredientId("beans"), new Quantity(Unit.Kilograms, 10), "rinsed and drained"),
                new MealIngredient(new IngredientId("tomatoes"), new Quantity(Unit.Kilograms, 10), "chopped"),
                new MealIngredient(new IngredientId("ground beef"), new Quantity(Unit.Kilograms, 10)),
                new MealIngredient(new IngredientId("water"), new Quantity(Unit.Liters, 10))
            };
            var chili = new Meal(2, new MealName("Chili"), chiliIngredients, new CookingInstructions("cook it"), new ServingSize(60));

            var cookieIngredients = new List<MealIngredient>
            {
                new MealIngredient(new IngredientId("flour"), new Quantity(Unit.Grams, 500), "fluffed"),
                new MealIngredient(new IngredientId("sugar"), new Quantity(Unit.Grams, 500))
            };
            var cookies = new Meal(3, new MealName("Cookies"), cookieIngredients, new CookingInstructions("bake 'em"), new ServingSize(4));

            _meals.TryAdd(1, bread);
            _meals.TryAdd(2, chili);
            _meals.TryAdd(3, cookies);

            _numMeals = _meals.Count;
            _mealIdCounter = _numMeals;
        }

        public IEnumerable<Meal> GetMeals(int count, int offset, out int totalCount)
        {
            totalCount = _numMeals;
            return _meals.Values.Skip(offset).Take(count);
        }

        public Option<Meal> TryGetMeal(int id)
        {
            Debug.WriteLine($"GET meal '{id}'");
            return _meals.TryGetValue(id, out var meal) ? meal : (Option<Meal>)None.Value;
        }

        public bool AddMeal(ref Meal meal)
        {
            lock (_meals)
            {
                var nextId = _mealIdCounter + 1;
                var mealToAdd = new Meal(nextId, meal.Name, meal.Ingredients, meal.Instructions, meal.ServingSize);
                var added = _meals.TryAdd(nextId, mealToAdd);
                if (added)
                {
                    Interlocked.Increment(ref _numMeals);
                    Interlocked.Increment(ref _mealIdCounter);
                    meal = mealToAdd;
                    return true;
                }
                return false;
            }
        }

        public Meal SaveMeal(Meal meal)
        {
            return _meals.AddOrUpdate(meal.Id, meal, (key, value) => meal);
        }

        public bool TryDeleteMeal(int id)
        {
            var removed = _meals.TryRemove(id, out _);
            if (removed)
            {
                Interlocked.Decrement(ref _numMeals);
            }
            return removed;
        }

        public IEnumerable<Ingredient> GetIngredients(int count, int offset, out int totalCount)
        {
            totalCount = _numIngredients;
            return _ingredients.Values.Skip(offset).Take(count);
        }

        public Option<Ingredient> TryGetIngredient(string name)
        {
            Debug.WriteLine($"GET ingredient '{name}'");
            if (_ingredients.TryGetValue(name, out var ingredient))
            {
                return ingredient;
            }
            return None.Value;
        }


        public bool AddIngredient(Ingredient ingredient)
        {
            var added = _ingredients.TryAdd(ingredient.Id.Name, ingredient);
            if (added)
            {
                Interlocked.Increment(ref _numIngredients);
                return true;
            }
            return false;
        }

        public Ingredient SaveIngredient(Ingredient ingredient)
        {
            return _ingredients.AddOrUpdate(ingredient.Id.Name, ingredient, (key, value) => ingredient);
        }

        public bool TryDeleteIngredient(string name)
        {
            var removed = _ingredients.TryRemove(name, out _);
            if (removed)
            {
                Interlocked.Decrement(ref _numIngredients);
            }
            return removed;
        }
    }
}
