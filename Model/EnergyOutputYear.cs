using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SolarApp.Model
{

	[JsonObject(MemberSerialization.OptIn)]
    public class EnergyOutputYear
    {

		[JsonProperty("_id", Required = Required.Always)]
        public int Month { get; set; }

		[JsonProperty("month_energy", Required = Required.Always)]
		public double MonthEnergy { get; set; }

    }
}
