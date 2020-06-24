namespace Shared.Addons.OkwyLogging.Formatters
{
	public interface IFormatter
	{
		string FormatMessage(Logger logger, LogLevel logLevel, string message);
	}
}
