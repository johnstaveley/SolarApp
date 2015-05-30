using System;
using TechTalk.SpecFlow;
using System.Configuration;
using Persistence;
using NUnit.Framework;
using Model;

namespace DataProcessor.Integration.Tests
{
    [Binding]
    public class PersistenceSteps
    {

        private string _connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ToString();
        private string _databaseName { get; set; }
        private Setting _setting { get; set; }
        private SolarAppContext _context { get; set; }
        
       
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
            _context = new SolarAppContext(_connectionString, _databaseName);
        }

        [When(@"I persist the value to the database")]
        public void WhenIPersistTheValueToTheDatabase()
        {
            _context.InsertSetting(_setting);
        }
        
        [Then(@"the random value should be retrievable from the database")]
        public void ThenTheRandomValueShouldBeRetrievableFromTheDatabase()
        {
            Assert.IsTrue(_context.FindSetting(_setting._id).Count == 1, string.Format("Could not find data value {0}", _setting._id));
        }

        [AfterScenario]
        public void TearDown()
        {
            if (_context != null && _setting != null)
            {
                _context.DeleteSetting(_setting);
            }
        }
    }
}
