using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB;
using MongoDB.Shared;
using MongoDB.Bson;
using Model;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;

namespace Persistence
{

	public class SolarAppContext
	{

		public MongoDatabase Database;

        public SolarAppContext(string connectionString, string databaseName)
		{
            var client = new MongoClient(connectionString);
			var server = client.GetServer();
            Database = server.GetDatabase(databaseName);            
		}

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

		public double? GetAverageOutputForHour(int hour)
		{
			var scope = new BsonDocument("criteria", new BsonDocument("hour", hour));
			string map = @"
				function (){
				var key = 'CurrentReading';
				var timestamp = new Date(this.Head.Timestamp);
				if (timestamp.getHours() === criteria.hour) {
					var value = this.Body.PAC.Values['1'];
					emit(key,value);
				}
			}";
			string reduce = @"
				function (key, values){
					var reducedValue = {
					'average': Array.avg(values),
					'count': values.length
					};
				return reducedValue;
			}";
			var args = new MapReduceArgs()
			{
				MapFunction = new BsonJavaScriptWithScope(map, scope),
				ReduceFunction = new BsonJavaScript(reduce),
				OutputMode = MapReduceOutputMode.Inline
			};
			var bsonResults = this.DataPoints.MapReduce(args).GetResults();
			var bsonResult = bsonResults.ToList().FirstOrDefault();
			if (bsonResult == null) return null;
			var jsonResult = bsonResult.ToJson();
			var mapReduceOutput = new
			{
				_id = "",
				value = new { 
					average = 0.0,
					count = 0.0
				}
			};

			mapReduceOutput = JsonConvert.DeserializeAnonymousType(jsonResult, mapReduceOutput);

			if (mapReduceOutput == null) return null;
			if (mapReduceOutput.value == null) return null;
			return mapReduceOutput.value.average;
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

        #endregion

    }
}


