using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SolarApp.DataProcessor.Utility.Interfaces;
using System.Collections;

namespace SolarApp.DataProcessor.Utility.Classes
{
	public class Configuration : IConfiguration
	{

        public string Environment { get; set; }
        public string NewFilePollPath { get; set; }
		public string MetOfficeApiKey { get; set; }
		public string MetOfficeForecastLocationId { get; set; }
		public string MetOfficeObservationLocationId { get; set; }
		public string MetOfficeUrl { get; set; }
		public string MongoDatabaseName { get; set; }
		public string MongoConnectionString { get; set; }
		public string FtpDestinationUrl { get; set; }
		public string FtpPassword { get; set; }
		public string FtpUsername { get; set; }
        public bool DeleteFileAfterDownload { get; set; }
        public int PollIntervalSeconds {get; set; }

		public Configuration()
		{
            Environment = "Dev";
            if (System.Configuration.ConfigurationManager.AppSettings["Environment"] != null)
            {
                Environment = System.Configuration.ConfigurationManager.AppSettings["Environment"];
            }
            PollIntervalSeconds = 60;
            if (System.Configuration.ConfigurationManager.AppSettings["PollIntervalSeconds"] != null)
            {
                PollIntervalSeconds = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PollIntervalSeconds"]);
            }
			MongoDatabaseName = System.Configuration.ConfigurationManager.AppSettings["DatabaseName"];
			NewFilePollPath = System.Configuration.ConfigurationManager.AppSettings["NewFilePollPath"];
			MetOfficeUrl = System.Configuration.ConfigurationManager.AppSettings["MetOfficeUrl"];
			MongoConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            DeleteFileAfterDownload = false;
            string deleteFileAfterDownload = System.Configuration.ConfigurationManager.AppSettings["DeleteFileAfterDownload"];
            if (!string.IsNullOrEmpty(deleteFileAfterDownload)) { DeleteFileAfterDownload = bool.Parse(deleteFileAfterDownload); }
			var privateSettings = (IDictionary)ConfigurationManager.GetSection("privateSettings");
			if (privateSettings != null)
			{
				if (privateSettings["FtpUsername"] != null) {
					FtpUsername = (string) privateSettings["FtpUsername"];
				}
				if (privateSettings["FtpPassword"] != null)
				{
					FtpPassword = (string) privateSettings["FtpPassword"];
				}
				if (privateSettings["FtpUrl"] != null)
				{
					FtpDestinationUrl = (string) privateSettings["FtpUrl"];
				}
				if (privateSettings["MetOfficeApiKey"] != null)
				{
					MetOfficeApiKey = (string) privateSettings["MetOfficeApiKey"];
				}
				if (privateSettings["MetOfficeForecastLocationId"] != null)
				{
					MetOfficeForecastLocationId = (string) privateSettings["MetOfficeForecastLocationId"];
				}
				if (privateSettings["MetOfficeObservationLocationId"] != null)
				{
					MetOfficeObservationLocationId = (string)privateSettings["MetOfficeObservationLocationId"];
				}
			}
		}

	}
}
