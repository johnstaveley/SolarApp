using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Model
{
    [Serializable]
    public class DataPoint
    {
        [JsonIgnore]
        public string _id { get; set; }

        [JsonProperty(Order = 1)]
        public Head Head { get; set; }

        [JsonProperty(Order = 2)]
        public Body Body { get; set; }

    }

    public class Head
    {
        [JsonProperty(Order=1)]
        public DateTime Timestamp { get; set; }

        [JsonProperty(Order = 2)]
        public RequestArguments RequestArguments { get; set; }

        [JsonProperty(Order = 3)]
        public Status Status { get; set; }
    }

    public class Status
    {
        [JsonProperty(Order = 1)]
        public int Code { get; set; }

        [JsonProperty(Order = 2)]
        public string Reason { get; set; }

        [JsonProperty(Order = 3)]
        public string UserMessage { get; set; }

    }

    public class RequestArguments
    {
        [JsonProperty(Order = 1)]
        public string Query { get; set; }

        [JsonProperty(Order = 2)]
        public string Scope { get; set; }
    }

    public class Body
    {
        [JsonProperty(Order = 1, PropertyName="PAC")]
        public FroniusEnergyReading CurrentReading { get; set; }

        [JsonProperty(Order = 2, PropertyName="DAY_ENERGY")]
        public FroniusEnergyReading DayEnergy { get; set; }

        [JsonProperty(Order = 3, PropertyName="YEAR_ENERGY")]
        public FroniusEnergyReading YearEnergy { get; set; }

        [JsonProperty(Order = 4, PropertyName="TOTAL_ENERGY")]
        public FroniusEnergyReading TotalEnergy { get; set; }
    }

    public class FroniusEnergyReading
    {
        [JsonProperty(Order = 1)]
        public string Unit { get; set; }

        [JsonProperty(Order = 2)]
        public FroniusEnergyReadingItem Values { get; set; }
    }

    public class FroniusEnergyReadingItem
    {

        [JsonProperty(PropertyName="1")]
        public int Value { get; set; }
    }
}
