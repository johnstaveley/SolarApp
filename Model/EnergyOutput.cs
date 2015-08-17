using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SolarApp.Model
{

	[JsonObject(MemberSerialization.OptIn)]
    public class EnergyOutput
    {

		[JsonProperty("_id", Required = Required.Always)]
        public DateTime Timestamp { get; set; }

		[JsonProperty("current_energy", Required = Required.Always)]
        public double CurrentEnergy { get; set; }

		[JsonProperty("day_energy", Required = Required.Always)]
		public double DayEnergy { get; set; }

    }
}
