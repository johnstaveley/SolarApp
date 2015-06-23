using System;

namespace SolarApp.Persistence
{
	public interface ISolarAppContext
	{
		MongoDB.Driver.MongoCollection<Model.DataPoint> DataPoints { get; }
		void DeleteDataPointById(string id);
		void DeleteSettingById(string id);
		Model.DataPoint FindDataPointById(string id);
		Model.Setting FindSettingById(string id);
		DateTime? GetLatestEnergyReading();
		double? GetAverageOutputForHour(int hour);
		void InsertDataPoint(Model.DataPoint dataPoint);
		void InsertSetting(Model.Setting setting);
		MongoDB.Driver.MongoCollection<Model.Setting> Settings { get; }
	}
}
