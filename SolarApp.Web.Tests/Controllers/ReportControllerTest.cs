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

		// TODO: Needs to be refactored
        [Test]
        public void DayGraphShouldShowYesterdaysEnergyOutput()
        {
            // Arrange
            _context.Expect(a => a.GetEnergyOutput(Arg<DateTime>.Is.Anything, Arg<DateTime>.Is.Anything)).Return(new List<EnergyOutput>());
            _context.Expect(a => a.IsDatabasePresent).Return(true);

            // Act
            ViewResult result = _controller.DayGraph() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = (EnergyReadingsViewModel)result.Model;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(true, viewModel.IsDatabaseAvailable);
            //Assert.IsNotNull(viewModel.EnergyReadings);

        }

	}
}
