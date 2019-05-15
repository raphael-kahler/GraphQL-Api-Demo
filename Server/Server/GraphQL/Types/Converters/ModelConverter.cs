using System;
using System.Collections.Generic;
using FoodAndMeals.Domain;
using FoodAndMeals.Domain.Values;
using Functional;
using GraphQLTest.GraphQL.Types.Input;

namespace GraphQLTest.GraphQL.Types.Converters
{
    public class ModelConverter
    {
        public Result<Meal> ToDomainModel(MealInput mealInput)
        {
            return MealName
                .CreateFrom(mealInput.Name)
                .Map(mealName =>
                {
                    return new Success<Meal>(new Meal
                    (
                        id: 0,
                        mealName: mealName,
                        ingredients: ToModel(mealInput.MealIngredients),
                        servingSize: new ServingSize(mealInput.FeedsNumPeople),
                        instructions: new CookingInstructions(mealInput.Instructions)
                    ));
                });
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

        public Result<Ingredient> ToDomainModel(IngredientInput ingredientInput)
        {
            Result<ImageUri> uriResult = null != ingredientInput.ImageUrl ? ImageUri.CreateFrom(ingredientInput.ImageUrl) : new Success<ImageUri>(null);

            return uriResult.Map<ImageUri, Ingredient>(uri =>
                new Ingredient(
                    id: new IngredientId(ingredientInput.Name),
                    description: ingredientInput.Description,
                    image: uri
                )
            );
        }
    }
}
