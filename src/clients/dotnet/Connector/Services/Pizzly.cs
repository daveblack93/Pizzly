using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Connector.Contracts;
using Connector.Entities;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace Connector.Services
{
    public class Pizzly : IPizzly
    {
        private string _baseUrl { get; set; }
        private string _integrationId { get; set; }

        public Pizzly(string baseUrl, string integrationId)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException($"{nameof(baseUrl)} cannot be null");
            }

            _baseUrl = baseUrl;
            _integrationId = integrationId;
        }

        public async Task<IFlurlResponse> CreateConfiguration(ConfigurationSetup setup)
        {
            var url = $"dashboard/{_integrationId}/configurations/new";
            return await TriggerCreateOrUpdate(url, setup).ConfigureAwait(false);
        }

        public async Task<IFlurlResponse> UpdateConfiguration(string setupId, ConfigurationSetup setup)
        {
            var url = $"dashboard/{_integrationId}/configurations/{setupId}";
            return await TriggerCreateOrUpdate(url, setup).ConfigureAwait(false);
        }

        public async Task<IFlurlResponse> DeleteConfiguration(string setupId)
        {
            var url = $"dashboard/{_integrationId}/configurations/{setupId}";
            return await TriggerDelete(url).ConfigureAwait(false);
        }

        public async Task<string> GetAuthenticationAsync(string setupId)
        {
            var url = $"api/{_integrationId}/authentication/{setupId}";
            var authentication = await GetDatasAsync<Authentication>(url).ConfigureAwait(false);
            return authentication.Auth_Id;
        }

        public async Task<JObject> GetDatasAsync(string endpoint, string authId)
        {
            var url = $"proxy/{_integrationId}/{endpoint}";
            return await GetAuthenticatedDatasAsync<JObject>(url, authId).ConfigureAwait(false);
        }

        #region Private Methods

        private string GetScopeListFormattedForRequest(IList<string> scopes)
        {
            var scopeListFormatted = new StringBuilder();

            foreach (var scope in scopes)
            {
                scopeListFormatted.AppendLine(scope);
            }

            return scopeListFormatted.ToString();
        }

        private async Task<IFlurlResponse> TriggerCreateOrUpdate(string url, ConfigurationSetup setup)
        {
            var result = await _baseUrl.AppendPathSegment(url)
                .PostJsonAsync(new
                {
                    setupKey = setup.Key,
                    setupSecret = setup.Secret,
                    scopes = GetScopeListFormattedForRequest(setup.Scopes)
                }).ConfigureAwait(false);

            return result;
        }

        private async Task<IFlurlResponse> TriggerDelete(string url)
        {
            var result = await _baseUrl.AppendPathSegment(url)
                .DeleteAsync().ConfigureAwait(false);

            return result;
        }

        private async Task<T> GetDatasAsync<T>(string url)
        {
            var result = await _baseUrl.AppendPathSegment(url)
                .GetJsonAsync<T>().ConfigureAwait(false);

            return result;
        }

        private async Task<T> GetAuthenticatedDatasAsync<T>(string url, string authId)
        {
            var result = await _baseUrl.AppendPathSegment(url)
                .WithHeader("Pizzly-Auth-Id", authId)
                .GetJsonAsync<T>().ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
