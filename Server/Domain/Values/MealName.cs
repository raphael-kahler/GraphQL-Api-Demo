using Functional;

namespace FoodAndMeals.Domain.Values
{
    public class MealName
    {
        public string Name { get; }

        private MealName(string name) => Name = name;

        public static Result<MealName> CreateFrom(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new ErrorMessage("Meal name cannot be null or empty.");
            }

            name = name.Trim();

            if (name.Length > 100)
            {
                return new ErrorMessage("Meal name can be at most 100 characters long.");
            }

            return new MealName(name);
        }
    }
}
