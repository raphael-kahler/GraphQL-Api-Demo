using GraphQL.Types;
using System.Collections.Generic;

namespace GraphQLTest.GraphQL.Types.Input
{
    public class MealInputType : InputObjectGraphType
    {
        public MealInputType()
        {
            Name = nameof(MealInput);
            Description = "The details of the meal that should be created.";
            Field<NonNullGraphType<StringGraphType>>(nameof(MealInput.Name), "The name of the meal.");
            Field<StringGraphType>(nameof(MealInput.Instructions), "The cooking instructions for the meal.");
            Field<IntGraphType>(nameof(MealInput.FeedsNumPeople), "How many people the meal feeds.");
            Field<ListGraphType<MealIngredientInputType>>(nameof(MealInput.MealIngredients), "The ingredients for the meal.");
            //Field<ListGraphType<MealIngredientInputType>>(
            //    name: nameof(MealInput.MealIngredients),
            //    description: "The ingredients for the meal.",
            //    resolve: context => context.
            //);
        }
    }

    public class MealInput
    {
        public string Name { get; set; }
        public string Instructions { get; set; }
        public int FeedsNumPeople { get; set; }
        public IEnumerable<MealIngredientInput> MealIngredients { get; set; }
    }
}
