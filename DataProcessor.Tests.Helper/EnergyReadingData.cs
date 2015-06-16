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
			}
			get
			{
				return Timestamp.ToString();
			}
		}


    }
}
