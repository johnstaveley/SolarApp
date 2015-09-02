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
using SolarApp.Utility.Interfaces;

namespace SolarApp.Web.Unit.Tests.Controllers
{
	[TestFixture]
	public class HomeControllerTest
	{

        private HomeController _controller;
        private IConfiguration _configuration;
        private ISolarAppContext _context;
		private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _configuration = MockRepository.GenerateMock<IConfiguration>();
            _context = MockRepository.GenerateMock<ISolarAppContext>();
			_logger = MockRepository.GenerateMock<ILogger>();
			_controller = new HomeController(_configuration, _context, _logger);
        }

        [TearDown]
        public void Teardown()
        {
            _configuration.VerifyAllExpectations();
			_context.VerifyAllExpectations();
        }

		[Test]
		public void Index()
		{
			// Arrange
			// Act
			ViewResult result = _controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}


        [Test]
        public void RequestWeatherForecastActionShouldChangeSettingInDatabase()
        {
            // Arrange
            _context.Expect(a => a.FindSettingById("RequestWeatherForecast")).Return(new Setting() { Value = "0", Id = "RequestWeatherForecast" });

            // Act
            var result = _controller.RequestWeatherForecast();

            // Assert
            Assert.IsNotNull(result);
            var redirect = (RedirectToRouteResult)result;
            Assert.IsTrue(redirect.RouteValues.Any(i => ((string) i.Value) == "Actions"));
            _context.AssertWasCalled(
                a => a.UpdateSetting(Arg<Setting>.Matches(
                    b => b.Id == "RequestWeatherForecast" && b.Value == "1")));

        }

        [Test]
        public void RequestWeatherObservationActionShouldChangeSettingInDatabase()
        {
            // Arrange
            _context.Expect(a => a.FindSettingById("RequestWeatherObservation")).Return(new Setting() { Value = "0", Id = "RequestWeatherObservation" });

            // Act
            var result = _controller.RequestWeatherObservation();

            // Assert
            Assert.IsNotNull(result);
            var redirect = (RedirectToRouteResult)result;
            Assert.IsTrue(redirect.RouteValues.Any(i => ((string)i.Value) == "Actions"));
            _context.AssertWasCalled(
                a => a.UpdateSetting(Arg<Setting>.Matches(
                    b => b.Id == "RequestWeatherObservation" && b.Value == "1")));

        }

        [Test]
        public void ActionsShouldShowSystemActionValues()
        {
            // Arrange
            _context.Expect(a => a.FindSettingById("RequestWeatherForecast")).Return(new Setting() { Value = "0" });
            _context.Expect(a => a.FindSettingById("RequestWeatherObservation")).Return(new Setting() { Value = "1" });
            _context.Expect(a => a.IsDatabasePresent).Return(true);

            // Act
            ViewResult result = _controller.Actions() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = (SystemActionViewModel)result.Model;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("No", viewModel.RequestWeatherForecast);
            Assert.AreEqual(true, viewModel.CanRequestWeatherForecast);
            Assert.AreEqual("Pending", viewModel.RequestWeatherObservation);
            Assert.AreEqual(false, viewModel.CanRequestWeatherObservation);

        }

        [Test]
        public void StatusShouldShowSystemStatusValues()
        {
            // Arrange
            _context.Expect(a => a.FindSettingById("LastRunDate")).Return(new Setting());
            _context.Expect(a => a.GetLatestEnergyReading()).Return(DateTime.Now);
            _context.Expect(a => a.GetNumberOfDataPoints()).Return(10);
            _context.Expect(a => a.GetNumberOfFailedData()).Return(5);
            _context.Expect(a => a.IsDatabasePresent).Return(true);
            _configuration.Expect(a => a.Environment).Return("UnitTest");

            // Act
            ViewResult result = _controller.Status() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = (SystemStateViewModel)result.Model;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("UnitTest", viewModel.Environment);
            Assert.AreEqual(10, viewModel.NumberOfDataPoints);

        }

		[Test]
		public void About()
		{
			// Arrange
			// Act
			ViewResult result = _controller.About() as ViewResult;

			// Assert
			Assert.AreEqual("Your application description page.", result.ViewBag.Message);
		}

		[Test]
		public void Contact()
		{
			// Arrange
			// Act
			ViewResult result = _controller.Contact() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
