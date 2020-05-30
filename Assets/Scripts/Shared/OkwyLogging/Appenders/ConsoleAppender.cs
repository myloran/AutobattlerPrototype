using System;
using System.Collections.Generic;

namespace Okwy.Logging.Appenders
{
	public class ConsoleAppender
	{
		public ConsoleAppender() : this(new Dictionary<LogLevel, ConsoleColor>
		{
			{
				LogLevel.Trace,
				ConsoleColor.Cyan
			},
			{
				LogLevel.Warn,
				ConsoleColor.DarkYellow
			},
			{
				LogLevel.Error,
				ConsoleColor.Red
			},
			{
				LogLevel.Fatal,
				ConsoleColor.DarkRed
			}
		})
		{
		}

		public ConsoleAppender(Dictionary<LogLevel, ConsoleColor> consoleColors)
		{
			this._consoleColors = consoleColors;
		}

		public void WriteLine(Logger logger, LogLevel logLevel, string message)
		{
			ConsoleColor foregroundColor;
			if (this._consoleColors.TryGetValue(logLevel, out foregroundColor))
			{
				Console.ForegroundColor = foregroundColor;
				Console.WriteLine(message);
				Console.ResetColor();
				return;
			}
			Console.WriteLine(message);
		}

		public readonly Dictionary<LogLevel, ConsoleColor> _consoleColors;
	}
}
