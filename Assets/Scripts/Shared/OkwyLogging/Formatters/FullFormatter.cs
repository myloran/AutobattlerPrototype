using System;
using System.Text;

namespace Okwy.Logging.Formatters {
    public class FullFormatter : IFormatter {
        static readonly int _maxLogLevelLength = GetLongestLogLevelWordLength() + 2;
        StringBuilder _stringBuilder = new StringBuilder();
        readonly string _timeFormat = "{0:yyyy-MM-dd hh:mm:ss,fff}";

        static int GetLongestLogLevelWordLength() {
            int num = 0;
            foreach (string text in Enum.GetNames(typeof(LogLevel))) {
                if (text.Length > num) {
                    num = text.Length;
                }
            }
            return num;
        }

        public string FormatMessage(Logger logger, LogLevel logLevel, string message) {
            string value = ("[" + logLevel.ToString().ToUpper() + "]").PadRight(_maxLogLevelLength);
            _stringBuilder.Length = 0;
            _stringBuilder.AppendFormat(_timeFormat, DateTime.Now);
            _stringBuilder.Append(" ");
            _stringBuilder.Append(value);
            _stringBuilder.Append(" <");
            _stringBuilder.Append(logger.name);
            _stringBuilder.Append("> ");
            _stringBuilder.Append(message);
            return _stringBuilder.ToString();
        }        
    }
}
