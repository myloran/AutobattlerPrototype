using System;
using System.Net;
using System.Net.Sockets;
using Okwy.Logging;

namespace Okwy.Networking {
    public abstract class AbstractTcpSocket
	{
		public event TcpSocketReceive OnReceived;

		protected AbstractTcpSocket(string loggerName)
		{
			this._logger = MainLog.GetLogger(loggerName);
			this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public abstract void Send(byte[] buffer);

		protected void send(Socket socket, byte[] buffer)
		{
			string text = AbstractTcpSocket.keyForEndPoint((IPEndPoint)socket.RemoteEndPoint);
			this._logger.Debug(string.Concat(new object[]
			{
				"Sending ",
				buffer.Length,
				" bytes via ",
				text
			}));
			socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.onSent), socket);
		}

		void onSent(IAsyncResult ar)
		{
			((Socket)ar.AsyncState).EndSend(ar);
		}

		protected void receive(ReceiveVO receiveVO)
		{
			receiveVO.socket.BeginReceive(receiveVO.bytes, 0, receiveVO.bytes.Length, SocketFlags.None, new AsyncCallback(this.onReceived), receiveVO);
		}

		protected abstract void onReceived(IAsyncResult ar);

		public abstract void Disconnect();

		protected void triggerOnReceived(ReceiveVO receiveVO, int bytesReceived)
		{
			if (this.OnReceived != null)
			{
				this.OnReceived(this, receiveVO.socket, AbstractTcpSocket.trimmedBytes(receiveVO.bytes, bytesReceived));
			}
		}

		protected static string keyForEndPoint(IPEndPoint endPoint)
		{
			return endPoint.Address + ":" + endPoint.Port;
		}

		static byte[] trimmedBytes(byte[] bytes, int length)
		{
			byte[] array = new byte[length];
			Array.Copy(bytes, array, length);
			return array;
		}

		protected readonly Logger _logger;

		protected readonly Socket _socket;
	}
}
