using FoodAndMeals.Framework;
using Functional;
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
                    return converter.ToDomainModel(mealInput)
                        .Map(meal => service.TryAddMeal(meal))
                        .OnSuccess(meal => messageService.SendMealAddedMessage(meal))
                        .Reduce(e =>
                        {
                            context.Errors.Add(new ExecutionError(e));
                            return null;
                        });
                }
            );

            Field<IngredientType>(
                name: "createIngredient",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IngredientInputType>> { Name = "ingredient" }),
                resolve: context =>
                {
                    var ingredientInput = context.GetArgument<IngredientInput>("ingredient");
                    return converter.ToDomainModel(ingredientInput)
                        .Map(ingredient => service.TryAddIngredient(ingredient))
                        .Reduce(e =>
                        {
                            context.Errors.Add(new ExecutionError(e));
                            return null;
                        });
                }
            );
        }
    }
}
