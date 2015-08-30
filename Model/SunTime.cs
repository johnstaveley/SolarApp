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
		public string Date { get; set; }

		[BsonElement("sunrise")]
		public string Sunrise { get; set; }

		[BsonElement("sunset")]
		public string Sunset { get; set; }

		[JsonIgnore]
		public DateTime? TargetDate
		{
			get
			{
				try
				{
					DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", new CultureInfo("en-GB"));
					return date;
				}
				catch
				{
					return null;
				}
			}
		}

        [JsonIgnore]
        public DateTime? SunAzimuthDateTime
        {
            get
            {
                if (SunriseDateTime.HasValue && SunsetDateTime.HasValue)
                {
                    return SunriseDateTime.Value.AddTicks((SunsetDateTime.Value.Ticks - SunriseDateTime.Value.Ticks) / 2);
                }
                return null;
            }
        }

		[JsonIgnore]
		public DateTime? SunriseDateTime
		{
			get
			{
				try
				{
					TimeSpan time = TimeSpan.Parse(Sunrise);
					return (TargetDate.HasValue ? (DateTime?) TargetDate.Value.AddTicks(time.Ticks) : null);
				}
				catch
				{
					return null;
				}
			}
		}

		[JsonIgnore]
		public DateTime? SunsetDateTime
		{
			get
			{
				try
				{
					TimeSpan time = TimeSpan.Parse(Sunset);
					return (TargetDate.HasValue ? (DateTime?)TargetDate.Value.AddTicks(time.Ticks) : null);
				}
				catch
				{
					return null;
				}
			}
		}

	}
}
