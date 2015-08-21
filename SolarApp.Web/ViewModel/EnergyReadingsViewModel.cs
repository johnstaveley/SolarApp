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

        public bool IsDatabaseAvailable { get; set; }

		public DateTime TargetDate { get; set; }

        public EnergyReadingsViewModel(bool isDatabaseAvailable, DateTime targetDate)
        {
            IsDatabaseAvailable = isDatabaseAvailable;
			TargetDate = targetDate;
        }

    }
}