using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolarApp.Persistence;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.DataProcessor.Utility.Classes;

namespace SolarApp.Web.Controllers
{

	public class HomeController : Controller
	{
		private ISolarAppContext _context { get; set; }
		private IConfiguration _configuration { get; set; }

		public HomeController()
		{
			_configuration = new SolarApp.DataProcessor.Utility.Classes.Configuration();
			_context = new SolarAppContext(_configuration);
		}

		public ActionResult Index()
		{
			ViewBag.LatestMeterReading = _context.GetLatestEnergyReading();
            ViewBag.Environment = _configuration.Environment;
			return View();
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