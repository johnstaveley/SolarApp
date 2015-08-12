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

        public bool IsDatabaseAvailable {
            get 
                {
                return !string.IsNullOrEmpty(LastRunDate);
                }
            }

        public SystemStateViewModel(string lastRunDate, DateTime? latestMeterReading, long numberOfDataPoints, long numberOfFailedData, string environment)
        {
            LastRunDate = lastRunDate;
            LatestMeterReading = (latestMeterReading.HasValue ? latestMeterReading.Value.ToString("dd/MM/yyyy HH:mm:ss") : "None");
            NumberOfDataPoints = numberOfDataPoints;
            NumberOfFailedData = numberOfFailedData;
            Environment = environment;
        }
    }
}