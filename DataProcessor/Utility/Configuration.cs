using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataProcessor.Utility
{
	public class Configuration : DataProcessor.Utility.IConfiguration
	{

		public string NewFilePollPath { get; set; }

		public Configuration()
		{
			NewFilePollPath = System.Configuration.ConfigurationManager.AppSettings["NewFilePollPath"];
		}

	}
}
