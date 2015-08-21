using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using SolarApp.Web;
using SolarApp.Web.Controllers;
using SolarApp.DataProcessor.Utility.Interfaces;
using Rhino.Mocks;
using SolarApp.Persistence;
using SolarApp.Model;
using SolarApp.Web.ViewModel;
using Newtonsoft.Json;

namespace SolarApp.Web.Unit.Tests.Controllers
{
	[TestFixture]
	public class ReportControllerTest
	{

        private ReportController _controller;
        private IConfiguration _configuration;
        private ISolarAppContext _context;

		[SetUp]
		public void Setup()
		{
			_configuration = MockRepository.GenerateMock<IConfiguration>();
			_context = MockRepository.GenerateMock<ISolarAppContext>();
			_controller = new ReportController(_configuration, _context);
		}

		[TearDown]
		public void Teardown()
		{
			_configuration.VerifyAllExpectations();
			_context.VerifyAllExpectations();
		}

        [Test]
        public void DayGraphShouldShowViewModelWithDatabaseStatusAndTargetDate()
        {
            // Arrange
            _context.Expect(a => a.IsDatabasePresent).Return(true);

            // Act
            ViewResult result = _controller.DayGraph() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = (EnergyReadingsViewModel)result.Model;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(true, viewModel.IsDatabaseAvailable);
            Assert.IsNotNull(viewModel.TargetDate, "Target date should not be null");

        }

		// TODO: Finish this test
		[Test]
		public void DayGraphDataShouldReturnEnergyData()
		{
			// Arrange
			var targetDate = DateTime.Now.AddDays(-7).Date;
			var energyReadings = new List<EnergyOutput>(){
				new EnergyOutput() { Timestamp = targetDate, CurrentEnergy = 100, DayEnergyInstant = 40 }
			};
			_context.Expect(a => a.GetEnergyOutput(targetDate, targetDate.AddDays(1))).Return(energyReadings);

			// Act
			JsonResult result = _controller.DayGraphData(targetDate) as JsonResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Data);
			IDictionary<string, object> wrapper = (IDictionary<string, object>)new System.Web.Routing.RouteValueDictionary(result.Data);

			Assert.IsNotNull(wrapper["targetDate"]);
			Assert.IsTrue(Convert.ToInt64((wrapper["targetDate"]).ToString()) > 10000000, "Target date is not set");
			Assert.IsNotNull(wrapper["data"]);
			Assert.IsNotNull(wrapper["totalProduction"]);
			Assert.IsNotNull(wrapper["maximumProduction"]);
			var outputEnergyReadings = (IDictionary<string, object>) new System.Web.Routing.RouteValueDictionary(wrapper["data"]);
			//Assert.AreEqual("Current", outputEnergyReadings.First().Key);

		}

	}
}
