using FoodAndMeals.Domain;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Server.GraphQL.Messaging
{
    public class MealMessageService
    {
        private readonly ISubject<MealAddedMessage> _messageStream = new ReplaySubject<MealAddedMessage>(1);

        public MealAddedMessage SendMealAddedMessage(Meal meal)
        {
            var message = new MealAddedMessage
            {
                Id = meal.Id,
                Name = meal.Name.Name
            };
            _messageStream.OnNext(message);
            return message;
        }

        public IObservable<MealAddedMessage> GetMessages()
        {
            return _messageStream.AsObservable();
        }
    }
}
