using System;
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
using SolarApp.DataProcessor.Utility.Classes;

namespace SolarApp.DataProcessor.Integration.Tests
{
    [Binding]
    public class PersistenceSteps
    {

        private Setting _setting { get; set; }
        private SolarAppContext _context { get; set; }

		[AfterScenario]
		public void ScenarioCleanup()
		{
			// Remove tracked items from the database
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			foreach (var dataItem in dataItemsToTrack)
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
					case (TableTypeKind.WeatherForecast):
						_context.DeleteWeatherForecastById(dataItem.Id);
						break;
					case (TableTypeKind.WeatherObservation):
						_context.DeleteWeatherObservationById(dataItem.Id);
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
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			configuration.MongoDatabaseName = databaseName;
			ScenarioContext.Current.Set<IConfiguration>(configuration);
        }

		[Given(@"I open a connection to the database")]
		[When(@"I open a connection to the database")]
        public void WhenIOpenAConnectionToTheDatabase()
        {
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			_context = new SolarAppContext(configuration);
        }

        [When(@"I persist the setting to the database")]
        public void WhenIPersistTheSettingToTheDatabase()
        {
            _context.InsertSetting(_setting);
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			dataItemsToTrack.Add(new DataItem(_setting));
			ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
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
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			dataItemsToTrack.Add(new DataItem(dataPoint));
			ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
		}

		[Then(@"I cannot retrieve a data point")]
		public void ThenICannotRetrieveADataPoint()
		{
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var dataPoint = _context.FindDataPointById(dataItemsToTrack.First().Id);
			Assert.IsNull(dataPoint, "Data point has been found in the database");
		}

		[Then(@"I can retrieve failed data with text: '(.*)'")]
		public void ThenICanRetrieveFailedDataWithText(string dataToFind)
		{
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var failedData = _context.FindFailedDataById(dataItemsToTrack.First().Id);
			Assert.IsNotNull(failedData, "Failed data has not been found");
			Assert.IsTrue(failedData.Data.Contains(dataToFind), string.Format("Failed data did not contained expected value {0}", dataToFind));
		}

		[Then(@"I can retrieve a data point with values:")]
		public void ThenICanRetrieveADataPointWithValues(Table table)
		{
			var energyReadingData = table.CreateInstance<EnergyReadingData>();
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var dataPoint = _context.FindDataPointById(dataItemsToTrack.First().Id);
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
				var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
				dataItemsToTrack.Add(new DataItem(dataPoint));
				ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
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
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			var filePath = Path.Combine(configuration.NewFilePollPath, energyReadingData.FileName);
			dataPoint.SaveAsJson(filePath);
		}

        [Given(@"I have a file containing garbage: '(.*)'")]
        public void GivenIHaveAFileContainingGarbage(string text)
        {
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
            var filePath = Path.Combine(configuration.NewFilePollPath, string.Format("Log{0}.log", Guid.NewGuid().ToString()));
            File.WriteAllText(filePath, text);
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
				var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
				dataItemsToTrack.Add(new DataItem(dataPoint));
				ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
			}
		}

    }
}
