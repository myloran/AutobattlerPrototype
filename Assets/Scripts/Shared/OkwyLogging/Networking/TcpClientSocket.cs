using System;
using System.Net;
using System.Net.Sockets;

namespace Okwy.Networking
{
	public class TcpClientSocket : AbstractTcpSocket
	{
		public event TcpClientSocket.TcpClientSocketHandler OnConnected;

		public event TcpClientSocket.TcpClientSocketHandler OnDisconnected;

		public bool isConnected
		{
			get
			{
				return this._socket.Connected;
			}
		}

		public TcpClientSocket() : base(typeof(TcpClientSocket).Name)
		{
		}

		public void Connect(IPAddress ipAddress, int port)
		{
			this._logger.Debug(string.Concat(new object[]
			{
				"Client is connecting to ",
				ipAddress,
				":",
				port,
				"..."
			}));
			this._socket.BeginConnect(ipAddress, port, new AsyncCallback(this.onConnected), this._socket);
		}

		public override void Send(byte[] buffer)
		{
			base.send(this._socket, buffer);
		}

		public override void Disconnect()
		{
			this._logger.Debug("Client is disconnecting...");
			this._socket.Shutdown(SocketShutdown.Both);
			this._socket.BeginDisconnect(false, new AsyncCallback(this.onDisconnected), this._socket);
		}

		void onConnected(IAsyncResult ar)
		{
			Socket socket = (Socket)ar.AsyncState;
			bool flag = false;
			try
			{
				socket.EndConnect(ar);
				flag = true;
			}
			catch (SocketException ex)
			{
				this._logger.Error(ex.Message);
			}
			if (flag)
			{
				IPEndPoint endPoint = (IPEndPoint)socket.RemoteEndPoint;
				this._logger.Debug("Client connected to " + AbstractTcpSocket.keyForEndPoint(endPoint));
				base.receive(new ReceiveVO(socket, new byte[socket.ReceiveBufferSize]));
				if (this.OnConnected != null)
				{
					this.OnConnected(this);
				}
			}
		}

		protected override void onReceived(IAsyncResult ar)
		{
			ReceiveVO receiveVO = (ReceiveVO)ar.AsyncState;
			int num = 0;
			try
			{
				num = receiveVO.socket.EndReceive(ar);
			}
			catch (SocketException)
			{
			}
			if (num == 0)
			{
				if (receiveVO.socket.Connected)
				{
					this.disconnectedByRemote(receiveVO.socket);
					return;
				}
			}
			else
			{
				string text = AbstractTcpSocket.keyForEndPoint((IPEndPoint)receiveVO.socket.RemoteEndPoint);
				this._logger.Debug(string.Concat(new object[]
				{
					"Client received ",
					num,
					" bytes from ",
					text
				}));
				base.triggerOnReceived(receiveVO, num);
				base.receive(receiveVO);
			}
		}

		void disconnectedByRemote(Socket client)
		{
			client.Close();
			this._logger.Info("Client got disconnected by remote");
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(this);
			}
		}

		void onDisconnected(IAsyncResult ar)
		{
			Socket socket = (Socket)ar.AsyncState;
			socket.EndDisconnect(ar);
			socket.Close();
			this._logger.Debug("Client disconnected");
			if (this.OnDisconnected != null)
			{
				this.OnDisconnected(this);
			}
		}

		public delegate void TcpClientSocketHandler(TcpClientSocket client);
	}
}
