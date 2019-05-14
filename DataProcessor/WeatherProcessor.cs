using Newtonsoft.Json;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.Model;
using SolarApp.Persistence;
using SolarApp.Utility.Interfaces;
using System;

namespace SolarApp.DataProcessor
{
	public class WeatherProcessor
	{

		private readonly IConfiguration _configuration;
		private readonly ISolarAppContext _context;
		private readonly ILogger _logger;
		private readonly IServices _services;

		public WeatherProcessor(IConfiguration configuration, ISolarAppContext context, ILogger logger, IServices services)
		{
			_configuration = configuration;
			_context = context;
			_logger = logger;
			_services = services;
		}

		public DateTime? GetWeatherForecast()
		{
			var requestWeatherForecast = _context.FindSettingById("RequestWeatherForecast");
			DateTime? forecastDownloaded = null;
			if (requestWeatherForecast != null && requestWeatherForecast.Value == "1")
			{
				var metOfficeLocationForecastUrl = string.Format("{0}wxfcs/all/json/{1}?res=3hourly&key={2}", _configuration.MetOfficeUrl, _configuration.MetOfficeForecastLocationId, _configuration.MetOfficeApiKey);
				_logger.Debug(string.Format("About to be http get to {0}", metOfficeLocationForecastUrl));
				var weatherForecastJson = _services.WebRequestForJson(metOfficeLocationForecastUrl);
				forecastDownloaded = DateTime.UtcNow;
                WeatherForecast weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(weatherForecastJson);
                weatherForecast.Id = string.Format("{0:yyyy-MM-ddTHHmmss}", forecastDownloaded.Value);
                _context.InsertWeatherForecast(weatherForecast);
				requestWeatherForecast.Value = "0";
				_context.UpdateSetting(requestWeatherForecast);
			}
			return forecastDownloaded;
		}

		public string GetWeatherObservation()
		{

			var requestWeatherObservation = _context.FindSettingById("RequestWeatherObservation");
			string observationDownloaded = null;
			if (requestWeatherObservation != null && requestWeatherObservation.Value == "1")
			{
				var metOfficeLocationObservationUrl = string.Format("{0}wxobs/all/json/{1}?res=hourly&key={2}", _configuration.MetOfficeUrl, _configuration.MetOfficeObservationLocationId, _configuration.MetOfficeApiKey);
				observationDownloaded = string.Format("{0:ddMMyyyy-HHmmss}", DateTime.Now);
				var weatherObservationJson = _services.WebRequestForJson(metOfficeLocationObservationUrl);
				_context.InsertWeatherObservation(new WeatherObservation() { Id = observationDownloaded, Data = weatherObservationJson });
				requestWeatherObservation.Value = "0";
				_context.UpdateSetting(requestWeatherObservation);
			}
			return observationDownloaded;
		}

	}
}
