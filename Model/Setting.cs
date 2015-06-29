using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SolarApp.Model
{
    public class Setting
    {
		[JsonIgnore, BsonElement("_id")]
		public string Id { get; set; }

        public string Value { get; set; }
    }
}
