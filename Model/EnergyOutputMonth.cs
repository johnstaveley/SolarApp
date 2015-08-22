using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SolarApp.Model
{

	[JsonObject(MemberSerialization.OptIn)]
    public class EnergyOutputMonth
    {

		[JsonProperty("_id", Required = Required.Always)]
        public int Day { get; set; }

		[JsonProperty("day_energy", Required = Required.Always)]
		public double DayEnergy { get; set; }

    }
}
