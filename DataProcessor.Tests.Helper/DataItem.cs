using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolarApp.Model;

namespace SolarApp.DataProcessor.Tests.Helper
{
	public class DataItem
	{
		public TableTypeKind TableTypeKind { get; set; }

		public string Id { get; set; }

		public DataItem(Setting setting)
		{
			this.Id = setting.Id;
			this.TableTypeKind = Helper.TableTypeKind.Setting;
		}

		public DataItem(DataPoint dataPoint)
		{
			this.Id = dataPoint.Id;
			this.TableTypeKind = Helper.TableTypeKind.DataPoint;
		}

		public DataItem(WeatherForecast weatherForecast)
		{
			this.Id = weatherForecast.Id;
			this.TableTypeKind = Helper.TableTypeKind.WeatherForecast;
		}

		public DataItem(WeatherObservation weatherObservation)
		{
			this.Id = weatherObservation.Id;
			this.TableTypeKind = Helper.TableTypeKind.WeatherObservation;
		}

	}
}
