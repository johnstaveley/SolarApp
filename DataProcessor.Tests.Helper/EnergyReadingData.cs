using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DataProcessor.Tests.Helper
{
    public class EnergyReadingData : EnergyReading
    {
		public string Time
		{
			set
			{
				if (value == "[Now]")
				{
					Timestamp = DateTime.Now;
				}
				else
				{
					Timestamp = DateTime.Parse(value);
				}
			}
			get
			{
				return Timestamp.ToString();
			}
		}

		private string _fileName;

		public string FileName
		{
			set
			{
				if (value == "[Random]")
				{
					_fileName = string.Format("Log{0}.log", Guid.NewGuid().ToString());
				}
				else
				{
					_fileName = value;
				}
			}
			get
			{
				return _fileName;
			}

		}


    }
}
