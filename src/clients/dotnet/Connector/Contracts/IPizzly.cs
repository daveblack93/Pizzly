using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using PizzlyConnector.Entities;

namespace PizzlyConnector.Contracts
{
    public interface IPizzly
    {
        Task<ConfigurationResponse> CreateConfiguration(ConfigurationSetup setup);
        Task<ConfigurationResponse> UpdateConfiguration(string setupId, ConfigurationSetup setup);
        Task<IFlurlResponse> DeleteConfiguration(string setupId);
        Task<string> GetAuthenticationAsync(string setupId);
        Task<JObject> GetDatasAsync(string endpoint);
    }
}
