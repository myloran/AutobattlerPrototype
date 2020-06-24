using System.IO;
using Shared.Addons.OkwyLogging;
using Shared.Addons.OkwyLogging.Appenders;
using Shared.Addons.OkwyLogging.Formatters;
using UnityEngine;
using static Shared.Addons.OkwyLogging.MainLog;

namespace Infrastructure.OkwyLoggingUnity {
  public class OkwyDefaultLog {
    public static void DefaultInit() {
      globalLogLevel = LogLevel.Info;
      ResetAppenders();
      AddAppender(new FileWriterAppender(
        Path.Combine(Application.persistentDataPath, "Application.log"),
        new FullFormatter()).WriteLine);
      AddAppender(new UnityConsoleAppender(new TimeFormatter()).WriteLine);
    }
  }
}