﻿using System;
using SolarApp.Model;
using System.Collections.Generic;
using MongoDB.Driver;

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
		SunTime FindSuntimesByDate(DateTime targetDate);
		WeatherForecast FindWeatherForecastById(string id);
		WeatherObservation FindWeatherObservationById(string id);
        double? GetAverageOutputForHour(int hour);
        List<EnergyOutputDay> GetEnergyOutputByDay(DateTime startDate, DateTime endDate);
		List<EnergyOutputMonth> GetEnergyOutputByMonth(DateTime startDate, DateTime endDate);
		List<EnergyOutputYear> GetEnergyOutputByYear(DateTime startDate, DateTime endDate);
        DateTime? GetLatestEnergyReading();
        long GetNumberOfDataPoints();
        long GetNumberOfFailedData();
        long GetNumberOfWeatherForecasts();
        long GetNumberOfWeatherObservations();
		void InsertAudit(Audit audit);
		void InsertDataPoint(DataPoint dataPoint);
		void InsertFailedData(FailedData failedData);
		void InsertSetting(Setting setting);
		void InsertWeatherForecast(WeatherForecast weatherForecast);
		void InsertWeatherObservation(WeatherObservation weatherObservation);
		bool IsDatabasePresent { get; }
		bool IsDatabaseSeeded { get; }
		void SeedDatabase();
		MongoCollection<Setting> Settings { get; }
		MongoCollection<SunTime> Suntimes { get; }
        void UpdateLastRunDate();
		void UpdateSetting(Setting setting);
		MongoCollection<WeatherForecast> WeatherForecast { get; }
		MongoCollection<WeatherObservation> WeatherObservation { get; }
	}
}
