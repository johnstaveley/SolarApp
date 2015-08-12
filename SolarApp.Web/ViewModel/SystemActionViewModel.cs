using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarApp.Web.ViewModel
{
    public class SystemActionViewModel
    {

        public string RequestWeatherForecast { get; set; }

        public string RequestWeatherObservation { get; set; }

        public bool IsDatabaseAvailable { get; set; }

        public SystemActionViewModel(bool isDatabaseAvailable, string requestWeatherForecast, string requestWeatherObservation)
        {
            IsDatabaseAvailable = isDatabaseAvailable;
            switch (requestWeatherForecast)
            {
                case "0":
                    RequestWeatherForecast = "No";
                    break;
                case "1":
                    RequestWeatherForecast = "Pending";
                    break;
                default:
                    RequestWeatherForecast = "Unknwon";
                    break;
            }
            switch (requestWeatherObservation)
            {
                case "0":
                    RequestWeatherObservation = "No";
                    break;
                case "1":
                    RequestWeatherObservation = "Pending";
                    break;
                default:
                    RequestWeatherObservation = "Unknwon";
                    break;
            }
        }
    }
}