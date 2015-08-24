using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolarApp.Persistence;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.DataProcessor.Utility.Classes;
using SolarApp.Web.ViewModel;

namespace SolarApp.Web.Controllers
{

	public class HomeController : Controller
	{
		private ISolarAppContext _context { get; set; }
		private IConfiguration _configuration { get; set; }

		public HomeController(IConfiguration configuration, ISolarAppContext context)
		{
            _configuration = configuration;
            _context = context;
		}

		public ActionResult Index()
		{
			return View();
		}

        public ActionResult Status()
        {

            SystemStateViewModel viewModel;

            if (_context.IsDatabasePresent)
            {
                viewModel = new SystemStateViewModel(
                    _context.FindSettingById("LastRunDate").Value,
                    _context.GetLatestEnergyReading(),
                    _context.GetNumberOfDataPoints(),
                    _context.GetNumberOfFailedData(),
                    _context.GetNumberOfWeatherForecasts(),
                    _context.GetNumberOfWeatherObservations(),
                    _configuration.Environment, null, null);
            }
            else
            {
                viewModel = new SystemStateViewModel("", null, 0, 0,0,0,_configuration.Environment, null, null);
            }
            return View(viewModel);
        }

        public ActionResult Actions()
        {
            SystemActionViewModel viewModel;
            if (_context.IsDatabasePresent)
            {
				string requestWeatherForecastFlag = "";
				string requestWeatherObservationFlag = "";
				var requestWeatherForecast = _context.FindSettingById("RequestWeatherForecast");
				var requestWeatherObservation = _context.FindSettingById("RequestWeatherObservation");
				if (requestWeatherForecast != null) requestWeatherForecastFlag = requestWeatherForecast.Value;
				if (requestWeatherObservation != null) requestWeatherObservationFlag = requestWeatherObservation.Value;
                viewModel = new SystemActionViewModel(true, requestWeatherForecastFlag, requestWeatherObservationFlag);
            }
            else
            {
                viewModel = new SystemActionViewModel(false, "Unknown", "Unknown");
            }
            return View(viewModel);
        }

        // TODO: CHange this to a HttpPOST
        public ActionResult RequestWeatherForecast()
        {
            var requestWeatherForecast = _context.FindSettingById("RequestWeatherForecast");
			if (requestWeatherForecast == null)
			{
				requestWeatherForecast = new Model.Setting() { Id = "RequestWeatherForecast", Value = "1" };
				_context.InsertSetting(requestWeatherForecast);
			}
			else
			{
				requestWeatherForecast.Value = "1";
				_context.UpdateSetting(requestWeatherForecast);
			}
            return RedirectToAction("Actions");
        }

        // TODO: CHange this to a HttpPOST
        public ActionResult RequestWeatherObservation()
        {
            var requestWeatherObservation = _context.FindSettingById("RequestWeatherObservation");
			if (requestWeatherObservation == null)
			{
				requestWeatherObservation = new Model.Setting() { Id = "RequestWeatherObservation", Value = "1" };
				_context.InsertSetting(requestWeatherObservation);
			}
			else
			{
				requestWeatherObservation.Value = "1";
				_context.UpdateSetting(requestWeatherObservation);
			}
            return RedirectToAction("Actions");
        }

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}