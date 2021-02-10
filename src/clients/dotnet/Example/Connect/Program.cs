using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Connector.Entities;
using Connector.Services;
using Newtonsoft.Json;

namespace Connect
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var pizzly = new Pizzly("http://localhost:8080", "github");

            var setUp = new ConfigurationSetup()
            {
                Key = "505dee414c575e811c09",
                Secret = "458ddfd5b9c978d3ec3a6a37d46693509e8c0d7a",
                Scopes = new List<string>()
                {
                    "testPizzlyNetCore",
                    "ciaoMondo"
                }
            };

            //var createConfiguration = pizzly.CreateConfiguration(setUp);
            //var updateConfiguration = await pizzly.UpdateConfiguration("c9209e49-be0a-449f-a046-f0d28fe1d644", setUp).ConfigureAwait(false);

            //if (updateConfiguration.StatusCode.Equals(200))
            //{
            //    Console.WriteLine("Success");
            //}

            var authenticationId = await pizzly.GetAuthenticationAsync("074a1934-41e6-49d7-9cd4-30ac5eb19601").ConfigureAwait(false);

            Console.WriteLine($"Auth: {authenticationId}");

            var userObject = await pizzly.GetDatasAsync("user", authenticationId).ConfigureAwait(false);

            Console.WriteLine($"User: {JsonConvert.SerializeObject(userObject)}");

            Console.ReadLine();
        }
    }
}
