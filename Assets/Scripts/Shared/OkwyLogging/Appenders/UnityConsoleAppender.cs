using Okwy.Logging.Formatters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Okwy.Logging.Appenders {
    public class UnityConsoleAppender {
        IFormatter formatter;

        //Dictionary<LogLevel, ConsoleColor> consoleColors = new Dictionary<LogLevel, ConsoleColor> { //add them later
        //    { LogLevel.Trace, ConsoleColor.Cyan },
        //    { LogLevel.Warn, ConsoleColor.DarkYellow },
        //    { LogLevel.Error, ConsoleColor.Red },
        //    { LogLevel.Fatal, ConsoleColor.DarkRed } };

        public UnityConsoleAppender() { }
        public UnityConsoleAppender(IFormatter formatter) { this.formatter = formatter; }
        //public UnityConsoleAppender(Dictionary<LogLevel, ConsoleColor> consoleColors) { this.consoleColors = consoleColors; }

        //public UnityConsoleAppender(IFormatter formatter, Dictionary<LogLevel, ConsoleColor> consoleColors) {
        //    this.formatter = formatter;
        //    this.consoleColors = consoleColors; }

        public void WriteLine(Logger logger, LogLevel logLevel, string message) =>
            LogMessage(formatter != null 
                ? formatter.FormatMessage(logger, logLevel, message) 
                : message, logLevel);

        void LogMessage(string message, LogLevel lvl) {
            if (lvl == LogLevel.Error)
                Debug.LogError(message);
            else if (lvl == LogLevel.Warn)
                Debug.LogWarning(message);
            else
                Debug.Log(message);
        }
    }
}