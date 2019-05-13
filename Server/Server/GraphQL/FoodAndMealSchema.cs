using GraphQL;
using GraphQL.Types;

namespace GraphQLTest.GraphQL
{
    public class FoodAndMealSchema : Schema
    {
        public FoodAndMealSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<FoodAndMealQuery>();
            Mutation = resolver.Resolve<FoodAndMealMutation>();
            Subscription = resolver.Resolve<FoodAndMealSubscription>();
        }
    }
}
