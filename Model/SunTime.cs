using System;
using System.Collections.Generic;
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

		public string Sunrise { get; set; }

		public string Sunset { get; set; }

		[JsonIgnore]
		public DateTime? TargetDate
		{
			get
			{
				try
				{
					DateTime date = DateTime.Parse(Date);
					return date;
				}
				catch
				{
					return null;
				}
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
