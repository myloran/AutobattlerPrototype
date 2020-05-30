using System;

namespace Okwy.Logging
{
	public delegate void LogDelegate(Logger logger, LogLevel logLevel, string message);
}
