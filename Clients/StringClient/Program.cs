using GraphQL.Client;
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
                var graphQLClient = new GraphQLClient("https://localhost:44364/graphql");
                var client = new FoodAndMealClient(graphQLClient);
                await client.GetMeal(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
