using System;
using NUnit.Framework;
using SolarApp.Model;
using SolarApp.DataProcessor;
using SolarApp.DataProcessor.Utility;
using SolarApp.DataProcessor.Utility.Interfaces;
using Newtonsoft.Json;
using SolarApp.DataProcessor.Unit.Tests.Properties;
using Rhino.Mocks;
using SolarApp.Persistence;

namespace SolarApp.DataProcessor.Unit.Tests
{
	[TestFixture]
	public class EnergyReadingConverterTest
	{

		[Test]
		public void Given_EmptyDataPoint_When_ConvertToEnergyReading_Then_EnergyReadingIsCreated()
		{
			// Arrange
			var dataPoint = new DataPoint();			

			// Act
			var energyReading = EnergyReadingConverter.CreateEnergyReading(dataPoint);

			// Assert
			Assert.IsNotNull(energyReading, "EnergyReading was not created correctly");

		}

		[Test]
		public void Given_EmptyEnergyReading_When_ConvertToDataPoint_Then_DataPointIsCreated()
		{
			// Arrange
			var energyReading = new EnergyReading();

			// Act
			var dataPoint = EnergyReadingConverter.CreateDataPoint(energyReading);

			// Assert
			Assert.IsNotNull(dataPoint, "DataPoint was not created correctly");

		}

	}
}
