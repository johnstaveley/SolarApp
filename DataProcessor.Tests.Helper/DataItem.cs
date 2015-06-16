using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DataProcessor.Tests.Helper
{
	public class DataItem
	{
		public TableTypeKind TableTypeKind { get; set; }

		public string Id { get; set; }

		public DataItem(Setting setting)
		{
			this.Id = setting._id;
			this.TableTypeKind = Helper.TableTypeKind.Setting;
		}

		public DataItem(DataPoint dataPoint)
		{
			this.Id = dataPoint._id;
			this.TableTypeKind = Helper.TableTypeKind.DataPoint;
		}
	}
}
