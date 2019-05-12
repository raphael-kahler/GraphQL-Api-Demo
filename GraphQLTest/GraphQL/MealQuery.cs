using FoodAndMeals.Domain;
using FoodAndMeals.Framework;
using Functional;
using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQLTest.GraphQL.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLTest.GraphQL
{
    public class FoodAndMealQuery : ObjectGraphType
    {
        public FoodAndMealQuery(IMealApplicationService service, IDataLoaderContextAccessor dataLoaderAccessor)
        {
            Field<ListGraphType<MealType>>(
                name: "meals",
                description: "Get details about meals",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "count", DefaultValue = 10 },
                    new QueryArgument<IntGraphType> { Name = "offset", DefaultValue = 0 }
                ),
                resolve: context => service.GetMeals(context.GetArgument<int>("count"), context.GetArgument<int>("offset"), out _)
            );

            Field<MealType>(
                name: "meal",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: context =>
                {
                    var loader = dataLoaderAccessor.Context.GetOrAddBatchLoader<int, Meal>("GetMealsById", (ids, c) =>
                    {
                        IDictionary<int, Meal> result = ids
                            .Select((id) => service.TryGetMeal(id))
                            .Flatten()
                            .ToDictionary(m => m.Id);
                        return Task.FromResult(result);
                    });

                    return loader.LoadAsync(context.GetArgument<int>("id"));

                    //service.TryGetMeal(context.GetArgument<int>("id"), out var meal); return meal;
                }
            );

            Field<ListGraphType<IngredientType>>(
                name: "ingredients",
                description: "Get details about ingredients",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "count", DefaultValue = 10 },
                    new QueryArgument<IntGraphType> { Name = "offset", DefaultValue = 0 }
                ),
                resolve: context => service.GetIngredients(context.GetArgument<int>("count"), context.GetArgument<int>("offset"), out _)
            );
        }
    }
}
