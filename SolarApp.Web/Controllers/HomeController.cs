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
                    _configuration.Environment);
            }
            else
            {
                viewModel = new SystemStateViewModel("", null, 0, 0,_configuration.Environment);
            }
            return View(viewModel);
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