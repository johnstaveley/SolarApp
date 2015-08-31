using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SolarApp.Model;

namespace SolarApp.DataProcessor.Unit.Tests.Model
{
	public class SunTimeTests
	{


		private SunTime GetSunData()
		{
			var sunTime = new SunTime()
			{
				Date = DateTime.ParseExact("2015-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture)
			};
			sunTime.Sunrise = sunTime.Date.AddHours(5).AddMinutes(6);
			sunTime.Sunset = sunTime.Sunrise.AddHours(12);
			return sunTime;
		}

		[Test]
		public void Given_SunriseAndSunsetTimes_When_GetAzimuth_Then_CorrectAzimuthIsReturned()
		{
			var sunTime = GetSunData();

			Assert.AreEqual(DateTime.ParseExact("2015-08-01 11:06:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sunTime.SunAzimuth);
		}

		[Test]
		public void Given_SunData_When_GetIntensityAtAzimuth_Then_Returns100Percent()
		{

			var sunTime = GetSunData();
			var targetDate = DateTime.ParseExact("2015-08-01 11:06:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

			Assert.AreEqual(100, sunTime.SunIntensity(targetDate));
		}

		[Test]
		public void Given_SunData_When_GetIntensityAtSunrise_Then_Returns0Percent()
		{

			var sunTime = GetSunData();
			var targetDate = sunTime.Sunrise;

			Assert.AreEqual(0, sunTime.SunIntensity(targetDate));
		}

		[Test]
		public void Given_SunData_When_GetIntensityAtSunset_Then_Returns0Percent()
		{

			var sunTime = GetSunData();
			var targetDate = sunTime.Sunset;

			Assert.AreEqual(0, sunTime.SunIntensity(targetDate));
		}

		[Test]
		public void Given_SunData_When_GetIntensityBeforeSunrise_Then_Returns0Percent()
		{

			var sunTime = GetSunData();
			var targetDate = sunTime.Sunrise.AddHours(-1);

			Assert.AreEqual(0, sunTime.SunIntensity(targetDate));
		}

		[Test]
		public void Given_SunData_When_GetIntensityAfterSunset_Then_Returns0Percent()
		{

			var sunTime = GetSunData();
			var targetDate = sunTime.Sunset.AddHours(1);

			Assert.AreEqual(0, sunTime.SunIntensity(targetDate));
		}

		// TODO: Change for gaussian distribution
		[Test]
		public void Given_SunData_When_GetIntensityHalfwayToAzimuth_Then_Returns50Percent()
		{

			var sunTime = GetSunData();
			var targetDate = DateTime.ParseExact("2015-08-01 08:06:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

			Assert.AreEqual(50, sunTime.SunIntensity(targetDate));
		}

		// TODO: Change for gaussian distribution
		[Test]
		public void Given_SunData_When_GetIntensityHalfwayPastAzimuth_Then_Returns50Percent()
		{

			var sunTime = GetSunData();
			var targetDate = DateTime.ParseExact("2015-08-01 14:06:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

			Assert.AreEqual(50, sunTime.SunIntensity(targetDate));
		}

	}
}
