using System;

namespace SolarApp.DataProcessor.Utility.Interfaces
{
	public interface IConfiguration
	{
		string NewFilePollPath { get; set; }
		string MetOfficeApiKey { get; set; }
		string MetOfficeForecastLocationId { get; set; }
		string MetOfficeObservationLocationId { get; set; }
		string MetOfficeUrl { get; set; }
		string MongoDatabaseName { get; set; }
		string MongoConnectionString { get; set; }
		string FtpDestinationUrl { get; set; }
		string FtpPassword { get; set; }
		string FtpUsername { get; set; }
        bool DeleteFileAfterDownload { get; set; }
        int PollIntervalSeconds { get; set; }
	}
}
