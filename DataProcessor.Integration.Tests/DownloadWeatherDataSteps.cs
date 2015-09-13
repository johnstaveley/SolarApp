using System;
using System.Collections.Generic;
using NUnit.Framework;
using SolarApp.DataProcessor.Tests.Helper;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.Model;
using SolarApp.Persistence;
using TechTalk.SpecFlow;
using System.Linq;
using System.Globalization;
using SolarApp.Utility.Interfaces;

namespace SolarApp.DataProcessor.Integration.Tests
{
    [Binding]
    public class DownloadWeatherDataSteps
    {
        [Given(@"I have credentials to the met office data point system")]
        public void GivenIHaveCredentialsToTheMetOfficeDataPointSystem()
        {
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			if (string.IsNullOrEmpty(configuration.MetOfficeApiKey)) { Assert.Inconclusive("MetOfficeApiKey not set"); }
			if (string.IsNullOrEmpty(configuration.MetOfficeUrl)) { Assert.Inconclusive("MetOfficeUrl not set"); }
		}
        
        [Given(@"a weather forecast has been marked as requested")]
        public void GivenAWeatherForecastHasBeenMarkedAsRequested()
        {
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var forecastRequestSetting = context.FindSettingById("RequestWeatherForecast");
			forecastRequestSetting.Value = "1";
			context.UpdateSetting(forecastRequestSetting);		
		}

		[Given(@"a weather observation has been marked as requested")]
		public void GivenAWeatherObservationHasBeenMarkedAsRequested()
		{
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var observationRequestSetting = context.FindSettingById("RequestWeatherObservation");
			observationRequestSetting.Value = "1";
			context.UpdateSetting(observationRequestSetting);
		}

        [Given(@"I have a target met office forecast area")]
        public void GivenIHaveATargetMetOfficeForecastArea()
        {
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			if (string.IsNullOrEmpty(configuration.MetOfficeForecastLocationId)) { Assert.Inconclusive("MetOfficeForecastLocationId not set"); }
		}

		[Given(@"I have a target met office observation area")]
		public void GivenIHaveATargetMetOfficeObservationArea()
		{
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			if (string.IsNullOrEmpty(configuration.MetOfficeObservationLocationId)) { Assert.Inconclusive("MetOfficeObservationLocationId not set"); }
		}        

        [When(@"I download a weather forecast")]
        public void WhenIDownloadAWeatherForecast()
        {
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var logger = ScenarioContext.Current.Get<ILogger>();
			var services = ScenarioContext.Current.Get<IServices>();
			var weather = new WeatherProcessor(configuration, context, logger, services);
			var result = weather.GetWeatherForecast();
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var weatherForecast = new WeatherForecast();
			weatherForecast.Id = result.Value.ToString("yyyy-MM-ddTHHmmss");
			dataItemsToTrack.Add(new DataItem(weatherForecast));
			ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
        }

		[When(@"I download a weather observation")]
		public void WhenIDownloadAWeatherObservation()
		{
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var logger = ScenarioContext.Current.Get<ILogger>();
			var services = ScenarioContext.Current.Get<IServices>();
			var weather = new WeatherProcessor(configuration, context, logger, services);
			var result = weather.GetWeatherObservation();
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var weatherObservation = new WeatherObservation();
			weatherObservation.Id = result;
			dataItemsToTrack.Add(new DataItem(weatherObservation));
			ScenarioContext.Current.Set<List<DataItem>>(dataItemsToTrack, "DataItemsToTrack");
		}

        [Then(@"The weather forecast is stored in the database")]
        public void ThenTheWeatherForecastIsStoredInTheDatabase()
        {
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var weatherForecast = context.FindWeatherForecastById(dataItemsToTrack.First().Id);
			Assert.IsNotNull(weatherForecast, "Weather forecast should have been stored");

		}

		[Then(@"The weather observation is stored in the database")]
		public void ThenTheWeatherObservationIsStoredInTheDatabase()
		{
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			var weatherObservation = context.FindWeatherObservationById(dataItemsToTrack.First().Id);
			Assert.IsNotNull(weatherObservation, "Weather observation should have been stored");
			Assert.IsTrue(weatherObservation.Data.Contains("\"type\":\"Obs\""), "Data returned is not of the correct type");

		}

    }
}
