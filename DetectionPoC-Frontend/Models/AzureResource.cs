using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace DetectionPoC_Frontend.Models
{
    public class AzureResource
    {
        public string Id { get; set; }
        public string AlertId { get; set; }
        public string ResourceId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroup { get; set; }
        public string AlertStatus { get; set; }
        public string ResourceType { get; set; }
        public string CurrentHealthStatus { get; set; }
        public string PreviousHealthStatus { get; set; }
        public DateTime EventTimestamp { get; set; }

        //public AzureResource()
        //{
        //    this.SubscriptionId = ResourceId.Split("/")[2];
        //    this.ResourceGroup = ResourceId.Split("/")[4];
        //    this.ResourceType = ResourceId.Split("/")[6];
        //    this.ResourceId = ResourceId.Split("/")[8];
        //}
    }
}
