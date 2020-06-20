using System;
using System.Collections.Generic;
using System.IO;
using Shared.Addons.OkwyLogging.Appenders;
using Shared.Addons.OkwyLogging.Formatters;
using UnityEngine;

namespace Shared.Addons.OkwyLogging {
  public static class MainLog {
    static LogLevel _globalLogLevel;
    static LogDelegate _appenders;
    static readonly Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>();
    static readonly Logger _logger = GetLogger("fabl");

    public static LogLevel globalLogLevel {
      get {
        return _globalLogLevel;
      }
      set {
        _globalLogLevel = value;
        foreach (Logger logger in _loggers.Values) {
          logger.logLevel = value;
        }
      }
    }

    public static void Trace(string message) {
      _logger.Trace(message);
    }

    public static void Debug(string message) {
      _logger.Debug(message);
    }

    public static void Info(string message) {
      _logger.Info(message);
    }

    public static void Warn(string message) {
      _logger.Warn(message);
    }

    public static void Error(string message) {
      _logger.Error(message);
    }

    public static void Fatal(string message) {
      _logger.Fatal(message);
    }

    public static void Assert(bool condition, string message) {
      _logger.Assert(condition, message);
    }

    public static void AddAppender(LogDelegate appender) {
      _appenders = (LogDelegate)Delegate.Combine(_appenders, appender);
      foreach (Logger logger in _loggers.Values) {
        logger.OnLog += appender;
      }
      appenders.Add(appender);
    }

    public static void RemoveAppender(LogDelegate appender) {
      _appenders = (LogDelegate)Delegate.Remove(_appenders, appender);
      foreach (Logger logger in _loggers.Values) {
        logger.OnLog -= appender;
      }
      appenders.Remove(appender);
    }

    public static Logger GetLogger(string name) {
      Logger logger;
      if (!_loggers.TryGetValue(name, out logger)) {
        logger = new Logger(name);
        logger.logLevel = MainLog.globalLogLevel;
        logger.OnLog += MainLog._appenders;
        _loggers.Add(name, logger);
      }
      return logger;
    }

    public static void ResetLoggers() {
      _loggers.Clear();
    }

    public static void ResetAppenders() {
      for (var i = appenders.Count - 1; i >= 0; i--) {
        var appender = appenders[i];
        RemoveAppender(appender);
      }
      _appenders = null;
    }

    public static void DefaultInit() {
      globalLogLevel = LogLevel.Info;
      ResetAppenders();
      AddAppender(new FileWriterAppender(
        Path.Combine(Application.persistentDataPath, "Application.log"),
        new FullFormatter()).WriteLine);
      AddAppender(new UnityConsoleAppender(new TimeFormatter()).WriteLine);
    }

    static readonly List<LogDelegate> appenders = new List<LogDelegate>();
  }
}