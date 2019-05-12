using GraphQL.Types;

namespace GraphQLTest.GraphQL.Types.Input
{
    public class MealIngredientInputType : InputObjectGraphType
    {
        public MealIngredientInputType()
        {
            Name = nameof(MealIngredientInput);
            Description = "The details of the ingredient that should be created.";
            Field<NonNullGraphType<StringGraphType>>(nameof(MealIngredientInput.Ingredient), "The name of the ingredient.");
            Field<NonNullGraphType<StringGraphType>>(nameof(MealIngredientInput.Unit), "The unit of the ingredient.");
            Field<NonNullGraphType<FloatGraphType>>(nameof(MealIngredientInput.Quantity), "The quantity of the ingredient.");
            Field<StringGraphType>(nameof(MealIngredientInput.Preparation), "How the ingredient should be prepared.");
        }
    }

    public class MealIngredientInput
    {
        /// <summary>
        /// The ingredient name.
        /// </summary>
        public string Ingredient { get; set; }

        /// <summary>
        /// The unit of the ingredient.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// The quantity of the ingredient.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// How the ingredient should be prepared.
        /// </summary>
        public string Preparation { get; set; }
    }
}