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

		public string CurrentReadingUnit { get; set; }

		public int DayEnergy { get; set; }

		public string DayEnergyUnit { get; set; }

		public int YearEnergy { get; set; }

		public string YearEnergyUnit { get; set; }

		public int TotalEnergy { get; set; }

		public string TotalEnergyUnit { get; set; }

		public string RequestArgumentsQuery { get; set; }

		public string RequestArgumentsScope { get; set; }

		public int StatusCode { get; set; }

		public string StatusReason { get; set; }

		public string StatusUserMessage { get; set; }

		public EnergyReading()
		{
			CurrentReadingUnit = "W";
			DayEnergyUnit = "Wh";
			RequestArgumentsQuery = "Inverter";
			RequestArgumentsScope = "System";
			StatusReason = "";
			StatusUserMessage = "";
			TotalEnergyUnit = "Wh";
			YearEnergyUnit = "Wh";
		}
	}
}
