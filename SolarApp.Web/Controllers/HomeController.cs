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
                    _configuration.Environment);
            }
            else
            {
                viewModel = new SystemStateViewModel("", null, 0, 0,0,0,_configuration.Environment);
            }
            return View(viewModel);
        }

		public ActionResult Yesterday()
		{
			var startDate = DateTime.Now.AddDays(-1).Date;
			var endDate = startDate.AddDays(1);
			var energyReadings = _context.GetEnergyOutput(startDate, endDate);
			EnergyReadingsViewModel viewModel = new EnergyReadingsViewModel(true, energyReadings);
			return View(viewModel);
		}

        public ActionResult Actions()
        {
            SystemActionViewModel viewModel;
            if (_context.IsDatabasePresent)
            {
                string requestWeatherForecast = _context.FindSettingById("RequestWeatherForecast").Value;
                string requestWeatherObservation = _context.FindSettingById("RequestWeatherObservation").Value;
                viewModel = new SystemActionViewModel(true, requestWeatherForecast, requestWeatherObservation);
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
            requestWeatherForecast.Value = "1";
            _context.UpdateSetting(requestWeatherForecast);
            return RedirectToAction("Actions");
        }

        // TODO: CHange this to a HttpPOST
        public ActionResult RequestWeatherObservation()
        {
            var requestWeatherObservation = _context.FindSettingById("RequestWeatherObservation");
            requestWeatherObservation.Value = "1";
            _context.UpdateSetting(requestWeatherObservation);
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