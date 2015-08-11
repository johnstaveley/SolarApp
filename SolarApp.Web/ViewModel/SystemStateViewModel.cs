using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarApp.Web.ViewModel
{
    public class SystemStateViewModel
    {
        public string LastRunDate {get; set; }

        public string LatestMeterReading {get; set; }

        public string Environment { get; set; }

        public bool IsDatabaseAvailable {
            get 
                {
                return !string.IsNullOrEmpty(LastRunDate);
                }
            }

        public SystemStateViewModel(string lastRunDate, DateTime? latestMeterReading, string environment)
        {
            LastRunDate = lastRunDate;
            LatestMeterReading = (latestMeterReading.HasValue ? latestMeterReading.Value.ToString("dd/MM/yyyy HH:mm:ss") : "None");
            Environment = environment;
        }
    }
}