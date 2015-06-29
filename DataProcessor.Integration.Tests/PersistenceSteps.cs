﻿using System;
using TechTalk.SpecFlow;
using System.Configuration;
using SolarApp.Persistence;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;
using SolarApp.DataProcessor.Tests.Helper;
using SolarApp.DataProcessor;
using SolarApp.Model;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SolarApp.DataProcessor.Utility.Interfaces;

namespace SolarApp.DataProcessor.Integration.Tests
{
    [Binding]
    public class PersistenceSteps
    {

        private Setting _setting { get; set; }
        private SolarAppContext _context { get; set; }
		private IConfiguration _configuration { get; set; }
		private List<DataItem> _dataItemsToTrack { get; set; }

		[BeforeScenario]
		public void ScenarioSetup()
		{
			_configuration = new SolarApp.DataProcessor.Utility.Classes.Configuration();
			_dataItemsToTrack = new List<DataItem>();
            if (!Directory.Exists(_configuration.NewFilePollPath))
            {
                Directory.CreateDirectory(_configuration.NewFilePollPath);
            }
		}

		[AfterScenario]
		public void ScenarioCleanup()
		{
			// Remove tracked items from the database
			foreach (var dataItem in _dataItemsToTrack)
			{
				switch (dataItem.TableTypeKind)
				{
					case (TableTypeKind.DataPoint):
						_context.DeleteDataPointById(dataItem.Id);
						_context.DeleteFailedDataById(dataItem.Id);
						break;
					case (TableTypeKind.Setting):
						_context.DeleteSettingById(dataItem.Id);
						break;
					default:
						throw new Exception(string.Format("Unable to determine type of artifact to delete for id ", dataItem.Id));
				}
			}
		}

		[When(@"I calculate the latest date")]
		public void WhenICalculateTheLatestDate()
		{
			DateTime? latestReading = _context.GetLatestEnergyReading();
			ScenarioContext.Current.Set<DateTime?>(latestReading, "LatestEnergyReading");
		}

		[Then(@"The calculated latest date is '(.*)'")]
		public void ThenTheCalculatedAverageValueIs(DateTime latestReading)
		{
			DateTime? result = null;
			if (ScenarioContext.Current["LatestEnergyReading"] != null)
			{
				result = ScenarioContext.Current.Get<DateTime?>("LatestEnergyReading");
			}
			Assert.AreEqual(latestReading, result);
		}

        [Given(@"I want to store some random value")]
        public void GivenIWantToStoreSomeRandomValue()
        {
            _setting = new Setting();
            _setting.Value = "Test";
            _setting.Id = Guid.NewGuid().ToString();
        }

        [Given(@"I want to use a database '(.*)'")]
        public void GivenIWantToUseADatabase(string databaseName)
        {
			_configuration.MongoDatabaseName = databaseName;
        }

		[Given(@"I open a connection to the database")]
		[When(@"I open a connection to the database")]
        public void WhenIOpenAConnectionToTheDatabase()
        {
			_context = new SolarAppContext(_configuration);
        }

        [When(@"I persist the setting to the database")]
        public void WhenIPersistTheSettingToTheDatabase()
        {
            _context.InsertSetting(_setting);
			_dataItemsToTrack.Add(new DataItem(_setting));
		}
        
        [Then(@"the random value should be retrievable from the database")]
        public void ThenTheRandomValueShouldBeRetrievableFromTheDatabase()
        {
            Assert.IsTrue(_context.FindSettingById(_setting.Id) != null, string.Format("Could not find data value {0}", _setting.Id));
        }

		[Given(@"I have a data point with values:")]
		public void GivenIHaveADataPointWithValues(Table table)
		{
			var energyReadingData = table.CreateInstance<EnergyReadingData>();
			ScenarioContext.Current.Add("EnergyReadingData", energyReadingData);
		}

		[When(@"I persist the data point to the database")]
		public void WhenIPersistTheDataPointToTheDatabase()
		{
			var energyReadingData = ScenarioContext.Current.Get<EnergyReadingData>("EnergyReadingData");
			var dataPoint = energyReadingData.CreateDataPoint();
			dataPoint.Id = energyReadingData.FileName;
			_context.InsertDataPoint(dataPoint);
			_dataItemsToTrack.Add(new DataItem(dataPoint));
		}

