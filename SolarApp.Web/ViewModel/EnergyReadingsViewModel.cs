using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolarApp.Model;

namespace SolarApp.Web.ViewModel
{
    public class EnergyReadingsViewModel
    {

        public List<EnergyOutput> EnergyReadings { get; set; }

        public bool IsDatabaseAvailable { get; set; }

        public EnergyReadingsViewModel(bool isDatabaseAvailable, List<EnergyOutput> energyReadings)
        {
            IsDatabaseAvailable = isDatabaseAvailable;
			EnergyReadings = energyReadings;
        }
    }
}