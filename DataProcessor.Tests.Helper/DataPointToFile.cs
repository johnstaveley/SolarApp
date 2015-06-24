using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarApp.Model;
using Newtonsoft.Json;

namespace SolarApp.DataProcessor.Tests.Helper
{
	public static class DataPointToFile
	{
		public static void SaveAsJson(this DataPoint dataPoint, string fileName)
		{
			var json = JsonConvert.SerializeObject(dataPoint);
			File.WriteAllText(fileName, json);
		}
	}
}
