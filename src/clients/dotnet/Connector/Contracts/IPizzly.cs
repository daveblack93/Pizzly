using System.Threading.Tasks;
using Connector.Entities;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace Connector.Contracts
{
    public interface IPizzly
    {
        Task<IFlurlResponse> CreateConfiguration(ConfigurationSetup setup);
        Task<IFlurlResponse> UpdateConfiguration(string setupId, ConfigurationSetup setup);
        Task<IFlurlResponse> DeleteConfiguration(string setupId);
        Task<string> GetAuthenticationAsync(string setupId);
        Task<JObject> GetDatasAsync(string endpoint, string authId);
    }
}
