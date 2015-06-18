using System;
namespace DataProcessor.Utility.Interfaces
{
	public interface IConfiguration
	{
		string NewFilePollPath { get; set; }
		string MongoDatabaseName { get; set; }
		string MongoConnectionString { get; set; }
	}
}
