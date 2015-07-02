﻿using System;
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
		private IFileSystem _fileSystem { get; set; }
		private ISolarAppContext _context { get; set; }

		public WeatherProcessor(IConfiguration configuration, IFileSystem fileSystem, ISolarAppContext context)
		{
			_configuration = configuration;
			_context = context;
			_fileSystem = fileSystem;
		}

		public void Process()
		{
			var metOfficeLocationForecastUrl = string.Format("{0}{1}?res=3hourly&key={2}", _configuration.MetOfficeUrl, _configuration.MetOfficeLocationId, _configuration.MetOfficeApiKey);
			var request = WebRequest.Create(metOfficeLocationForecastUrl);
			request.ContentType = "application/json; charset=utf-8";
			string weatherForecastJson;
			var response = (HttpWebResponse) request.GetResponse();

			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				weatherForecastJson = sr.ReadToEnd();
			}
			var id = string.Format("{0:ddMMyyyy-HHmmss}", DateTime.Now);
			_context.InsertWeatherForecast(new WeatherForecast() {Id = id , Data = weatherForecastJson});
		}

	}
}