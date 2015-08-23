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
			var energyReadings = _context.GetEnergyOutputByDay(startDate, endDate);
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

		public ActionResult MonthGraph(DateTime? targetDate = null)
		{
			var isDatabasePresent = _context.IsDatabasePresent;
			if (targetDate == null)
			{
				targetDate = DateTime.Now;
				targetDate = targetDate.Value.Date.AddDays(1 - targetDate.Value.Day);
			}
			EnergyReadingsViewModel viewModel = new EnergyReadingsViewModel(isDatabasePresent, targetDate.Value);
			return View(viewModel);
		}

		public JsonResult MonthGraphData(DateTime targetDate)
		{
			var startDate = targetDate.Date.AddDays(1-targetDate.Day);
			var endDate = startDate.AddMonths(1);
			var energyReadings = _context.GetEnergyOutputByMonth(startDate, endDate);
			double averageProduction = 0;
			double totalProduction = 0;
			double maximumProduction = 0;
			if (energyReadings.Count > 0)
			{
				averageProduction = energyReadings.Average(e => e.DayEnergy);
				totalProduction = energyReadings.Sum(e => e.DayEnergy);
				maximumProduction = energyReadings.Max(e => e.DayEnergy);
			}
			return Json(new
			{
				targetDate = startDate.ToJavaScriptMilliseconds(),
				data = energyReadings
					.Select(a => new { timestamp = targetDate.AddDays(a.Day-1).ToJavaScriptMilliseconds(), dayEnergy = a.DayEnergy }),
				totalProduction = totalProduction,
				maximumProduction = maximumProduction,
				averageProduction = averageProduction.ToString("0")
			}, JsonRequestBehavior.AllowGet);
		}

		public ActionResult YearGraph(DateTime? targetDate = null)
		{
			var isDatabasePresent = _context.IsDatabasePresent;
			if (targetDate == null)
			{
				targetDate = DateTime.Now;
				targetDate = targetDate.Value.Date.AddMonths(1 - targetDate.Value.Month).AddDays(1 - targetDate.Value.Day);
			}
			EnergyReadingsViewModel viewModel = new EnergyReadingsViewModel(isDatabasePresent, targetDate.Value);
			return View(viewModel);
		}

		public JsonResult YearGraphData(DateTime targetDate)
		{
			var startDate = targetDate.Date.AddMonths(1 - targetDate.Month).AddDays(1 - targetDate.Day);
			var endDate = startDate.AddYears(1);
			var energyReadings = _context.GetEnergyOutputByYear(startDate, endDate);
			double averageProduction = 0;
			double totalProduction = 0;
			double maximumProduction = 0;
			if (energyReadings.Count > 0)
			{
				averageProduction = energyReadings.Average(e => e.MonthEnergy);
				totalProduction = energyReadings.Sum(e => e.MonthEnergy);
				maximumProduction = energyReadings.Max(e => e.MonthEnergy);
			}
			return Json(new
			{
				targetDate = startDate.ToJavaScriptMilliseconds(),
				data = energyReadings
					.Select(a => new { timestamp = targetDate.AddMonths(a.Month - 1).ToJavaScriptMilliseconds(), monthEnergy = a.MonthEnergy }),
				totalProduction = totalProduction,
				maximumProduction = maximumProduction,
				averageProduction = averageProduction.ToString("0")
			}, JsonRequestBehavior.AllowGet);
		}

	}
}