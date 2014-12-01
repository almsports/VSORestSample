using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSample.DataModel
{
    public class WorkItemDefinition
    {
        [JsonProperty(PropertyName = "id")]
        public String ID { get; set; }

        [JsonProperty(PropertyName = "rev")]
        public int Rev { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public WorkItemFields Fields { get; set; }

        [JsonProperty(PropertyName = "url")]
        public String Url { get; set; }
    }

    public class WorkItemFields
    {
        [JsonProperty(PropertyName = "System.AreaPath")]
        public String AreaPath { get; set; }

        [JsonProperty(PropertyName = "System.TeamProject")]
        public String TeamProject { get; set; }

        [JsonProperty(PropertyName = "System.IterationPath")]
        public String IterationPath { get; set; }

        [JsonProperty(PropertyName = "System.WorkItemType")]
        public String WorkItemType { get; set; }

        [JsonProperty(PropertyName = "System.State")]
        public String State { get; set; }

        [JsonProperty(PropertyName = "System.Reason")]
        public String Reason { get; set; }

        [JsonProperty(PropertyName = "System.CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "System.CreatedBy")]
        public String CreatedBy { get; set; }

        [JsonProperty(PropertyName = "System.ChangedDate")]
        public DateTime ChangedDate { get; set; }

        [JsonProperty(PropertyName = "System.ChangedBy")]
        public String ChangedBy { get; set; }

        [JsonProperty(PropertyName = "System.Title")]
        public String Title { get; set; }

        [JsonProperty(PropertyName = "Microsoft.VSTS.Scheduling.RemainingWork")]
        public int RemainingWork { get; set; }

        [JsonProperty(PropertyName = "System.Description")]
        public String Description { get; set; }
    }
}
