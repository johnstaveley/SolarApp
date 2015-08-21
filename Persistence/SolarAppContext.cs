using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB;
using MongoDB.Shared;
using MongoDB.Bson;
using SolarApp.Model;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using SolarApp.DataProcessor.Utility.Interfaces;
using Newtonsoft.Json.Linq;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System.Text.RegularExpressions;

namespace SolarApp.Persistence
{

    public class SolarAppContext : ISolarAppContext
    {

        public MongoDatabase Database;
        private IConfiguration _configuration;

        #region Setup

        public SolarAppContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient(configuration.MongoConnectionString);
            var server = client.GetServer();
            Database = server.GetDatabase(configuration.MongoDatabaseName);
        }

        public bool IsDatabasePresent
        {
            get
            {
                try
                {
                    var ignore = Database.CollectionExists("DataPoints");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool IsDatabaseSeeded
        {
            get
            {
                var lastRunDate = this.FindSettingById("LastRunDate");
                if (lastRunDate == null) return false;
                return true;
            }
        }

        public void SeedDatabase()
        {

            if (!Database.CollectionExists("DataPoints"))
            {
                Database.CreateCollection("DataPoints");
            }
            if (!Database.GetCollection("DataPoints").IndexExistsByName("HeadTimestampIndex"))
            {
                Database.GetCollection("DataPoints").CreateIndex(IndexKeys.Ascending("Head.Timestamp"), IndexOptions.SetName("HeadTimestampIndex"));
            }
            if (!Database.CollectionExists("FailedData"))
            {
                Database.CreateCollection("FailedData");
            }
            if (!Database.CollectionExists("Settings"))
            {
                Database.CreateCollection("Settings");
            }
            var setting = this.FindSettingById("RequestWeatherForecast");
            if (setting == null)
            {
                var requestWeatherForecastSetting = new Setting();
                requestWeatherForecastSetting.Id = "RequestWeatherForecast";
                requestWeatherForecastSetting.Value = "0";
                this.InsertSetting(requestWeatherForecastSetting);
            }
            setting = this.FindSettingById("RequestWeatherObservation");
            if (setting == null)
            {
                var requestWeatherObservationSetting = new Setting();
                requestWeatherObservationSetting.Id = "RequestWeatherObservation";
                requestWeatherObservationSetting.Value = "0";
                this.InsertSetting(requestWeatherObservationSetting);
            }
            this.UpdateLastRunDate();
        }

        #endregion

        #region DataPoints

        public MongoCollection<DataPoint> DataPoints
        {
            get
            {
                return Database.GetCollection<DataPoint>("DataPoints");
            }
        }

        public void InsertDataPoint(DataPoint dataPoint)
        {
            this.DataPoints.Insert(dataPoint);
        }

        public DataPoint FindDataPointById(string id)
        {
            return this.DataPoints.Find(Query.EQ("_id", BsonValue.Create(id))).FirstOrDefault();
        }

        public DateTime? GetLatestEnergyReading()
        {
            var aggregate = new AggregateArgs
            {
                Pipeline = new[] {
					new BsonDocument("$group", new BsonDocument
						{
							{"_id", "all" },
							{"latest_reading", new BsonDocument("$max" , "$Head.Timestamp")}
						}),
					new BsonDocument("$project", new BsonDocument
						{
							{"latest_reading", new BsonDocument("$dateToString", new BsonDocument {
								new BsonDocument("format", "%Y-%m-%d %H:%M:%S:%L") ,
								new BsonDocument("date", "$latest_reading")
							})}
						})
				}
            };
            var bsonResults = this.DataPoints.Aggregate(aggregate);
            var bsonResult = bsonResults.ToList().FirstOrDefault();
            if (bsonResult == null) return null;
            var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            var jsonResult = bsonResult.ToJson(jsonWriterSettings);
            JObject output = JObject.Parse(jsonResult);
            if (output == null) return null;
            if (output["latest_reading"] == null) return null;
            var latestReadingString = output["latest_reading"].ToString();
            DateTime latestReading;
            if (DateTime.TryParseExact(latestReadingString, "yyyy-MM-dd HH:mm:ss:fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out latestReading))
            {
                return latestReading;
            }
            return null;

        }

        public long GetNumberOfDataPoints()
        {
            return this.DataPoints.Count();
        }

		public List<EnergyOutput> GetEnergyOutputByMonth(DateTime startDate, DateTime endDate)
		{
			var results = new List<EnergyOutput>();
			var aggregate = new AggregateArgs
			{
				Pipeline = new[] {
					new BsonDocument("$match", new BsonDocument {{
                        "Head.Timestamp", new BsonDocument {
                            {"$gte", startDate.ToUniversalTime() },
                            {"$lte", endDate.ToUniversalTime() }
                        }}
                    }),
                    new BsonDocument("$project", new BsonDocument
						{
							{"_id", "$Head.Timestamp"},
							{"current_energy", "$Body.PAC.Values.1"},
							{"day_energy", "$Body.DAY_ENERGY.Values.1"}
						}),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                }
			};
			//var output = GetAggregateOfDataPointsResult(aggregate);
			var bsonResults = this.DataPoints.Aggregate(aggregate);
			var bsonResult = bsonResults.ToList();
			if (bsonResult == null) return null;
			var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Shell };
			var jsonResult = bsonResult.ToJson(jsonWriterSettings);
			jsonResult = Regex.Replace(jsonResult, "ISODate\\((.{22})\\)", "$1");
			var energyReadings = JsonConvert.DeserializeObject<List<EnergyOutput>>(jsonResult);
			double lastEnergyProduction = 0;
			foreach (var energyReading in energyReadings)
			{
				energyReading.DayEnergyInstant = energyReading.DayEnergy - lastEnergyProduction;
				lastEnergyProduction = energyReading.DayEnergy;
			}
			return energyReadings;
		}

        public List<EnergyOutput> GetEnergyOutputByDay(DateTime startDate, DateTime endDate)
        {
            var results = new List<EnergyOutput>();
            var aggregate = new AggregateArgs
            {
                Pipeline = new[] {
					new BsonDocument("$match", new BsonDocument {{
                        "Head.Timestamp", new BsonDocument {
                            {"$gte", startDate.ToUniversalTime() },
                            {"$lte", endDate.ToUniversalTime() }
                        }}
                    }),
                    new BsonDocument("$project", new BsonDocument
						{
							{"_id", "$Head.Timestamp"},
							{"current_energy", "$Body.PAC.Values.1"},
							{"day_energy", "$Body.DAY_ENERGY.Values.1"}
						}),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                }
            };
            //var output = GetAggregateOfDataPointsResult(aggregate);
			var bsonResults = this.DataPoints.Aggregate(aggregate);
			var bsonResult = bsonResults.ToList();
			if (bsonResult == null) return null;
			var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Shell };
			var jsonResult = bsonResult.ToJson(jsonWriterSettings);
			jsonResult = Regex.Replace(jsonResult, "ISODate\\((.{22})\\)", "$1");
			var energyReadings = JsonConvert.DeserializeObject<List<EnergyOutput>>(jsonResult);
			double lastEnergyProduction = 0;
			foreach (var energyReading in energyReadings)
			{
				energyReading.DayEnergyInstant = energyReading.DayEnergy - lastEnergyProduction;
				lastEnergyProduction = energyReading.DayEnergy;
			}
			return energyReadings;
        }

        public double? GetAverageOutputForHour(int hour)
        {
            var aggregate = new AggregateArgs
            {
                Pipeline = new[] {
					new BsonDocument("$project", new BsonDocument
						{
							{"date", "$Head.Timestamp"},
							{"hour", new BsonDocument("$hour", "$Head.Timestamp")},
							{"current_energy", "$Body.PAC.Values.1"},
							{"day_energy", "$Body.DAY_ENERGY.Values.1"},
							{"total_energy", "$Body.TOTAL_ENERGY.Values.1"}
						}),
					new BsonDocument("$match", new BsonDocument("hour", hour)),
					new BsonDocument("$group", new BsonDocument
						{
							{"_id", "$hour"},
							{"average", new BsonDocument("$avg", "$current_energy")},
							{"maximum", new BsonDocument("$max", "$current_energy")},
							{"minimum", new BsonDocument("$min", "$current_energy")},
							{"count", new BsonDocument("$sum", 1)}
						}),
					new BsonDocument("$sort", new BsonDocument("_id", 1)),
				}
            };

            var output = GetAggregateOfDataPointsResult(aggregate);
            if (output == null) return null;
            if (output["average"] == null) return null;
            var averageString = output["average"].ToString();
            double average;
            if (double.TryParse(averageString, out average))
            {
                return average;
            }
            return null;
        }

        private JObject GetAggregateOfDataPointsResult(AggregateArgs aggregate)
        {
            var bsonResults = this.DataPoints.Aggregate(aggregate);
            var bsonResult = bsonResults.ToList().FirstOrDefault();
            if (bsonResult == null) return null;
            var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            var jsonResult = bsonResult.ToJson(jsonWriterSettings);
            JObject output = JObject.Parse(jsonResult);
            return output;
        }

        public void DeleteDataPointById(string id)
        {
            this.DataPoints.Remove(Query.EQ("_id", BsonValue.Create(id)));
        }

        #endregion

        #region Settings

        public MongoCollection<Setting> Settings
        {
            get
            {
                return Database.GetCollection<Setting>("Settings");
            }
        }

        public void InsertSetting(Setting setting)
        {
            this.Settings.Insert(setting);
        }

        public Setting FindSettingById(string id)
        {
            return this.Settings.Find(Query.EQ("_id", BsonValue.Create(id))).FirstOrDefault();
        }

        public void DeleteSettingById(string id)
        {
            this.Settings.Remove(Query.EQ("_id", BsonValue.Create(id)));
        }

        public void UpdateSetting(Setting newSetting)
        {
            var setting = this.FindSettingById(newSetting.Id);
            setting = newSetting;
            this.Settings.Save(setting);
        }

        public void UpdateLastRunDate()
        {
            var setting = this.FindSettingById("LastRunDate");
			if (setting == null)
			{
				setting = new Setting() {
					Id = "LastRunDate"
				};
				this.InsertSetting(setting);
			}
			setting.Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            this.UpdateSetting(setting);
        }

        #endregion

        #region FailedData

        public MongoCollection<FailedData> FailedData
        {
            get
            {
                return Database.GetCollection<FailedData>("FailedData");
            }
        }

        public FailedData FindFailedDataById(string id)
        {
            return this.FailedData.Find(Query.EQ("_id", BsonValue.Create(id))).FirstOrDefault();
        }

        public long GetNumberOfFailedData()
        {
            return this.FailedData.Count();
        }

        public void InsertFailedData(FailedData failedData)
        {
            this.FailedData.Insert(failedData);
        }

        public void DeleteFailedDataById(string id)
        {
            this.FailedData.Remove(Query.EQ("_id", BsonValue.Create(id)));
        }

        #endregion

        #region WeatherForecast

        public MongoCollection<WeatherForecast> WeatherForecast
        {
            get
            {
                return Database.GetCollection<WeatherForecast>("WeatherForecast");
            }
        }

        public WeatherForecast FindWeatherForecastById(string id)
        {
            return this.WeatherForecast.Find(Query.EQ("_id", BsonValue.Create(id))).FirstOrDefault();
        }

        public void InsertWeatherForecast(WeatherForecast weatherForecast)
        {
            this.WeatherForecast.Insert(weatherForecast);
        }

        public void DeleteWeatherForecastById(string id)
        {
            this.WeatherForecast.Remove(Query.EQ("_id", BsonValue.Create(id)));
        }

        public long GetNumberOfWeatherForecasts()
        {
            return this.WeatherForecast.Count();
        }

        #endregion

        #region WeatherObservation

        public MongoCollection<WeatherObservation> WeatherObservation
        {
            get
            {
                return Database.GetCollection<WeatherObservation>("WeatherObservation");
            }
        }

        public WeatherObservation FindWeatherObservationById(string id)
        {
            return this.WeatherObservation.Find(Query.EQ("_id", BsonValue.Create(id))).FirstOrDefault();
        }

        public void InsertWeatherObservation(WeatherObservation weatherObservation)
        {
            this.WeatherObservation.Insert(weatherObservation);
        }

        public void DeleteWeatherObservationById(string id)
        {
            this.WeatherObservation.Remove(Query.EQ("_id", BsonValue.Create(id)));
        }

        public long GetNumberOfWeatherObservations()
        {
            return this.WeatherObservation.Count();
        }

        #endregion

    }
}


