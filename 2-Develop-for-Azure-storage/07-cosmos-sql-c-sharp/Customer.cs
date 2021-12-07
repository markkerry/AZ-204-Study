using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cosmosDotNet
{
    class Customer
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "customername")]
        public string Customername { get; set; }
        [JsonProperty(PropertyName = "customercity")]
        public string Customercity { get; set; }
    }
}
