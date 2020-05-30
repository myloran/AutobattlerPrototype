using System.Text;

namespace Okwy.Logging.Appenders {
    public class SosMaxAppender : AbstractTcpSocketAppender
	{
		protected override byte[] serializeMessage(Logger logger, LogLevel logLevel, string message)
		{
			return Encoding.UTF8.GetBytes(this.formatLogMessage(logLevel.ToString(), message));
		}

		string formatLogMessage(string logLevel, string message)
		{
			string[] array = message.Split(new char[]
			{
				'\n'
			});
			string arg = (array.Length == 1) ? "showMessage" : "showFoldMessage";
			bool flag = array.Length > 1;
			return string.Format("!SOS<{0} key=\"{1}\">{2}</{0}>\0", arg, logLevel, flag ? this.multilineMessage(array[0], message) : this.replaceXmlSymbols(message));
		}

		string multilineMessage(string title, string message)
		{
			return string.Concat(new string[]
			{
				"<title>",
				this.replaceXmlSymbols(title),
				"</title><message>",
				this.replaceXmlSymbols(message.Substring(message.IndexOf('\n') + 1)),
				"</message>"
			});
		}

		string replaceXmlSymbols(string str)
		{
			return str.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&lt;", "<![CDATA[<]]>").Replace("&gt;", "<![CDATA[>]]>").Replace("&", "<![CDATA[&]]>");
		}
	}
}
