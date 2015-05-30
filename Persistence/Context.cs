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

        public MongoCollection<DataPoint> DataPoints
		{
			get
			{
                return Database.GetCollection<DataPoint>("DataPoints");
			}
		}

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

        public List<Setting> FindSetting(string key)
        {
            return this.Settings.Find(Query.EQ("_id", BsonValue.Create(key))).ToList<Setting>();
        }
    }
}








