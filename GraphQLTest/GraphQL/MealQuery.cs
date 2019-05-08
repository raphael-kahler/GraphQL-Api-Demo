using FoodAndMeals.Framework;
using GraphQL.Types;
using GraphQLTest.GraphQL.Types;

namespace GraphQLTest.GraphQL
{
    public class FoodAndMealQuery : ObjectGraphType
    {
        public FoodAndMealQuery(IMealApplicationService service)
        {
            Field<ListGraphType<MealType>>(
                name: "meals",
                description: "Get details about meals",
                resolve: context => service.GetMeals(10, 0, out _)
            );

            Field<MealType>(
                name: "meal",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: context => { service.TryGetMeal(context.GetArgument<int>("id"), out var meal); return meal; }
            );
        }
    }
}
