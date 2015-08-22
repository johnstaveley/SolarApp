using System;
using SolarApp.Model;
using System.Collections.Generic;

namespace SolarApp.Persistence
{
	public interface ISolarAppContext
	{
		MongoDB.Driver.MongoCollection<DataPoint> DataPoints { get; }
		void DeleteDataPointById(string id);
		void DeleteFailedDataById(string id);
		void DeleteSettingById(string id);
		void DeleteWeatherForecastById(string id);
		void DeleteWeatherObservationById(string id);
		MongoDB.Driver.MongoCollection<FailedData> FailedData { get; }
		FailedData FindFailedDataById(string id);
		DataPoint FindDataPointById(string id);
		Setting FindSettingById(string id);
		WeatherForecast FindWeatherForecastById(string id);
		WeatherObservation FindWeatherObservationById(string id);
        double? GetAverageOutputForHour(int hour);
        List<EnergyOutputDay> GetEnergyOutputByDay(DateTime startDate, DateTime endDate);
		List<EnergyOutputMonth> GetEnergyOutputByMonth(DateTime startDate, DateTime endDate);
        DateTime? GetLatestEnergyReading();
        long GetNumberOfDataPoints();
        long GetNumberOfFailedData();
        long GetNumberOfWeatherForecasts();
        long GetNumberOfWeatherObservations();
		void InsertDataPoint(DataPoint dataPoint);
		void InsertFailedData(FailedData failedData);
		void InsertSetting(Setting setting);
		void InsertWeatherForecast(WeatherForecast weatherForecast);
		void InsertWeatherObservation(WeatherObservation weatherObservation);
		bool IsDatabasePresent { get; }
		bool IsDatabaseSeeded { get; }
		void SeedDatabase();
		MongoDB.Driver.MongoCollection<Setting> Settings { get; }
        void UpdateLastRunDate();
		void UpdateSetting(Setting setting);
		MongoDB.Driver.MongoCollection<WeatherForecast> WeatherForecast { get; }
		MongoDB.Driver.MongoCollection<WeatherObservation> WeatherObservation { get; }
	}
}
