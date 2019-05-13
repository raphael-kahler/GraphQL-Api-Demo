using GraphQL.Resolvers;
using GraphQL.Types;
using Server.GraphQL.Messaging;
using Server.GraphQL.Types.Messages;

namespace GraphQLTest.GraphQL
{
    public class FoodAndMealSubscription : ObjectGraphType
    {
        public FoodAndMealSubscription(MealMessageService messageService)
        {
            Name = "mealSubscription";
            AddField(new EventStreamFieldType
            {
                Name = "mealAdded",
                Type = typeof(MealAddedMessageType),
                Resolver = new FuncFieldResolver<MealAddedMessage>(c => c.Source as MealAddedMessage),
                Subscriber = new EventStreamResolver<MealAddedMessage>(c => messageService.GetMessages())
            });
        }
    }
}
