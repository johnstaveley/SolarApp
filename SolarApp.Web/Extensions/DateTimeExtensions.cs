using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarApp.Web.Extensions
{
	public static class DateTimeExtensions
	{
		private static readonly long DatetimeMinTimeTicks =
			(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

		public static long ToJavaScriptMilliseconds(this DateTime dt)
		{
			return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
		}
	}
}