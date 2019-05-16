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

            var flour = new Ingredient(IngredientId.CreateFrom("flour").Reduce(whenError: null), "bake it", ImageUri.CreateFrom("https://upload.wikimedia.org/wikipedia/commons/6/64/All-Purpose_Flour_%284107895947%29.jpg").Reduce(whenError: null));
            var water = new Ingredient(IngredientId.CreateFrom("water").Reduce(whenError: null), "drink it", ImageUri.CreateFrom("https://upload.wikimedia.org/wikipedia/commons/2/24/Cat_drinking_water_%28ubt%29.jpeg").Reduce(whenError: null));
            var sugar = new Ingredient(IngredientId.CreateFrom("sugar").Reduce(whenError: null), "eat it");
            var yeast = new Ingredient(IngredientId.CreateFrom("yeast").Reduce(whenError: null), "bake it");
            var chiliPeppers = new Ingredient(IngredientId.CreateFrom("chili peppers").Reduce(whenError: null), "cook it");
            var beans = new Ingredient(IngredientId.CreateFrom("beans").Reduce(whenError: null), "cook it");
            var tomatoes = new Ingredient(IngredientId.CreateFrom("tomatoes").Reduce(whenError: null), "cook it");
            var groundBeef = new Ingredient(IngredientId.CreateFrom("ground beef").Reduce(whenError: null), "85% lean");

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
                new MealIngredient(IngredientId.CreateFrom("flour").Reduce(whenError: null), new Quantity(Unit.Grams, 500), "fluffed"),
                new MealIngredient(IngredientId.CreateFrom("water").Reduce(whenError: null), new Quantity(Unit.Milliliters, 100)),
                new MealIngredient(IngredientId.CreateFrom("yeast").Reduce(whenError: null), new Quantity(Unit.Grams, 10))
            };
            var bread = new Meal(1, MealName.CreateFrom("Bread").Reduce(whenError: null), breadIngredients, new CookingInstructions("bake it"), new ServingSize(4));

            var chiliIngredients = new List<MealIngredient>
            {
                new MealIngredient(IngredientId.CreateFrom("chili peppers").Reduce(whenError: null), new Quantity(Unit.Kilograms, 10), "chopped"),
                new MealIngredient(IngredientId.CreateFrom("beans").Reduce(whenError: null), new Quantity(Unit.Kilograms, 10), "rinsed and drained"),
                new MealIngredient(IngredientId.CreateFrom("tomatoes").Reduce(whenError: null), new Quantity(Unit.Kilograms, 10), "chopped"),
                new MealIngredient(IngredientId.CreateFrom("ground beef").Reduce(whenError: null), new Quantity(Unit.Kilograms, 10)),
                new MealIngredient(IngredientId.CreateFrom("water").Reduce(whenError: null), new Quantity(Unit.Liters, 10))
            };
            var chili = new Meal(2, MealName.CreateFrom("Chili").Reduce(whenError: null), chiliIngredients, new CookingInstructions("cook it"), new ServingSize(60));

            var cookieIngredients = new List<MealIngredient>
            {
                new MealIngredient(IngredientId.CreateFrom("flour").Reduce(whenError: null), new Quantity(Unit.Grams, 500), "fluffed"),
                new MealIngredient(IngredientId.CreateFrom("sugar").Reduce(whenError: null), new Quantity(Unit.Grams, 500))
            };
            var cookies = new Meal(3, MealName.CreateFrom("Cookies").Reduce(whenError: null), cookieIngredients, new CookingInstructions("bake 'em"), new ServingSize(4));

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

        public Result<Meal> TryAddMeal(Meal meal)
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
                    return meal;
                }
                return new ErrorMessage($"Meal with id {nextId} already exists");
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


        public Result<Ingredient> TryAddIngredient(Ingredient ingredient)
        {
            var added = _ingredients.TryAdd(ingredient.Id.Name, ingredient);
            if (added)
            {
                Interlocked.Increment(ref _numIngredients);
                return ingredient;
            }
            return new ErrorMessage($"Ingredient with name \"{ingredient.Id.Name}\" already exists");
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
