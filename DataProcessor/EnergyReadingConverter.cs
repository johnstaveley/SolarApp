﻿using System;
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
			energyReading.Timestamp = dataPoint.Head.Timestamp;
			energyReading.RequestArgumentsQuery = dataPoint.Head.RequestArguments.Query;
			energyReading.RequestArgumentsScope = dataPoint.Head.RequestArguments.Scope;
			energyReading.StatusCode = dataPoint.Head.Status.Code;
			energyReading.StatusReason = dataPoint.Head.Status.Reason;
			energyReading.StatusUserMessage = dataPoint.Head.Status.UserMessage;
			energyReading.CurrentReading = dataPoint.Body.CurrentReading.Values.Value;
			energyReading.DayEnergy = dataPoint.Body.DayEnergy.Values.Value;
			energyReading.YearEnergy = dataPoint.Body.YearEnergy.Values.Value;
			energyReading.TotalEnergy = dataPoint.Body.TotalEnergy.Values.Value;
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
