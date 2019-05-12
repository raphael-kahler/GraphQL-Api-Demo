using GraphQL.Types;

namespace GraphQLTest.GraphQL.Types.Input
{
    public class IngredientInputType : InputObjectGraphType
    {
        public IngredientInputType()
        {
            Name = nameof(IngredientInput);
            Description = "The details of the ingredient that should be created.";
            Field<NonNullGraphType<StringGraphType>>(nameof(IngredientInput.Name), "The name of the ingredient.");
            Field<NonNullGraphType<StringGraphType>>(nameof(IngredientInput.Description), "A description of the ingredient.");
            Field<StringGraphType>(nameof(IngredientInput.ImageUrl), "A url to an image of the ingredient.");
        }
    }

    public class IngredientInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
