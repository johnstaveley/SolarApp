using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class DataPoint
    {
        public string _id { get; set; }
        public Head Head { get; set; }
        public Body Body { get; set; }
    }

    public class Head
    {
        public DateTime Timestamp { get; set; }
        public RequestArguments RequestArguments { get; set; }
        public Status Status { get; set; }
    }

    public class Status
    {
        public int Code { get; set; }
        public string Reason { get; set; }
        public string UserMessage { get; set; }
    }

    public class RequestArguments
    {
        public string Query { get; set; }
        public string Scope { get; set; }
    }

    public class Body
    {
        public FroniusEnergyReading PAC { get; set; }
        public FroniusEnergyReading DAY_ENERGY { get; set; }
        public FroniusEnergyReading YEAR_ENERGY { get; set; }
        public FroniusEnergyReading TOTAL_ENERGY { get; set; }
    }

    public class FroniusEnergyReading
    {
        public string Unit { get; set; }
        public FroniusEnergyReadingItem Values { get; set; }
    }

    public class FroniusEnergyReadingItem
    {

        [JsonProperty("1")]
        public int Value { get; set; }
    }
}
