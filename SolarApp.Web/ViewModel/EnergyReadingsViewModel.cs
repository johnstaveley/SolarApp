using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SolarApp.Model;

namespace SolarApp.Web.ViewModel
{
    public class EnergyReadingsViewModel
    {

        public List<EnergyOutput> EnergyReadings { get; set; }

		public string GraphData { get; set; }

        public bool IsDatabaseAvailable { get; set; }

        public EnergyReadingsViewModel(bool isDatabaseAvailable, List<EnergyOutput> energyReadings)
        {
            IsDatabaseAvailable = isDatabaseAvailable;
			EnergyReadings = energyReadings;
			GraphData = string.Format("{0}", energyReadings.Select(e => e.DayEnergyInstant)
				.Aggregate(new StringBuilder(), (sb, next) => sb.Append(",").Append(next), sb => sb.ToString().Trim(',')));
        }
    }
}