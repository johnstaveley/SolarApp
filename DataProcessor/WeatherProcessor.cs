using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SolarApp.Persistence;
using Newtonsoft.Json;
using SolarApp.Model;
using SolarApp.DataProcessor.Utility;
using SolarApp.DataProcessor.Utility.Interfaces;
using System.Net;

namespace SolarApp.DataProcessor
{
	public class WeatherProcessor
	{

		private IConfiguration _configuration { get; set; }
		private ISolarAppContext _context { get; set; }
		private IServices _services { get; set; }

		public WeatherProcessor(IConfiguration configuration, ISolarAppContext context, IServices services)
		{
			_configuration = configuration;
			_context = context;
			_services = services;
		}

		public string GetWeatherForecast()
		{
			var requestWeatherForecast = _context.FindSettingById("RequestWeatherForecast");
			string forecastDownloaded = null;
			if (requestWeatherForecast != null && requestWeatherForecast.Value == "1")
			{
				var metOfficeLocationForecastUrl = string.Format("{0}wxfcs/all/json/{1}?res=3hourly&key={2}", _configuration.MetOfficeUrl, _configuration.MetOfficeForecastLocationId, _configuration.MetOfficeApiKey);
				forecastDownloaded = string.Format("{0:ddMMyyyy-HHmmss}", DateTime.Now);
				var weatherForecastJson = _services.WebRequestForJson(metOfficeLocationForecastUrl);
				_context.InsertWeatherForecast(new WeatherForecast() { Id = forecastDownloaded, Data = weatherForecastJson });
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
