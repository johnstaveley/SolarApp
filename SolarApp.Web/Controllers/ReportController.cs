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
			EnergyReadingsViewModel viewModel = new EnergyReadingsViewModel(true, targetDate ?? DateTime.Now.AddDays(-1).Date);
			return View(viewModel);
		}

		public JsonResult DayGraphData(DateTime targetDate)
		{
			var startDate = targetDate.Date;
			var endDate = startDate.AddDays(1);
			var energyReadings = _context.GetEnergyOutput(startDate, endDate);
			return Json(new { targetDate = startDate.ToJavaScriptMilliseconds(), data = energyReadings.Select(a => new { timestamp = a.Timestamp.ToJavaScriptMilliseconds(), currentEnergy = a.CurrentEnergy, dayEnergyInstant = a.DayEnergyInstant }) }, JsonRequestBehavior.AllowGet);
		}


	}
}