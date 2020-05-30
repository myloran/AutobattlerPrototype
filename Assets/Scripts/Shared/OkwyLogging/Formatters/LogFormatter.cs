using System;

namespace Okwy.Logging.Formatters
{
	public delegate string LogFormatter(Logger logger, LogLevel logLevel, string message);
}
