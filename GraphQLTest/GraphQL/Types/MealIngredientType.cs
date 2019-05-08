using FoodAndMeals.Domain.Values;
using FoodAndMeals.Framework;
using GraphQL.Types;

namespace GraphQLTest.GraphQL.Types
{
    public class MealIngredientType : ObjectGraphType<MealIngredient>
    {
        public MealIngredientType(IMealApplicationService service)
        {
            Field(mi => mi.Preparation).Description("How to prepare the ingredient.");

            Field<IngredientType>(
                name: "Ingredient",
                description: "The ingredient details.",
                resolve: context => { service.TryGetIngredient(context.Source.Ingredient.Name, out var ingredient); return ingredient; }
            );

            Field<QuantityType>(
                name: "Quantity",
                description: "The quantity of the ingredient.",
                resolve: context => context.Source.Quantity
            );
        }
    }
}
