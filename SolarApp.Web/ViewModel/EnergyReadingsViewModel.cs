using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SolarApp.Model;
using SolarApp.Web.Extensions;

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
            GraphData = energyReadings.Select(e => new { e.Timestamp, e.DayEnergyInstant, e.CurrentEnergy })
				.Aggregate(new StringBuilder(), (sb, next) => sb.Append("[").Append(next.Timestamp.ToJavaScriptMilliseconds()).Append(",").Append(next.DayEnergyInstant).Append(",").Append(next.CurrentEnergy).Append("],"), sb => sb.ToString().Trim(','));
        }

    }
}