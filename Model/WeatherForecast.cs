using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SolarApp.Model
{
    [Serializable]
    public class WeatherForecast
    {

        [JsonIgnore, BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("SiteReport"), JsonProperty(PropertyName = "SiteRep")]
        public SiteRep SiteReport { get; set; }

        public bool IsValid()
        {
            return SiteReport != null;
        }

    }

    public class Param
    {
        [BsonElement("Name"), JsonProperty(PropertyName="name")]
        public string Name { get; set; }

        [BsonElement("Units"), JsonProperty(PropertyName = "units")]
        public string Units { get; set; }

        [BsonElement("Description"), JsonProperty(PropertyName="$")]
        public string Description { get; set; }
    }

    public class Wx
    {
        [BsonElement("Parameters"), JsonProperty(PropertyName = "Param")]
        public List<Param> Parameters { get; set; }
    }

    public class Rep
    {

        [BsonElement("WindDirection"), JsonProperty(PropertyName = "D")]
        public string WindDirection { get; set; }

        [BsonElement("FeelsLikeTemperatureCentigrade"), JsonProperty(PropertyName = "F")]
        public int FeelsLikeTemperatureCentigrade { get; set; }

        [BsonElement("WindGustMPH"), JsonProperty(PropertyName = "G")]
        public int WindGustMPH { get; set; }

        [BsonElement("ScreenRelativeHumidityPercentage"), JsonProperty(PropertyName = "H")]
        public int ScreenRelativeHumidityPercentage { get; set; }

        [BsonElement("PrecipitationProbability"), JsonProperty(PropertyName = "Pp")]
        public int PrecipitationProbability { get; set; }

        [BsonElement("WindSpeedMPH"), JsonProperty(PropertyName = "S")]
        public int WindSpeedMPH { get; set; }

        [BsonElement("TemperatureCentigrade"), JsonProperty(PropertyName = "T")]
        public int Temperature { get; set; }

        [BsonElement("Visibility"), JsonProperty(PropertyName = "V")]
        public string Visibility { get; set; }

        /// <summary>
        /// http://www.metoffice.gov.uk/datapoint/support/documentation/code-definitions
        /// </summary>
        [BsonElement("WeatherType"), JsonProperty(PropertyName = "W")]
        public int WeatherType { get; set; }

        [BsonElement("MaxUVIndex"), JsonProperty(PropertyName = "U")]
        public int MaxUVIndex { get; set; }

        [BsonElement("TimeMinutes"), JsonProperty(PropertyName = "$")]
        public int TimeMinutes { get; set; }

    }

    public class Period
    {
        [BsonElement("Type"), JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [BsonElement("Value"), JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        public List<Rep> Rep { get; set; }
    }

    public class Location
    {
        [BsonElement("UniqueIdentifier"), JsonProperty(PropertyName = "i")]
        public int UniqueIdentifier { get; set; }

        [BsonElement("Latitude"), JsonProperty(PropertyName = "lat")]
        public float Latitude { get; set; }

        [BsonElement("Longitude"), JsonProperty(PropertyName = "lon")]
        public float Longitude { get; set; }

        [BsonElement("Name"), JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [BsonElement("Country"), JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [BsonElement("Continent"), JsonProperty(PropertyName = "continent")]
        public string Continent { get; set; }

        [BsonElement("Elevation"), JsonProperty(PropertyName = "elevation")]
        public float Elevation { get; set; }

        public List<Period> Period { get; set; }
    }

    public class DV
    {
        [BsonElement("DataDate"), JsonProperty(PropertyName = "dataDate")]
        public DateTime DataDate { get; set; }

        [BsonElement("Type"), JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public Location Location { get; set; }
    }

    public class SiteRep
    {
        public Wx Wx { get; set; }
        public DV DV { get; set; }
    }

}
