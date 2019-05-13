using GraphQL.Client.Http;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var graphQLClient = new GraphQLHttpClient("https://localhost:44364/graphql");
                var client = new FoodAndMealClient(graphQLClient);

                if (args != null && args[0].Equals("subscribe"))
                {
                    await client.Subscribe();
                    Console.WriteLine("Subscribed to messages. Press any button to quit.");
                    Console.ReadKey();
                }
                else
                {
                    await client.GetMeal(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
