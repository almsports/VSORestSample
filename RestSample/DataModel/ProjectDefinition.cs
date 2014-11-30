using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSample.DataModel
{
    public class ProjectDefinition
    {

        [JsonProperty(PropertyName = "id")]
        public String ID { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public String URL { get; set; }

        [JsonProperty(PropertyName = "description")]
        public String Description { get; set; }

        [JsonProperty(PropertyName = "state")]
        public String State { get; set; }

        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }

        [JsonProperty(PropertyName = "defaultTeam")]
        public Team DefaultTeam { get; set; }
    }

    public class Links
    {
        [JsonProperty(PropertyName = "self")]
        public LinkTyp Self { get; set; }

        [JsonProperty(PropertyName = "collection")]
        public LinkTyp Collection { get; set; }

        [JsonProperty(PropertyName = "web")]
        public LinkTyp Web { get; set; }
    }

    public class LinkTyp
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
    }

    public class Team
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }
    }
}
