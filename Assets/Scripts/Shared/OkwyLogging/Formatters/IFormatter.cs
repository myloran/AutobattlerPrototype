using System;

namespace Okwy.Logging.Formatters
{
	public interface IFormatter
	{
		string FormatMessage(Logger logger, LogLevel logLevel, string message);
	}
}
