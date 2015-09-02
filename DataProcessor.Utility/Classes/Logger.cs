using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SolarApp.Utility.Interfaces;

namespace SolarApp.Utility.Classes
{
	public class Logger : ILogger
	{
		private readonly ILog log;

		public Logger(string type)
		{
			this.log = LogManager.GetLogger(type);
		}

		public void Debug(object message)
		{
			this.log.Debug(message);
		}

		public void Debug(object message, Exception exception)
		{
			this.log.Debug(message, exception);
		}

		public void Error(object message)
		{
			this.log.Error(message);
		}

		public void Error(object message, Exception exception)
		{
			this.log.Error(message, exception);
		}

		public void Fatal(object message)
		{
			this.log.Fatal(message);
		}

		public void Fatal(object message, Exception exception)
		{
			this.log.Fatal(message, exception);
		}

		public void Info(object message)
		{
			this.log.Info(message);
		}

		public void Info(object message, Exception exception)
		{
			this.log.Info(message, exception);
		}

		public void Warn(object message)
		{
			this.log.Warn(message);
		}

		public void Warn(object message, Exception exception)
		{
			this.log.Warn(message, exception);
		}
	}
}
