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
                var quantity = new Quantity(Unit.Parse(mealIngredient.Unit), mealIngredient.Quantity);
                IngredientId
                    .CreateFrom(mealIngredient.Ingredient)
                    .Map<IngredientId, MealIngredient>(ingredientId => new MealIngredient(ingredientId, quantity, mealIngredient.Preparation))
                    .OnSuccess(meal => models.Add(meal));
            }

            return models;
        }

        public Result<Ingredient> ToDomainModel(IngredientInput ingredientInput)
        {
            Result<ImageUri> uriResult = null != ingredientInput.ImageUrl ? ImageUri.CreateFrom(ingredientInput.ImageUrl) : new Success<ImageUri>(null);

            return uriResult
                .And(IngredientId.CreateFrom(ingredientInput.Name))
                .Map<(ImageUri, IngredientId), Ingredient>(((ImageUri uri, IngredientId ingredientId) parts) =>
                    new Ingredient(
                        id: parts.ingredientId,
                        description: ingredientInput.Description,
                        image: parts.uri
                    ));
        }
    }
}
