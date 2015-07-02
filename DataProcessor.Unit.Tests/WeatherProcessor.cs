using NUnit.Framework;
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
			var solarAppContext = MockRepository.GenerateMock<ISolarAppContext>();
			solarAppContext.Expect(c => c.FindSettingById("RequestWeatherForecast")).Return(new Setting() { Id = "", Value = "0" });

			// Act
			var weatherProcessor = new WeatherProcessor(configuration, solarAppContext);
			var results = weatherProcessor.Process();

			// Assert
			Assert.AreEqual(0, results.Count);
			configuration.VerifyAllExpectations();
			solarAppContext.VerifyAllExpectations();
				
			}

	}
}
