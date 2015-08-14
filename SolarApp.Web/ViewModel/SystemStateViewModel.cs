using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarApp.Web.ViewModel
{
    public class SystemStateViewModel
    {

        public string Environment { get; set; }
        
        public string LastRunDate { get; set; }

        public string LatestMeterReading {get; set; }

        public long NumberOfDataPoints { get; set;  }

        public long NumberOfFailedData { get; set; }

        public long NumberOfWeatherForecasts { get; set; }

        public long NumberOfWeatherObservations { get; set; }

        public bool IsDatabaseAvailable {
            get 
                {
                return !string.IsNullOrEmpty(LastRunDate);
                }
            }

        public SystemStateViewModel(string lastRunDate, DateTime? latestMeterReading, 
            long numberOfDataPoints, long numberOfFailedData, long numberOFWeatherForecasts,
            long numberOfWeatherObservations, string environment)
        {
            LastRunDate = lastRunDate;
            LatestMeterReading = (latestMeterReading.HasValue ? latestMeterReading.Value.ToString("dd/MM/yyyy HH:mm:ss") : "None");
            NumberOfDataPoints = numberOfDataPoints;
            NumberOfFailedData = numberOfFailedData;
            NumberOfWeatherForecasts = numberOFWeatherForecasts;
            NumberOfWeatherObservations = numberOfWeatherObservations;
            Environment = environment;
        }
    }
}