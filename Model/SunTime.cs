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

		/// <summary>
		/// Midnight of the target date in UTC
		/// </summary>
		[BsonElement("_id")]
		public DateTime Date { get; set; }

		/// <summary>
		/// Sunrise of the target date in UTC
		/// </summary>
		[BsonElement("sunrise")]
		public DateTime Sunrise { get; set; }

		/// <summary>
		/// Sunset of the target date in UTC
		/// </summary>
		[BsonElement("sunset")]
		public DateTime Sunset { get; set; }

		/// <summary>
		/// The sun azimuth time and date in UTC
		/// </summary>
        [JsonIgnore]
        public DateTime SunAzimuth
        {
            get
            {
	            return Sunrise.AddTicks((Sunset.Ticks - Sunrise.Ticks) / 2);
            }
        }

		public double SunIntensity(DateTime date)
		{
			if (date >= Sunset) return 0;
			if (date <= Sunrise) return 0;
			if (date > SunAzimuth)
			{
				return ((double)(date.Ticks - SunAzimuth.Ticks) / (double)(Sunset.Ticks - SunAzimuth.Ticks) * 100.0);
			}
			else
			{
				return ((double)(date.Ticks - Sunrise.Ticks) / (double) (SunAzimuth.Ticks - Sunrise.Ticks)) * 100.0;
			}
		}

	}
}
