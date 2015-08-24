using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarApp.Web.ViewModel
{
    public class SystemStateViewModel
    {

        public string Environment { get; set; }

		public bool IsDatabaseAvailable
		{
			get
			{
				return !string.IsNullOrEmpty(LastRunDate);
			}
		}
		
		public string LastRunDate { get; set; }

        public string LatestMeterReading {get; set; }

        public long NumberOfDataPoints { get; set;  }

        public long NumberOfFailedData { get; set; }

        public long NumberOfWeatherForecasts { get; set; }

        public long NumberOfWeatherObservations { get; set; }

		public string Sunrise { get; set; }

		public string Sunset { get; set; }

        public SystemStateViewModel(string lastRunDate, DateTime? latestMeterReading, 
            long numberOfDataPoints, long numberOfFailedData, long numberOFWeatherForecasts,
            long numberOfWeatherObservations, string environment, DateTime? sunrise, DateTime? sunset)
        {
            LastRunDate = lastRunDate;
            LatestMeterReading = (latestMeterReading.HasValue ? latestMeterReading.Value.ToString("dd/MM/yyyy HH:mm:ss") : "None");
            NumberOfDataPoints = numberOfDataPoints;
            NumberOfFailedData = numberOfFailedData;
            NumberOfWeatherForecasts = numberOFWeatherForecasts;
            NumberOfWeatherObservations = numberOfWeatherObservations;
            Environment = environment;
			Sunrise = (sunrise.HasValue ? sunrise.Value.ToString("HH:mm"): "Not set" );
			Sunset = (sunset.HasValue ? sunset.Value.ToString("HH:mm") : "Not set");
        }
    }
}