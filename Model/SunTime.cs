using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SolarApp.Model
{
	public class SunTime
	{

		[BsonElement("_id")]
		public DateTime Date { get; set; }

		[BsonElement("sunrise")]
		public DateTime Sunrise { get; set; }

		[BsonElement("sunset")]
		public DateTime Sunset { get; set; }

        [JsonIgnore]
        public DateTime SunAzimuth
        {
            get
            {
	            return Sunrise.AddTicks((Sunset.Ticks - Sunrise.Ticks) / 2);
            }
        }

	}
}
