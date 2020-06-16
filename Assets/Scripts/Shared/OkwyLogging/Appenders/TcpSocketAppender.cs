using System.Text;

namespace Shared.OkwyLogging.Appenders {
    public class TcpSocketAppender : AbstractTcpSocketAppender
	{
		protected override byte[] serializeMessage(Logger logger, LogLevel logLevel, string message)
		{
			return Encoding.UTF8.GetBytes(message);
		}
	}
}
