using System;
using TechTalk.SpecFlow;
using System.Configuration;
using Persistence;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;
using DataProcessor.Tests.Helper;
using DataProcessor;
using Model;

namespace DataProcessor.Integration.Tests
{
    [Binding]
    public class PersistenceSteps
    {

        private string _databaseName { get; set; }
        private Setting _setting { get; set; }
        private SolarAppContext _context { get; set; }
		private string _mongoDbConnectionString { get; set; }
		private Guid _dataPointId { get; set; }

		[BeforeScenario]
		public void ScenarioSetup()
		{
			_mongoDbConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
			_dataPointId = Guid.NewGuid();
		}
       
        [Given(@"I want to store some random value")]
        public void GivenIWantToStoreSomeRandomValue()
        {
            _setting = new Setting();
            _setting.Value = "Test";
            _setting._id = Guid.NewGuid().ToString();
        }

        [Given(@"I want to use a database '(.*)'")]
        public void GivenIWantToUseADatabase(string databaseName)
        {
            _databaseName = databaseName;
        }

        [When(@"I open a connection to the database")]
        public void WhenIOpenAConnectionToTheDatabase()
        {
			_context = new SolarAppContext(_mongoDbConnectionString, _databaseName);
        }

        [When(@"I persist the setting to the database")]
        public void WhenIPersistTheSettingToTheDatabase()
        {
            _context.InsertSetting(_setting);
        }
        
        [Then(@"the random value should be retrievable from the database")]
        public void ThenTheRandomValueShouldBeRetrievableFromTheDatabase()
        {
            Assert.IsTrue(_context.FindSettingById(_setting._id) != null, string.Format("Could not find data value {0}", _setting._id));
        }

        [AfterScenario]
        public void TearDown()
        {
            if (_context != null && _setting != null)
            {
                _context.DeleteSetting(_setting);
            }
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
			dataPoint._id = _dataPointId.ToString();
			_context.InsertDataPoint(dataPoint);
		}

		[Then(@"I can retrieve a data point with values:")]
		public void ThenICanRetrieveADataPointWithValues(Table table)
		{
			var energyReadingData = table.CreateInstance<EnergyReadingData>();
			var dataPoint = _context.FindDataPointById(_dataPointId.ToString());
			var retrievedEnergyReadingData = dataPoint.CreateEnergyReading();
			Assert.IsTrue(retrievedEnergyReadingData.Timestamp - energyReadingData.Timestamp < new TimeSpan(0, 5, 0), "Timestamp is incorrect");
			Assert.AreEqual(energyReadingData.DayEnergy, retrievedEnergyReadingData.DayEnergy);
			Assert.AreEqual(energyReadingData.YearEnergy, retrievedEnergyReadingData.YearEnergy);
			Assert.AreEqual(energyReadingData.TotalEnergy, retrievedEnergyReadingData.TotalEnergy);
			// Cleanup state
			_context.DeleteDataPoint(dataPoint);

		}
    }
}
