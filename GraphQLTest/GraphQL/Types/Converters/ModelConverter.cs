using System;
using System.Collections.Generic;
using FoodAndMeals.Domain;
using FoodAndMeals.Domain.Values;
using GraphQLTest.GraphQL.Types.Input;

namespace GraphQLTest.GraphQL.Types.Converters
{
    public class ModelConverter
    {
        public Meal ToDomainModel(MealInput mealInput)
        {
            return new Meal
            (
                id: 0,
                mealName: new MealName(mealInput.Name),
                ingredients: ToModel(mealInput.MealIngredients),
                servingSize: new ServingSize(mealInput.FeedsNumPeople),
                instructions: new CookingInstructions(mealInput.Instructions)
            );
        }

        private IList<MealIngredient> ToModel(IEnumerable<MealIngredientInput> mealIngredients)
        {
            var models = new List<MealIngredient>();

            foreach (var mealIngredient in mealIngredients)
            {
                models.Add(new MealIngredient
                    (
                        ingredient: new IngredientId(mealIngredient.Ingredient),
                        quantity: new Quantity(Unit.Parse(mealIngredient.Unit), mealIngredient.Quantity),
                        preparation: mealIngredient.Preparation
                    )
                );
            }

            return models;
        }

        public Ingredient ToDomainModel(IngredientInput ingredientInput)
        {
            var uri = null != ingredientInput.ImageUrl ? new ImageUri(ingredientInput.ImageUrl) : null;
            return new Ingredient
            (
                id: new IngredientId(ingredientInput.Name),
                description: ingredientInput.Description,
                image: uri
            );
        }
    }
}
