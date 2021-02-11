using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PizzlyConnector.Entities;
using PizzlyConnector.Services;
using Newtonsoft.Json;

namespace ConsoleApp.Connect
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var pizzly = new Pizzly("http://localhost:8080", "github");
            await pizzly.SetAuthenticationAsync("5b41eb73-5dce-493a-8bfc-1ebc020d740a").ConfigureAwait(false);

            //var setUp = new ConfigurationSetup()
            //{
            //    ClientId = "505dee414c575e811c09",
            //    ClientSecret = "458ddfd5b9c978d3ec3a6a37d46693509e8c0d7a",
            //    Scopes = new List<string>()
            //    {
            //        "user_profile"
            //    }
            //};

            //var configurationCreated = await pizzly.CreateConfiguration(setUp).ConfigureAwait(false);
            //var updateConfiguration = await pizzly.UpdateConfiguration("8c1d1f66-c26b-40e9-a95f-8b7e1eb6e5ab", setUp).ConfigureAwait(false);
            //var authenticationId = await pizzly.GetAuthenticationAsync(configurationCreated.Configuration.Setup_Id).ConfigureAwait(false);
            //Console.WriteLine($"Auth: {authenticationId}");

            var userObject = await pizzly.GetDatasAsync("user").ConfigureAwait(false);

            Console.WriteLine($"User: {JsonConvert.SerializeObject(userObject)}");

            Console.ReadLine();
        }
    }
}
