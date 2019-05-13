using FoodAndMeals.Domain.Values;

namespace FoodAndMeals.Domain
{
    public interface IUnitConverter
    {
        bool CanConvert(Unit from, Unit to, out double conversionRatio);
    }
}