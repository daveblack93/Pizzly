using System.Collections.Generic;

namespace Connector.Entities
{
    public class ConfigurationSetup
    {
        public string Key { get; set; }
        public string Secret { get; set; }
        public IList<string> Scopes { get; set; }
    }
}
