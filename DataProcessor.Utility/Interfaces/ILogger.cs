using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarApp.Utility.Interfaces
{
	public interface ILogger
	{
		void Debug(object message);
		void Debug(object message, Exception exception);
		void DebugFormat(string format, object arg);
		void DebugFormat(string format, params object[] args);

		void Error(object message);
		void Error(object message, Exception exception);
		void ErrorFormat(string format, object arg);
		void ErrorFormat(string format, params object[] args);

		void Fatal(object message);
		void Fatal(object message, Exception exception);

		void Info(object message);
		void Info(object message, Exception exception);

		void Warn(object message);
		void Warn(object message, Exception exception);
	}
}