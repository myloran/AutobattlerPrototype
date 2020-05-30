using System;
using System.Text;

namespace Okwy.Logging.Formatters
{
	public class DefaultLogMessageFormatter : IFormatter
	{
		static int getLongestLogLevelWordLength()
		{
			int num = 0;
			foreach (string text in Enum.GetNames(typeof(LogLevel)))
			{
				if (text.Length > num)
				{
					num = text.Length;
				}
			}
			return num;
		}

		public string FormatMessage(Logger logger, LogLevel logLevel, string message)
		{
			string value = ("[" + logLevel.ToString().ToUpper() + "]").PadRight(DefaultLogMessageFormatter._maxLogLevelLength);
			this._stringBuilder.Length = 0;
			this._stringBuilder.Append(value);
			this._stringBuilder.Append(" ");
			this._stringBuilder.Append(logger.name);
			this._stringBuilder.Append(": ");
			this._stringBuilder.Append(message);
			return this._stringBuilder.ToString();
		}

		static readonly int _maxLogLevelLength = DefaultLogMessageFormatter.getLongestLogLevelWordLength() + 2;

		StringBuilder _stringBuilder = new StringBuilder();
	}
}
