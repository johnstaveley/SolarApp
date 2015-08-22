﻿using System;
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
using System.Web.Script.Serialization;

namespace SolarApp.Web.Unit.Tests.Controllers
{
	[TestFixture]
	public class ReportControllerTest
	{

        private ReportController _controller;
        private IConfiguration _configuration;
        private ISolarAppContext _context;
        private JavaScriptSerializer _serializer;


		[SetUp]
		public void Setup()
		{
			_configuration = MockRepository.GenerateMock<IConfiguration>();
			_context = MockRepository.GenerateMock<ISolarAppContext>();
			_controller = new ReportController(_configuration, _context);
            _serializer = new JavaScriptSerializer();
        }

		[TearDown]
		public void Teardown()
		{
			_configuration.VerifyAllExpectations();
			_context.VerifyAllExpectations();
            _serializer = null;
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

		[Test]
		public void MonthGraphShouldShowViewModelWithDatabaseStatusAndTargetDate()
		{
			// Arrange
			_context.Expect(a => a.IsDatabasePresent).Return(true);

			// Act
			ViewResult result = _controller.MonthGraph() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			var viewModel = (EnergyReadingsViewModel)result.Model;
			Assert.IsNotNull(viewModel);
			Assert.AreEqual(true, viewModel.IsDatabaseAvailable);
			Assert.IsNotNull(viewModel.TargetDate, "Target date should not be null");

		}

		[Test]
		public void DayGraphDataShouldReturnEnergyData()
		{
			// Arrange
			var targetDate = DateTime.Parse("2015-08-01").Date;
			var energyReadings = new List<EnergyOutputDay>(){
				new EnergyOutputDay() { Timestamp = targetDate, CurrentEnergy = 100, DayEnergyInstant = 40 },
                new EnergyOutputDay() { Timestamp = targetDate.AddMinutes(15), CurrentEnergy = 70, DayEnergyInstant = 48 }
			};
			_context.Expect(a => a.GetEnergyOutputByDay(targetDate, targetDate.AddDays(1))).Return(energyReadings);

			// Act
			JsonResult result = _controller.DayGraphData(targetDate) as JsonResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Data);

            var responseObject = _serializer.Serialize(result.Data) as dynamic;
            var response = _serializer.Deserialize<dynamic>(responseObject);
            Assert.AreEqual(1438383600000, response["targetDate"]);
            Assert.AreEqual(3, response["data"][0].Count);
            Assert.AreEqual(3, response["data"][1].Count);
            Assert.AreEqual(88, response["totalProduction"]);
            Assert.AreEqual(100, response["maximumProduction"]);

		}

		[Test]
		public void MonthGraphDataShouldReturnEnergyData()
		{
			// Arrange
			var targetDate = DateTime.Parse("2015-01-01").Date;
			var energyReadings = new List<EnergyOutputMonth>(){
				new EnergyOutputMonth() { Day = targetDate.Day, DayEnergy = 100 },
                new EnergyOutputMonth() { Day = targetDate.Day, DayEnergy = 120 }
			};
			_context.Expect(a => a.GetEnergyOutputByMonth(targetDate, targetDate.AddMonths(1))).Return(energyReadings);

			// Act
			JsonResult result = _controller.MonthGraphData(targetDate) as JsonResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Data);

            var responseObject = _serializer.Serialize(result.Data) as dynamic;
            var response = _serializer.Deserialize<dynamic>(responseObject);
            Assert.AreEqual(1420070400000, response["targetDate"]);
            Assert.AreEqual(2, response["data"][0].Count);
            Assert.AreEqual(2, response["data"][1].Count);
            Assert.AreEqual(220, response["totalProduction"]);
            Assert.AreEqual(120, response["maximumProduction"]);
            Assert.AreEqual("110", response["averageProduction"]);

		}

	}
}
