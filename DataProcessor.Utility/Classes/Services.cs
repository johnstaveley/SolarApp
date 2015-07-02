using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace SolarApp.DataProcessor.Utility.Classes
{
	public class Services : SolarApp.DataProcessor.Utility.Interfaces.IServices
	{

		public string WebRequestForJson(string url)
		{
			var request = WebRequest.Create(url);
			request.ContentType = "application/json; charset=utf-8";
			var response = (HttpWebResponse)request.GetResponse();
			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				return sr.ReadToEnd();
			}

		}

	}
}
