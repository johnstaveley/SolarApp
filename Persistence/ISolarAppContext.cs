using System;
using SolarApp.Model;

namespace SolarApp.Persistence
{
	public interface ISolarAppContext
	{
		MongoDB.Driver.MongoCollection<DataPoint> DataPoints { get; }
		void DeleteDataPointById(string id);
		void DeleteFailedDataById(string id);
		void DeleteSettingById(string id);
		MongoDB.Driver.MongoCollection<FailedData> FailedData { get; }
		FailedData FindFailedDataById(string id);
		DataPoint FindDataPointById(string id);
		Setting FindSettingById(string id);
		WeatherForecast FindWeatherForecastById(string id);
		DateTime? GetLatestEnergyReading();
		double? GetAverageOutputForHour(int hour);
		void InsertDataPoint(DataPoint dataPoint);
		void InsertFailedData(FailedData failedData);
		void InsertSetting(Setting setting);
		void InsertWeatherForecast(WeatherForecast weatherForecast);
		void SeedDatabase();
		MongoDB.Driver.MongoCollection<Setting> Settings { get; }
		void UpdateSetting(Setting setting);
		MongoDB.Driver.MongoCollection<WeatherForecast> WeatherForecast { get; }
	}
}
