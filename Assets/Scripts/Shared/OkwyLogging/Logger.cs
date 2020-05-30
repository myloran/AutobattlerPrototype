using System;

namespace Okwy.Logging
{
	public class Logger
	{
		public event LogDelegate OnLog;

		public LogLevel logLevel { get; set; }

		public string name { get; set; }

		public Logger(string name)
		{
			this.name = name;
		}

		public void Trace(string message)
		{
			this.log(LogLevel.Trace, message);
		}

		public void Debug(string message)
		{
			this.log(LogLevel.Debug, message);
		}

		public void Info(string message)
		{
			this.log(LogLevel.Info, message);
		}

		public void Warn(string message)
		{
			this.log(LogLevel.Warn, message);
		}

		public void Error(string message)
		{
			this.log(LogLevel.Error, message);
		}

		public void Fatal(string message)
		{
			this.log(LogLevel.Fatal, message);
		}

		public void Assert(bool condition, string message)
		{
			if (!condition)
			{
				throw new FablAssertException(message);
			}
		}

		void log(LogLevel logLvl, string message)
		{
			if (this.OnLog != null && this.logLevel <= logLvl)
			{
				this.OnLog(this, logLvl, message);
			}
		}
	}
}
