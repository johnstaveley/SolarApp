using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarApp.Model;

namespace SolarApp.DataProcessor
{
	public static class EnergyReadingConverter
	{

		public static EnergyReading CreateEnergyReading(this DataPoint dataPoint)
		{

			var energyReading = new EnergyReading();
			if (dataPoint.Head != null)
			{
				energyReading.Timestamp = dataPoint.Head.Timestamp;
				if (dataPoint.Head.RequestArguments != null)
				{

					energyReading.RequestArgumentsQuery = dataPoint.Head.RequestArguments.Query;
					energyReading.RequestArgumentsScope = dataPoint.Head.RequestArguments.Scope;
				}
				if (dataPoint.Head.Status != null)
				{

					energyReading.StatusCode = dataPoint.Head.Status.Code;
					energyReading.StatusReason = dataPoint.Head.Status.Reason;
					energyReading.StatusUserMessage = dataPoint.Head.Status.UserMessage;
				}
			}
			if (dataPoint.Body != null)
			{
				if (dataPoint.Body.CurrentReading != null && dataPoint.Body.CurrentReading.Values != null)
				{
					energyReading.CurrentReading = dataPoint.Body.CurrentReading.Values.Value;
				}
				if (dataPoint.Body.DayEnergy != null && dataPoint.Body.DayEnergy.Values != null)
				{
					energyReading.DayEnergy = dataPoint.Body.DayEnergy.Values.Value;
				}
				if (dataPoint.Body.YearEnergy != null && dataPoint.Body.YearEnergy.Values != null)
				{
					energyReading.YearEnergy = dataPoint.Body.YearEnergy.Values.Value;
				}
				if (dataPoint.Body.TotalEnergy != null && dataPoint.Body.TotalEnergy.Values != null)
				{
					energyReading.TotalEnergy = dataPoint.Body.TotalEnergy.Values.Value;
				}
			}
			return energyReading;

		}

		public static DataPoint CreateDataPoint(this EnergyReading energyReading)
		{

			DataPoint dataPoint = new DataPoint();
			dataPoint.Id = Guid.NewGuid().ToString();
			dataPoint.Body = new Body()
			{
				CurrentReading = new FroniusEnergyReading()
				{
					Unit = energyReading.CurrentReadingUnit,
					Values = new FroniusEnergyReadingItem()
					{
						Value = energyReading.CurrentReading
					}
				},
				DayEnergy = new FroniusEnergyReading()
				{
					Unit = energyReading.DayEnergyUnit,
					Values = new FroniusEnergyReadingItem()
					{
						Value = energyReading.DayEnergy
					}
				},
				YearEnergy = new FroniusEnergyReading()
				{
					Unit = energyReading.YearEnergyUnit,
					Values = new FroniusEnergyReadingItem()
					{
						Value = energyReading.YearEnergy
					}
				},
				TotalEnergy = new FroniusEnergyReading()
				{
					Unit = energyReading.TotalEnergyUnit,
					Values = new FroniusEnergyReadingItem()
					{
						Value = energyReading.TotalEnergy
					}
				}
			};
			dataPoint.Head = new Head()
			{
				Timestamp = energyReading.Timestamp.ToUniversalTime(),
				RequestArguments = new RequestArguments()
				{
					Query = energyReading.RequestArgumentsQuery,
					Scope = energyReading.RequestArgumentsScope
				},
				Status = new Status()
				{
					Code = energyReading.StatusCode,
					Reason = energyReading.StatusReason,
					UserMessage = energyReading.StatusUserMessage
				}
			};
			return dataPoint;

		}

	}
}
