﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DataProcessor.Utility.Interfaces;

namespace DataProcessor.Utility.Classes
{
	public class Configuration : IConfiguration
	{

		public string NewFilePollPath { get; set; }
		public string MongoDatabaseName { get; set; }
		public string MongoConnectionString { get; set; }

		public Configuration()
		{
			MongoDatabaseName = System.Configuration.ConfigurationManager.AppSettings["DatabaseName"];
			NewFilePollPath = System.Configuration.ConfigurationManager.AppSettings["NewFilePollPath"];
			MongoConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
		}

	}
}
