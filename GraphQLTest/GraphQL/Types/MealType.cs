using FoodAndMeals.Domain;
using FoodAndMeals.Framework;
using GraphQL.Types;

namespace GraphQLTest.GraphQL.Types
{
    public class MealType : ObjectGraphType<Meal>
    {
        public MealType(IMealApplicationService service)
        {
            Field<IntGraphType>("Id", "The ID of the meal.", resolve: context => context.Source.Id);
            Field<StringGraphType>("Name", "The name of the meal.", resolve: context => context.Source.Name.Name);
            Field<StringGraphType>("Instructions", "The cooking instructions for the meal.", resolve: context => context.Source.Instructions.Text);
            Field<IntGraphType>("ServingSize", "The serving size of the meal.", resolve: context => context.Source.ServingSize.FeedsNumPeople);
            Field<ListGraphType<MealIngredientType>>(name: "Ingredients", description: "The ingredients of the meal.", resolve: context => context.Source.Ingredients);
        }
    }
}
