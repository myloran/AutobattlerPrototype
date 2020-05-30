using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Okwy.Networking;

namespace Okwy.Logging.Appenders {
    public abstract class AbstractTcpSocketAppender
	{
		public AbstractTcpSocket socket { get; set; }

		public void Connect(IPAddress ip, int port)
		{
			TcpClientSocket tcpClientSocket = new TcpClientSocket();
			this.socket = tcpClientSocket;
			tcpClientSocket.OnConnected += delegate(TcpClientSocket sender)
			{
				this.onConnected();
			};
			tcpClientSocket.Connect(ip, port);
		}

		public void Listen(int port)
		{
			TcpServerSocket tcpServerSocket = new TcpServerSocket();
			this.socket = tcpServerSocket;
			tcpServerSocket.OnClientConnected += delegate(TcpServerSocket sender, Socket client)
			{
				this.onConnected();
			};
			tcpServerSocket.Listen(port);
		}

		public void Disconnect()
		{
			this.socket.Disconnect();
		}

		public void Send(Logger logger, LogLevel logLevel, string message)
		{
			if (this.isSocketReady())
			{
				this.socket.Send(this.serializeMessage(logger, logLevel, message));
				return;
			}
			this._history.Add(new AbstractTcpSocketAppender.HistoryItem(logger, logLevel, message));
		}

		bool isSocketReady()
		{
			if (this.socket != null)
			{
				TcpServerSocket tcpServerSocket = this.socket as TcpServerSocket;
				if (tcpServerSocket != null)
				{
					return tcpServerSocket.count > 0;
				}
				TcpClientSocket tcpClientSocket = this.socket as TcpClientSocket;
				if (tcpClientSocket != null)
				{
					return tcpClientSocket.isConnected;
				}
			}
			return false;
		}

		void onConnected()
		{
			if (this._history.Count > 0)
			{
				this.Send(AbstractTcpSocketAppender._logger, LogLevel.Debug, "Flush history - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
				foreach (AbstractTcpSocketAppender.HistoryItem historyItem in this._history)
				{
					this.Send(historyItem.logger, historyItem.logLevel, historyItem.message);
				}
				this.Send(AbstractTcpSocketAppender._logger, LogLevel.Debug, "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
				this._history.Clear();
			}
		}

		protected abstract byte[] serializeMessage(Logger logger, LogLevel logLevel, string message);

		static readonly Logger _logger = MainLog.GetLogger(typeof(AbstractTcpSocketAppender).Name);

		readonly List<AbstractTcpSocketAppender.HistoryItem> _history = new List<AbstractTcpSocketAppender.HistoryItem>();

		class HistoryItem
		{
			public HistoryItem(Logger logger, LogLevel logLevel, string message)
			{
				this.logger = logger;
				this.logLevel = logLevel;
				this.message = message;
			}

			public readonly Logger logger;

			public readonly LogLevel logLevel;

			public readonly string message;
		}
	}
}
