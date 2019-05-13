using FoodAndMeals.Framework;
using GraphQL;
using GraphQL.Types;
using GraphQLTest.GraphQL.Types;
using GraphQLTest.GraphQL.Types.Converters;
using GraphQLTest.GraphQL.Types.Input;
using Server.GraphQL.Messaging;

namespace GraphQLTest.GraphQL
{
    public class FoodAndMealMutation : ObjectGraphType
    {
        public FoodAndMealMutation(IMealApplicationService service, MealMessageService messageService, ModelConverter converter)
        {
            Field<MealType>(
                name: "createMeal",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<MealInputType>> { Name = "meal" }),
                resolve: context =>
                {
                    var mealInput = context.GetArgument<MealInput>("meal");
                    var meal = converter.ToDomainModel(mealInput);
                    service.AddMeal(ref meal);
                    messageService.SendMealAddedMessage(meal);
                    return meal; 
                }
            );

            Field<IngredientType>(
                name: "createIngredient",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IngredientInputType>> { Name = "ingredient" }),
                resolve: context =>
                {
                    var ingredientInput = context.GetArgument<IngredientInput>("ingredient");
                    var ingredient = converter.ToDomainModel(ingredientInput);
                    if (service.AddIngredient(ingredient))
                    {
                        return ingredient;
                    }
                    context.Errors.Add(new ExecutionError($"Ingredient with name \"{ingredientInput.Name}\" already exists"));
                    return null;
                }
            );
        }
    }
}
