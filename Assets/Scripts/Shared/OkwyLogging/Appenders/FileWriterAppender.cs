using System;
using System.IO;
using Okwy.Logging.Formatters;

namespace Okwy.Logging.Appenders {
    public class FileWriterAppender {
        public FileWriterAppender(string filePath) {
            _filePath = filePath;
        }

        public void ClearFile() {
            object @lock = _lock;
            lock (@lock) {
                using (StreamWriter streamWriter = new StreamWriter(_filePath, false)) {
                    streamWriter.Write(string.Empty);
                }
            }
        }

        public void WriteLine(Logger logger, LogLevel logLevel, string message) {
            object @lock = _lock;
            lock (@lock) {
                using (StreamWriter streamWriter = new StreamWriter(_filePath, true)) {
                    if (_formatter != null) {
                        streamWriter.WriteLine(_formatter.FormatMessage(logger, logLevel, message));
                    } else {
                        streamWriter.WriteLine(message);
                    }
                }
            }
        }

        public FileWriterAppender(string filePath, IFormatter formatter) {
            _filePath = filePath;
            _formatter = formatter;
        }

        readonly object _lock = new object();

        readonly string _filePath;

        readonly IFormatter _formatter;
    }
}