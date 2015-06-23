using System;

namespace SolarApp.DataProcessor.Utility.Interfaces
{
	public interface IConfiguration
	{
		string NewFilePollPath { get; set; }
		string MongoDatabaseName { get; set; }
		string MongoConnectionString { get; set; }
		string FtpDestinationUrl { get; set; }
		string FtpPassword { get; set; }
		string FtpUsername { get; set; }
	}
}
