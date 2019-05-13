using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client
{
    public class FoodAndMealClient
    {
        private readonly GraphQLClient _graphQLClient;

        public FoodAndMealClient(GraphQLClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
        }

        public async Task GetMeal(int id)
        {
            Console.WriteLine($"Getting meal with id = {id}");
            var query = new GraphQLRequest
            {
                Query = @"
                query mealQuery($id: Int!) {
                    meal(id: $id) {
                        name
                        instructions
                        servingSize
                        ingredients {
                            ingredient {
                                name
                            }
                            quantity {
                                unit
                                value
                            }
                        }
                    }
                }",
                Variables = new { id = id }
            };
            var response = await _graphQLClient.PostAsync(query);
            PrintResponse(response);
        }

        private void PrintResponse(GraphQLResponse response)
        {
            if (response.Errors != null)
            {
                Console.WriteLine("Error returned from GraphQL request:");
                foreach (var error in response.Errors)
                {
                    Console.WriteLine(error.Message);
                }
            }
            Console.WriteLine("Data:");
            Console.WriteLine(response.Data);
        }
    }
}
