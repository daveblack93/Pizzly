using System.Collections.Generic;

namespace PizzlyConnector.Entities
{
    public class ConfigurationSetup : Credential
    {
        public IList<string> Scopes { get; set; }
    }
}
