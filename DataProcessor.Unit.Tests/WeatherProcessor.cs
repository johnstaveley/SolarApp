﻿using NUnit.Framework;
using SolarApp.Model;
using SolarApp.DataProcessor.Utility.Interfaces;
using Rhino.Mocks;
using SolarApp.Persistence;
using System.Linq;

namespace SolarApp.DataProcessor.Unit.Tests
{
	[TestFixture]
	public class WeatherProcessorTest
	{

		[Test]
		public void Given_WeatherForecastNotRequested_When_Process_Then_NoForecastsDownloaded()
		{
			// Arrange
			var configuration = MockRepository.GenerateMock<IConfiguration>();
			var services = MockRepository.GenerateMock<IServices>();
			var solarAppContext = MockRepository.GenerateMock<ISolarAppContext>();
			solarAppContext.Expect(c => c.FindSettingById("RequestWeatherForecast")).Return(new Setting() { Id = "", Value = "0" });

			// Act
			var weatherProcessor = new WeatherProcessor(configuration, solarAppContext, services);
			var result = weatherProcessor.GetWeatherForecast();

			// Assert
			Assert.IsNull(result, "No weather download should have taken place");
			configuration.VerifyAllExpectations();
			solarAppContext.VerifyAllExpectations();

		}

		[Test]
		public void Given_WeatherForecastRequested_When_Process_Then_ForecastDownloaded()
		{
			// Arrange
			var configuration = MockRepository.GenerateMock<IConfiguration>();
			var services = MockRepository.GenerateMock<IServices>();
			var solarAppContext = MockRepository.GenerateMock<ISolarAppContext>();
			services.Expect(s => s.WebRequestForJson(Arg<string>.Is.Anything)).Return("{}");
			solarAppContext.Expect(c => c.FindSettingById("RequestWeatherForecast")).Return(new Setting() { Id = "", Value = "1" });

			// Act
			var weatherProcessor = new WeatherProcessor(configuration, solarAppContext, services);
			var weatherForecastId = weatherProcessor.GetWeatherForecast();

			// Assert
			Assert.IsTrue(!string.IsNullOrEmpty(weatherForecastId), "Forecast should have been downloaded");
			//TODO: Assert database changes
			configuration.VerifyAllExpectations();
			services.VerifyAllExpectations();
			solarAppContext.VerifyAllExpectations();

		}

	}
}
