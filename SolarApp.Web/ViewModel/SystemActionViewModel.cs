using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarApp.Web.ViewModel
{
    public class SystemActionViewModel
    {

        public bool CanRequestWeatherForecast { get; set; }

        public bool CanRequestWeatherObservation { get; set; }

        public string RequestWeatherForecast { get; set; }

        public string RequestWeatherObservation { get; set; }

        public bool IsDatabaseAvailable { get; set; }

        public SystemActionViewModel(bool isDatabaseAvailable, string requestWeatherForecast, string requestWeatherObservation)
        {
            IsDatabaseAvailable = isDatabaseAvailable;
            CanRequestWeatherForecast = false;
            switch (requestWeatherForecast)
            {
                case "0":
                    RequestWeatherForecast = "No";
                    CanRequestWeatherForecast = true;
                    break;
                case "1":
                    RequestWeatherForecast = "Pending";
                    break;
                default:
                    RequestWeatherForecast = "Unknown";
                    CanRequestWeatherForecast = true;
                    break;
            }
            CanRequestWeatherObservation = false;
            switch (requestWeatherObservation)
            {
                case "0":
                    RequestWeatherObservation = "No";
                    CanRequestWeatherObservation = true;
                    break;
                case "1":
                    RequestWeatherObservation = "Pending";
                    break;
                default:
                    RequestWeatherObservation = "Unknown";
                    CanRequestWeatherObservation = true;
                    break;
            }
        }
    }
}