using System;

namespace Okwy.Logging.Formatters
{
	public class TimestampFormatter : IFormatter
	{
		public TimestampFormatter() : this("{0:yyyy/MM/dd/hh:mm:ss:fff}")
		{
		}

		public TimestampFormatter(string timeFormat)
		{
			this._timeFormat = timeFormat;
		}

		public string FormatMessage(Logger logger, LogLevel logLevel, string message)
		{
			string str = string.Format(this._timeFormat, DateTime.Now);
			return string.Format(this._timeFormat, str + " " + message);
		}

		readonly string _timeFormat;
	}
}
