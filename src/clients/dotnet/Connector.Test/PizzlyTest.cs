using System;
using NUnit.Framework;
using PizzlyConnector.Entities;
using PizzlyConnector.Services;

namespace Connector.Test
{
    public class PizzlyTest
    {
        private Pizzly _pizzly;

        [SetUp]
        public void Setup()
        {
            var host = "http://localhost:8080";
            var integrationId = "Github";
            var authId = Guid.NewGuid().ToString();

            _pizzly = new Pizzly(host, integrationId, authId);
        }

        [Test]
        public void Create_Pizzly_Null_Configuration()
        {
            var configurationSetup = new ConfigurationSetup();

            Assert.ThrowsAsync<ArgumentNullException>(() => _pizzly.CreateConfiguration(configurationSetup));
        }

        [Test]
        public void Update_Pizzly_With_SetupId_Null()
        {
            var setupId = string.Empty;
            var configurationSetup = new ConfigurationSetup();

            Assert.ThrowsAsync<ArgumentNullException>(() => _pizzly.UpdateConfiguration(setupId, configurationSetup));
        }

        [Test]
        public void Update_Pizzly_With_ConfigurationSetup_Null()
        {
            var setupId = "Github";
            var configurationSetup = new ConfigurationSetup();

            Assert.ThrowsAsync<ArgumentNullException>(() => _pizzly.UpdateConfiguration(setupId, configurationSetup));
        }

        [Test]
        public void Delete_Pizzly_With_SetupId_Null()
        {
            var setupId = string.Empty;

            Assert.ThrowsAsync<ArgumentNullException>(() => _pizzly.DeleteConfiguration(setupId));
        }

        [Test]
        public void Get_AuthenticationId_With_Null_SetupId()
        {
            var setupId = string.Empty;

            Assert.ThrowsAsync<ArgumentNullException>(() => _pizzly.GetAuthenticationAsync(setupId));
        }
    }
}