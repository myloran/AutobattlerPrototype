using System;
using System.Text;

namespace Shared.Addons.OkwyLogging.Formatters {
    public class TimeFormatter : IFormatter {
        StringBuilder builder = new StringBuilder();
        readonly string timeFormat = "{0:mm:ss,fff}";

        public string FormatMessage(Logger logger, LogLevel logLevel, string message) {
            builder.Length = 0;

            return builder
                .Append("[")
                .AppendFormat(timeFormat, DateTime.Now)
                .Append("] <")
                // .Append("<")
                .Append(logger.name)
                .Append("> ")
                .Append(message)
                .ToString(); 
        }
    }
}
