namespace Shared.OkwyLogging.Formatters
{
	public interface IFormatter
	{
		string FormatMessage(Logger logger, LogLevel logLevel, string message);
	}
}
