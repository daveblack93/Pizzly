using System.Collections.Generic;

namespace PizzlyConnector.Entities
{
    public class ConfigurationBody
    {
        public string Id { get; set; }
        public string Setup_Id { get; set; }
        public string Object { get; set; }
        public IList<string> Scopes { get; set; }
        public Credential Credentials { get; set; }
    }
}
