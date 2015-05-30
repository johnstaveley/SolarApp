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

        public void DeleteSetting(Setting setting)
        {
            this.Settings.Remove(Query.EQ("_id", BsonValue.Create(setting._id)));
        }

        #endregion

    }
}


