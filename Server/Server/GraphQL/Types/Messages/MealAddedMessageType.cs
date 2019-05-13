using GraphQL.Types;
using Server.GraphQL.Messaging;

namespace Server.GraphQL.Types.Messages
{
    public class MealAddedMessageType : ObjectGraphType<MealAddedMessage>
    {
        public MealAddedMessageType()
        {
            Field<IntGraphType>("Id", "The ID of the meal.", resolve: context => context.Source.Id);
            Field<StringGraphType>("Name", "The name of the meal.", resolve: context => context.Source.Name);
        }
    }
}
