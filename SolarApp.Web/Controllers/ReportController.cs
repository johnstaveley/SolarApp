using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolarApp.Persistence;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.DataProcessor.Utility.Classes;
using SolarApp.Web.ViewModel;
using SolarApp.Web.Extensions;

namespace SolarApp.Web.Controllers
{

	public class ReportController : Controller
	{
		private ISolarAppContext _context { get; set; }
		private IConfiguration _configuration { get; set; }

		public ReportController(IConfiguration configuration, ISolarAppContext context)
		{
            _configuration = configuration;
            _context = context;
		}

		public ActionResult DayGraph(DateTime? targetDate = null)
		{
			var isDatabasePresent = _context.IsDatabasePresent;
			EnergyReadingsViewModel viewModel = new EnergyReadingsViewModel(isDatabasePresent, targetDate ?? DateTime.Now.AddDays(-1).Date);
			return View(viewModel);
		}

		public JsonResult DayGraphData(DateTime targetDate)
		{
			var startDate = targetDate.Date;
			var endDate = startDate.AddDays(1);
			var energyReadings = _context.GetEnergyOutput(startDate, endDate);
			double totalProduction = 0;
			double maximumProduction = 0;
			if (energyReadings.Count > 0)
			{
				totalProduction = energyReadings.Sum(e => e.DayEnergyInstant);
				maximumProduction = energyReadings.Max(e => e.CurrentEnergy);
			}
			return Json(new { 
				targetDate = startDate.ToJavaScriptMilliseconds(), 
				data = energyReadings
					.Select(a => new { timestamp = a.Timestamp.ToJavaScriptMilliseconds(), currentEnergy = a.CurrentEnergy, dayEnergyInstant = a.DayEnergyInstant }),
				totalProduction = totalProduction,
				maximumProduction = maximumProduction
			}, JsonRequestBehavior.AllowGet);
		}

	}
}