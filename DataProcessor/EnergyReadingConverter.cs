using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DataProcessor
{
	public static class EnergyReadingConverter
	{

		public static EnergyReading CreateEnergyReading(this DataPoint dataPoint)
		{

			var energyReading = new EnergyReading();
			energyReading.Timestamp = dataPoint.Head.Timestamp;
			energyReading.CurrentReading = dataPoint.Body.CurrentReading.Values.Value;
			energyReading.DayEnergy = dataPoint.Body.DayEnergy.Values.Value;
			energyReading.YearEnergy = dataPoint.Body.YearEnergy.Values.Value;
			energyReading.TotalEnergy = dataPoint.Body.TotalEnergy.Values.Value;
			return energyReading;

		}

		public static DataPoint CreateDataPoint(this EnergyReading energyReading)
		{

			DataPoint dataPoint = new DataPoint();
			dataPoint._id = Guid.NewGuid().ToString();
			dataPoint.Body = new Body()
			{
				CurrentReading = new FroniusEnergyReading()
				{
					Unit = "W",
					Values = new FroniusEnergyReadingItem()
					{
						Value = energyReading.CurrentReading
					}
				},
				DayEnergy = new FroniusEnergyReading()
				{
					Unit = "Wh",
					Values = new FroniusEnergyReadingItem()
					{
						Value = energyReading.DayEnergy
					}
				},
				YearEnergy = new FroniusEnergyReading()
				{
					Unit = "Wh",
					Values = new FroniusEnergyReadingItem()
					{
						Value = energyReading.YearEnergy
					}
				},
				TotalEnergy = new FroniusEnergyReading()
				{
					Unit = "Wh",
					Values = new FroniusEnergyReadingItem()
					{
						Value = energyReading.TotalEnergy
					}
				}
			};
			dataPoint.Head = new Head()
			{
				Timestamp = energyReading.Timestamp,
				RequestArguments = new RequestArguments()
				{
					Query = "Inverter",
					Scope = "System"
				},
				Status = new Status()
				{
					Code = 0,
					Reason = "",
					UserMessage = ""
				}
			};
			return dataPoint;

		}

	}
}
