using FoodAndMeals.Domain;
using FoodAndMeals.Domain.Values;
using FoodAndMeals.Framework;
using Functional;
using GraphQL.DataLoader;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLTest.GraphQL.Types
{
    public class MealIngredientType : ObjectGraphType<MealIngredient>
    {
        public MealIngredientType(IMealApplicationService service, IDataLoaderContextAccessor dataLoaderAccessor)
        {
            Field(mi => mi.Preparation).Description("How to prepare the ingredient.");

            Field<IngredientType>(
                name: "Ingredient",
                description: "The ingredient details.",
                resolve: context =>
                {
                    // The batch loader will first gather all the names of ingredients that should get fetched and then make one
                    // batch request to get all the ingredients at once.
                    // This way each required ingredient only gets fetched a single time.
                    var loader = dataLoaderAccessor.Context.GetOrAddBatchLoader<string, Ingredient>("GetIngredientByName", (names, c) =>
                    {
                        // Method that does the batch lookup of ingredients. Since the persistence layer doesn't support
                        // querying multiple ingredients at once we have to query one ingredient at a time.
                        IDictionary<string, Ingredient> result = names
                                .Select((name) => service.TryGetIngredient(name))
                                .Flatten()
                                .ToDictionary(m => m.Id.Name);
                        return Task.FromResult(result);
                    });

                    return loader.LoadAsync(context.Source.Ingredient.Name);
                }
            );

            Field<QuantityType>(
                name: "Quantity",
                description: "The quantity of the ingredient.",
                resolve: context => context.Source.Quantity
            );
        }
    }
}
