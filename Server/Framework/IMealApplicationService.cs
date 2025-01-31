﻿using FoodAndMeals.Domain;
using Functional;
using System.Collections.Generic;

namespace FoodAndMeals.Framework
{
    public interface IMealApplicationService
    {
        Option<Meal> TryGetMeal(int id);
        IEnumerable<Meal> GetMeals(int count, int skip, out int totalCount);
        Result<Meal> TryAddMeal(Meal meal);
        Meal SaveMeal(Meal meal);
        bool TryDeleteMeal(int id);

        Option<Ingredient> TryGetIngredient(string name);
        IEnumerable<Ingredient> GetIngredients(int count, int skip, out int totalCount);
        Result<Ingredient> TryAddIngredient(Ingredient ingredient);
        Ingredient SaveIngredient(Ingredient ingredient);
        bool TryDeleteIngredient(string name);
    }
}