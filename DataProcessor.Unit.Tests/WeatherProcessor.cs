using NUnit.Framework;
using SolarApp.Model;
using SolarApp.DataProcessor.Utility.Interfaces;
using Rhino.Mocks;
using SolarApp.Persistence;
using System.Linq;
using SolarApp.Utility.Interfaces;

namespace SolarApp.DataProcessor.Unit.Tests
{
	[TestFixture]
	public class WeatherProcessorTest
	{

        IConfiguration configuration;
		ILogger logger;
        IServices services;
        ISolarAppContext solarAppContext;

        [SetUp]
        public void Setup()
        {
            configuration = MockRepository.GenerateMock<IConfiguration>();
			logger = MockRepository.GenerateMock<ILogger>();
            services = MockRepository.GenerateMock<IServices>();
            solarAppContext = MockRepository.GenerateMock<ISolarAppContext>();
        }

        [TearDown]
        public void Teardown()
        {
            configuration = null;
            services = null;
            solarAppContext = null;
        }

        public void VerifyAllExpectation()
        {
            configuration.VerifyAllExpectations();
            services.VerifyAllExpectations();
            solarAppContext.VerifyAllExpectations();
        }

		[Test]
		public void Given_WeatherForecastNotRequested_When_Process_Then_NoForecastsDownloaded()
		{
			// Arrange
            solarAppContext.Expect(c => c.FindSettingById("RequestWeatherForecast")).Return(new Setting() { Id = "RequestWeatherForecast", Value = "0" });

			// Act
			var weatherProcessor = new WeatherProcessor(configuration, solarAppContext, logger, services);
			var result = weatherProcessor.GetWeatherForecast();

			// Assert
			Assert.IsNull(result, "No weather forecast download should have taken place");
            VerifyAllExpectation();

		}

		[Test]
		public void Given_WeatherForecastRequested_When_Process_Then_ForecastDownloaded()
		{
			// Arrange
			services.Expect(s => s.WebRequestForJson(Arg<string>.Is.Anything)).Return("{}");
            solarAppContext.Expect(c => c.FindSettingById("RequestWeatherForecast")).Return(new Setting() { Id = "RequestWeatherForecast", Value = "1" });

			// Act
			var weatherProcessor = new WeatherProcessor(configuration, solarAppContext, logger, services);
			var weatherForecastId = weatherProcessor.GetWeatherForecast();

			// Assert
			Assert.IsTrue(weatherForecastId.HasValue, "Forecast should have been downloaded");
            solarAppContext.AssertWasCalled(c => c.InsertWeatherForecast(Arg<WeatherForecast>.Is.Anything));
            solarAppContext.AssertWasCalled(c => c.UpdateSetting(Arg<Setting>.Is.Anything));
            VerifyAllExpectation();
		}

        [Test]
        public void Given_WeatherObservationNotRequested_When_Process_Then_NoObservationDownloaded()
        {
            // Arrange
            solarAppContext.Expect(c => c.FindSettingById("RequestWeatherObservation")).Return(new Setting() { Id = "RequestWeatherObservation", Value = "0" });

            // Act
			var weatherProcessor = new WeatherProcessor(configuration, solarAppContext, logger, services);
            var result = weatherProcessor.GetWeatherObservation();

            // Assert
            Assert.IsNull(result, "No weather observation download should have taken place");
            VerifyAllExpectation();
        }

        [Test]
        public void Given_WeatherObservationRequested_When_Process_Then_ObservationDownloaded()
        {
            // Arrange
            services.Expect(s => s.WebRequestForJson(Arg<string>.Is.Anything)).Return("{}");
            solarAppContext.Expect(c => c.FindSettingById("RequestWeatherObservation")).Return(new Setting() { Id = "RequestWeatherObservation", Value = "1" });

            // Act
			var weatherProcessor = new WeatherProcessor(configuration, solarAppContext, logger, services);
            var weatherObservationId = weatherProcessor.GetWeatherObservation();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(weatherObservationId), "Observation should have been downloaded");
            solarAppContext.AssertWasCalled(c => c.InsertWeatherObservation(Arg<WeatherObservation>.Is.Anything));
            solarAppContext.AssertWasCalled(c => c.UpdateSetting(Arg<Setting>.Is.Anything));
            VerifyAllExpectation();

        }
	}
}
