using FoodAndMeals.Domain.Values;
using Functional;
using System;

namespace FoodAndMeals.Domain
{
    public class IngredientId
    {
        public string Name { get; }

        private IngredientId(string name) => Name = name;

        public static Result<IngredientId> CreateFrom(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new ErrorMessage("Ingredient ID name cannot be null or empty.");
            }
            if (name.Length > 200)
            {
                return new ErrorMessage("Ingredient ID name can't be longer than 200 characters");
            }
            return new IngredientId(name);
        }
    }

    public class Ingredient
    {
        public IngredientId Id { get; }

        public string Description { get; private set; }

        public ImageUri Image { get; private set; }

        public Ingredient(IngredientId id, string description, ImageUri image = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Image = image;
        }
    }
}
