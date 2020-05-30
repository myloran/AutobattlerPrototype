using System;
using System.Collections.Generic;
using System.Text;

namespace Okwy.Logging.Formatters
{
	public class ColorCodeFormatter : IFormatter
	{
		public string FormatMessage(Logger logger, LogLevel logLevel, string message)
		{
			this._stringBuilder.Length = 0;
			this._stringBuilder.Append("\u001b[");
			this._stringBuilder.Append(ColorCodeFormatter.colors[logLevel][1]);
			this._stringBuilder.Append("\u001b[");
			this._stringBuilder.Append(ColorCodeFormatter.colors[logLevel][0]);
			this._stringBuilder.Append(message);
			this._stringBuilder.Append("\u001b[");
			this._stringBuilder.Append("0m");
			return this._stringBuilder.ToString();
		}

		const string Reset = "0m";

		const string ESC = "\u001b[";

		const string FG_Black = "30m";

		const string FG_Red = "31m";

		const string FG_Green = "32m";

		const string FG_Yellow = "33m";

		const string FG_Blue = "34m";

		const string FG_Magenta = "35m";

		const string FG_Cyan = "36m";

		const string FG_White = "37m";

		const string BG_None = "";

		const string BG_Black = "40m";

		const string BG_Red = "41m";

		const string BG_Green = "42m";

		const string BG_Yellow = "43m";

		const string BG_Blue = "44m";

		const string BG_Magenta = "45m";

		const string BG_Cyan = "46m";

		const string BG_White = "47m";

		StringBuilder _stringBuilder = new StringBuilder();

		public static readonly Dictionary<LogLevel, string[]> colors = new Dictionary<LogLevel, string[]>
		{
			{
				LogLevel.Trace,
				new string[]
				{
					"37m",
					"46m"
				}
			},
			{
				LogLevel.Debug,
				new string[]
				{
					"34m",
					""
				}
			},
			{
				LogLevel.Info,
				new string[]
				{
					"32m",
					""
				}
			},
			{
				LogLevel.Warn,
				new string[]
				{
					"33m",
					""
				}
			},
			{
				LogLevel.Error,
				new string[]
				{
					"37m",
					"41m"
				}
			},
			{
				LogLevel.Fatal,
				new string[]
				{
					"37m",
					"45m"
				}
			}
		};
	}
}