		[Then(@"I cannot retrieve a data point")]
		public void ThenICannotRetrieveADataPoint()
		{
			var dataPoint = _context.FindDataPointById(_dataItemsToTrack.First().Id);
			Assert.IsNull(dataPoint, "Data point has been found in the database");
		}

		[Then(@"I can retrieve failed data with text: '(.*)'")]
		public void ThenICanRetrieveFailedDataWithText(string dataToFind)
		{
			var failedData = _context.FindFailedDataById(_dataItemsToTrack.First().Id);
			Assert.IsNotNull(failedData, "Failed data has not been found");
			Assert.IsTrue(failedData.Data.Contains(dataToFind), string.Format("Failed data did not contained expected value {0}", dataToFind));
		}

		[Then(@"I can retrieve a data point with values:")]
		public void ThenICanRetrieveADataPointWithValues(Table table)
		{
			var energyReadingData = table.CreateInstance<EnergyReadingData>();
			var dataPoint = _context.FindDataPointById(_dataItemsToTrack.First().Id);
			var retrievedEnergyReadingData = dataPoint.CreateEnergyReading();
			Assert.IsTrue(retrievedEnergyReadingData.Timestamp - energyReadingData.Timestamp < new TimeSpan(0, 5, 0), "Timestamp is incorrect");
			Assert.AreEqual(energyReadingData.DayEnergy, retrievedEnergyReadingData.DayEnergy);
			Assert.AreEqual(energyReadingData.YearEnergy, retrievedEnergyReadingData.YearEnergy);
			Assert.AreEqual(energyReadingData.TotalEnergy, retrievedEnergyReadingData.TotalEnergy);

		}

		[Given(@"I have a data points with values:")]
		public void GivenIHaveADataPointsWithValues(Table table)
		{
			var energyReadingData = table.CreateSet<EnergyReadingData>();
			foreach (var energyReading in energyReadingData) {
				var dataPoint = energyReading.CreateDataPoint();
				dataPoint.Id = Guid.NewGuid().ToString();
				_context.InsertDataPoint(dataPoint);
				_dataItemsToTrack.Add(new DataItem(dataPoint));
			}
		}

		[When(@"I calculate the mean for hour (.*)")]
		public void WhenICalculateTheMeanForHour(int hour)
		{
			double? average = _context.GetAverageOutputForHour(hour);
			ScenarioContext.Current.Set<double?>(average, "CalculatedAverage");
		}

		[Then(@"The calculated average value is ([0-9]+)")]
		public void ThenTheCalculatedAverageValueIs(double average)
		{
			AssertAverage(average);
		}

		[Then(@"The calculated average value is null")]
		public void ThenTheCalculatedAverageValueIsNull()
		{
			AssertAverage(null);
		}

		public void AssertAverage(double? average)
		{
			double? result = null;
			if (ScenarioContext.Current["CalculatedAverage"] != null)
			{
				result = ScenarioContext.Current.Get<double?>("CalculatedAverage");
			}
			Assert.AreEqual(average, result);
		}

		[Given(@"I save the data point to a file")]
		public void GivenISaveTheDataPointToAFile()
		{
			var energyReadingData = ScenarioContext.Current.Get<EnergyReadingData>("EnergyReadingData");
			var dataPoint = energyReadingData.CreateDataPoint();
			dataPoint.Id = energyReadingData.FileName;
			var filePath = Path.Combine(_configuration.NewFilePollPath, energyReadingData.FileName);
			dataPoint.SaveAsJson(filePath);
		}

		[When(@"I process the file")]
		public void WhenIProcessTheFile()
		{
			IConfiguration configuration = new SolarApp.DataProcessor.Utility.Classes.Configuration();
			LocalFileProcessor fileProcessor = new LocalFileProcessor(configuration, new SolarApp.DataProcessor.Utility.Classes.FileSystem(), _context);
			var dataPointIds = fileProcessor.Process();
			foreach (var dataPointId in dataPointIds)
			{
				var dataPoint = new DataPoint();
				dataPoint.Id = dataPointId;
				_dataItemsToTrack.Add(new DataItem(dataPoint));
			}
		}

    }
}
