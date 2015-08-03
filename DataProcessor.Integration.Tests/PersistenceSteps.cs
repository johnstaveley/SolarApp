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


		[When(@"I calculate the latest date")]
		public void WhenICalculateTheLatestDate()
		{
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			DateTime? latestReading = context.GetLatestEnergyReading();
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
            var setting = new Setting();
            setting.Value = "Test";
            setting.Id = Guid.NewGuid().ToString();
			ScenarioContext.Current.Set<Setting>(setting, "RandomVariable");
        }

        [When(@"I persist the setting to the database")]
        public void WhenIPersistTheSettingToTheDatabase()
        {
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var setting = ScenarioContext.Current.Get<Setting>("RandomVariable");
            context.InsertSetting(setting);
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			dataItemsToTrack.Add(new DataItem(setting));
			ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
		}
        
        [Then(@"the random value should be retrievable from the database")]
        public void ThenTheRandomValueShouldBeRetrievableFromTheDatabase()
        {
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var setting = ScenarioContext.Current.Get<Setting>("RandomVariable");
            Assert.IsTrue(context.FindSettingById(setting.Id) != null, string.Format("Could not find data value {0}", setting.Id));
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
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			context.InsertDataPoint(dataPoint);
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			dataItemsToTrack.Add(new DataItem(dataPoint));
			ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
		}

		[Then(@"I cannot retrieve a data point")]
		public void ThenICannotRetrieveADataPoint()
		{
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var dataPoint = context.FindDataPointById(dataItemsToTrack.First().Id);
			Assert.IsNull(dataPoint, "Data point has been found in the database");
		}

		[Then(@"I can retrieve failed data with text: '(.*)'")]
		public void ThenICanRetrieveFailedDataWithText(string dataToFind)
		{
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var failedData = context.FindFailedDataById(dataItemsToTrack.First().Id);
			Assert.IsNotNull(failedData, "Failed data has not been found");
			Assert.IsTrue(failedData.Data.Contains(dataToFind), string.Format("Failed data did not contained expected value {0}", dataToFind));
		}

		[Then(@"I can retrieve a data point with values:")]
		public void ThenICanRetrieveADataPointWithValues(Table table)
		{
			var energyReadingData = table.CreateInstance<EnergyReadingData>();
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var dataPoint = context.FindDataPointById(dataItemsToTrack.First().Id);
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
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			foreach (var energyReading in energyReadingData) {
				var dataPoint = energyReading.CreateDataPoint();
				dataPoint.Id = Guid.NewGuid().ToString();
				context.InsertDataPoint(dataPoint);
				var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
				dataItemsToTrack.Add(new DataItem(dataPoint));
				ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
			}
		}

		[When(@"I calculate the mean for hour (.*)")]
		public void WhenICalculateTheMeanForHour(int hour)
		{
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
            var now = DateTime.Now;
            var hoursInUtc = now.AddHours(-now.Hour).AddHours(hour).ToUniversalTime().Hour;
            double? average = context.GetAverageOutputForHour(hoursInUtc);
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
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			LocalFileProcessor fileProcessor = new LocalFileProcessor(configuration, new SolarApp.DataProcessor.Utility.Classes.FileSystem(), context);
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
