using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace SolarApp.Model
{
    [Serializable]
    public class DataPoint
    {
        [JsonIgnore, BsonElement("_id")]
        public string Id { get; set; }

        [JsonProperty(Order = 1)]
        public Head Head { get; set; }

        [JsonProperty(Order = 2)]
        public Body Body { get; set; }

		public DataPoint()
		{
			Head = new Head();
			Body = new Body();
		}
    }

    public class Head
    {
        [JsonProperty(Order=1)]
        public DateTime Timestamp { get; set; }

        [JsonProperty(Order = 2)]
        public RequestArguments RequestArguments { get; set; }

        [JsonProperty(Order = 3)]
        public Status Status { get; set; }

		public Head()
		{
			RequestArguments = new RequestArguments();
			Status = new Status();
		}
    }

    public class Status
    {
        [JsonProperty(Order = 1)]
        public int Code { get; set; }

        [JsonProperty(Order = 2)]
        public string Reason { get; set; }

        [JsonProperty(Order = 3)]
        public string UserMessage { get; set; }

		public Status()
		{
			Reason = "";
			UserMessage = "";
		}
    }

    public class RequestArguments
    {
        [JsonProperty(Order = 1)]
        public string Query { get; set; }

        [JsonProperty(Order = 2)]
        public string Scope { get; set; }

		public RequestArguments()
		{
			Query = "Inverter";
			Scope = "System";
		}
    }

    public class Body
    {
        [JsonProperty(Order = 1, PropertyName="PAC"), BsonElement("PAC")]
        public FroniusEnergyReading CurrentReading { get; set; }

		[JsonProperty(Order = 2, PropertyName = "DAY_ENERGY"), BsonElement("DAY_ENERGY")]
        public FroniusEnergyReading DayEnergy { get; set; }

		[JsonProperty(Order = 3, PropertyName = "YEAR_ENERGY"), BsonElement("YEAR_ENERGY")]
        public FroniusEnergyReading YearEnergy { get; set; }

		[JsonProperty(Order = 4, PropertyName = "TOTAL_ENERGY"), BsonElement("TOTAL_ENERGY")]
        public FroniusEnergyReading TotalEnergy { get; set; }

		public Body()
		{
			CurrentReading = new FroniusEnergyReading();
			DayEnergy = new FroniusEnergyReading();
			YearEnergy = new FroniusEnergyReading();
			TotalEnergy = new FroniusEnergyReading();
		}
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

        [JsonProperty(PropertyName="1"), BsonElement("1")]
        public int Value { get; set; }
    }
}
