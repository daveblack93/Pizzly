using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using PizzlyConnector.Contracts;
using PizzlyConnector.Entities;

namespace PizzlyConnector.Services
{
    public class Pizzly : IPizzly
    {
        private string _baseUrl { get; set; }
        private string _integrationId { get; set; }
        private string _authId { get; set; }

        public Pizzly(string baseUrl, string integrationId, string authenticationId)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException($"{nameof(baseUrl)} cannot be null");
            }
            if (string.IsNullOrWhiteSpace(integrationId))
            {
                throw new ArgumentNullException($"{nameof(integrationId)} cannot be null");
            }

            _baseUrl = baseUrl;
            _integrationId = integrationId;
            _authId = authenticationId;
        }

        public async Task<ConfigurationResponse> CreateConfiguration(ConfigurationSetup setup)
        {
            if (setup == null)
            {
                throw new ArgumentNullException($"{nameof(ConfigurationSetup)} must me not null");
            }

            var url = $"api/{_integrationId}/configurations";
            return await TriggerCreateOrUpdateConfiguration(url, setup, true).ConfigureAwait(false);
        }

        public async Task<ConfigurationResponse> UpdateConfiguration(string setupId, ConfigurationSetup setup)
        {
            if (string.IsNullOrWhiteSpace(setupId))
            {
                throw new ArgumentNullException($"{nameof(setupId)} must be not null or empty");
            }
            if (setup == null)
            {
                throw new ArgumentNullException($"{nameof(ConfigurationSetup)} must me not null");
            }

            var url = $"api/{_integrationId}/configurations/{setupId}";
            return await TriggerCreateOrUpdateConfiguration(url, setup, false).ConfigureAwait(false);
        }

        public async Task<IFlurlResponse> DeleteConfiguration(string setupId)
        {
            if (string.IsNullOrWhiteSpace(setupId))
            {
                throw new ArgumentNullException($"{nameof(setupId)} must be not null or empty");
            }

            var url = $"api/{_integrationId}/configurations/{setupId}";
            return await TriggerDelete(url).ConfigureAwait(false);
        }

        public async Task<string> GetAuthenticationAsync(string setupId)
        {
            if (string.IsNullOrWhiteSpace(setupId))
            {
                throw new ArgumentNullException($"{nameof(setupId)} must be not null or empty");
            }

            var url = $"api/{_integrationId}/authentication/{setupId}";
            var authentication = await GetDatasAsync<Authentication>(url).ConfigureAwait(false);
            return authentication.Auth_Id;
        }

        public async Task SetAuthenticationAsync(string setupId)
        {
            _authId = await GetAuthenticationAsync(setupId).ConfigureAwait(false);
        }

        public async Task<JObject> GetDatasAsync(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(_authId))
            {
                throw new ArgumentNullException("You have to set the authid first");
            }
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException($"{nameof(endpoint)} must be not null or empty");
            }

            var url = $"proxy/{_integrationId}/{endpoint}";
            return await GetAuthenticatedDatasAsync<JObject>(url, _authId).ConfigureAwait(false);
        }

        #region Private Methods

        private string[] GetScopeListFormattedForRequest(IList<string> scopes)
        {
            return scopes.ToArray();
        }

        private async Task<ConfigurationResponse> TriggerCreateOrUpdateConfiguration(string url, ConfigurationSetup setup, bool isCreate)
        {
            object body = new
            {
                credentials = new
                {
                    clientId = setup.ClientId,
                    clientSecret = setup.ClientSecret
                },
                scopes = GetScopeListFormattedForRequest(setup.Scopes)
            };

            ConfigurationResponse result;
            if (isCreate)
            {
                result = await _baseUrl.AppendPathSegment(url)
                    .PostJsonAsync(body)
                    .ReceiveJson<ConfigurationResponse>().ConfigureAwait(false);
            }
            else
            {
                result = await _baseUrl.AppendPathSegment(url)
                   .PutJsonAsync(body)
                   .ReceiveJson<ConfigurationResponse>().ConfigureAwait(false);
            }

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
