using FoodAndMeals.Domain;
using GraphQL.Types;

namespace GraphQLTest.GraphQL.Types
{
    public class IngredientType : ObjectGraphType<Ingredient>
    {
        public IngredientType()
        {
            Field(i => i.Id.Name).Description("The name of the ingredient.");
            Field(i => i.Description).Description("A description of the ingredient.");

            Field<StringGraphType>(
                name: "Image",
                description: "The image of the ingredient",
                resolve: context => context.Source.Image?.Uri?.ToString()
            );
        }
    }
}
