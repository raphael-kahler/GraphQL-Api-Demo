using FoodAndMeals.Domain.Values;
using GraphQL.Types;

namespace GraphQLTest.GraphQL.Types
{
    public class QuantityType : ObjectGraphType<Quantity>
    {
        public QuantityType()
        {
            Field<StringGraphType>("Unit", resolve: context => context.Source.Unit.Name);
            Field<IntGraphType>("Value", resolve: context => context.Source.Value);
        }
    }
}
