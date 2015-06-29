using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarApp.Model
{
	public class EnergyReading
	{

		public DateTime Timestamp { get; set; }

		public int CurrentReading { get; set; }

		public int DayEnergy { get; set; }

		public int YearEnergy { get; set; }

		public int TotalEnergy { get; set; }

		public string RequestArgumentsQuery { get; set; }

		public string RequestArgumentsScope { get; set; }

		public int StatusCode { get; set; }

		public string StatusReason { get; set; }

		public string StatusUserMessage { get; set; }

		public EnergyReading()
		{
			RequestArgumentsQuery = "Inverter";
			RequestArgumentsScope = "System";
			StatusReason = "";
			StatusUserMessage = "";
		}
	}
}
