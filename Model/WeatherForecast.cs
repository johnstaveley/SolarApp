using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SolarApp.Model
{
	[Serializable]
	public class WeatherForecast
	{
		    
        [JsonIgnore, BsonElement("_id")]
        public string Id { get; set; }

        [JsonProperty(Order = 1)]
        public string Data { get; set; }

	}
}
