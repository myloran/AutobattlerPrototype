using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Okwy.Networking
{
	public class TcpServerSocket : AbstractTcpSocket
	{
		public event TcpServerSocket.TcpServerSocketHandler OnClientConnected;

		public event TcpServerSocket.TcpServerSocketHandler OnClientDisconnected;

		public int count
		{
			get
			{
				return this._clients.Count;
			}
		}

		public TcpServerSocket() : base(typeof(TcpServerSocket).Name)
		{
			this._clients = new Dictionary<string, Socket>();
		}

		public void Listen(int port)
		{
			this._logger.Info("Server is listening on port " + port + "...");
			this._socket.Bind(new IPEndPoint(IPAddress.Any, port));
			this._socket.Listen(128);
			this._socket.BeginAccept(new AsyncCallback(this.onAccepted), this._socket);
		}

		public Socket GetClientWithRemoteEndPoint(IPEndPoint endPoint)
		{
			Socket result;
			this._clients.TryGetValue(AbstractTcpSocket.keyForEndPoint(endPoint), out result);
			return result;
		}

		public override void Send(byte[] buffer)
		{
			if (this._clients.Count != 0)
			{
				foreach (Socket socket in this._clients.Values.ToArray<Socket>())
				{
					base.send(socket, buffer);
				}
				return;
			}
			this._logger.Debug("Server doesn't have any connected clients. Won't send.");
		}

		public void SendTo(byte[] buffer, IPEndPoint endPoint)
		{
			Socket clientWithRemoteEndPoint = this.GetClientWithRemoteEndPoint(endPoint);
			base.send(clientWithRemoteEndPoint, buffer);
		}

		public void DisconnectClient(IPEndPoint endPoint)
		{
			Socket clientWithRemoteEndPoint = this.GetClientWithRemoteEndPoint(endPoint);
			clientWithRemoteEndPoint.Shutdown(SocketShutdown.Both);
			clientWithRemoteEndPoint.BeginDisconnect(false, new AsyncCallback(this.onDisconnectClient), clientWithRemoteEndPoint);
		}

		public override void Disconnect()
		{
			this._socket.Close();
			this._logger.Info("Server stopped listening");
			foreach (Socket socket in this._clients.Values.ToArray<Socket>())
			{
				socket.Shutdown(SocketShutdown.Both);
				socket.BeginDisconnect(false, new AsyncCallback(this.onDisconnectClient), socket);
			}
		}

		void onAccepted(IAsyncResult ar)
		{
			Socket socket = (Socket)ar.AsyncState;
			Socket socket2 = null;
			try
			{
				socket2 = socket.EndAccept(ar);
			}
			catch (ObjectDisposedException)
			{
			}
			if (socket2 != null)
			{
				this.addClient(socket2);
				this._socket.BeginAccept(new AsyncCallback(this.onAccepted), this._socket);
			}
		}

		void addClient(Socket client)
		{
			string text = AbstractTcpSocket.keyForEndPoint((IPEndPoint)client.RemoteEndPoint);
			this._clients.Add(text, client);
			this._logger.Debug("Server accepted new client connection from " + text);
			base.receive(new ReceiveVO(client, new byte[client.ReceiveBufferSize]));
			if (this.OnClientConnected != null)
			{
				this.OnClientConnected(this, client);
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
					this.removeClient(receiveVO.socket);
					return;
				}
			}
			else
			{
				string text = AbstractTcpSocket.keyForEndPoint((IPEndPoint)receiveVO.socket.RemoteEndPoint);
				this._logger.Debug(string.Concat(new object[]
				{
					"Server received ",
					num,
					" bytes from ",
					text
				}));
				base.triggerOnReceived(receiveVO, num);
				base.receive(receiveVO);
			}
		}

		void removeClient(Socket socket)
		{
			string key = this._clients.Single((KeyValuePair<string, Socket> kv) => kv.Value == socket).Key;
			this._clients.Remove(key);
			socket.Close();
			this._logger.Debug("Client " + key + " disconnected from server");
			if (this.OnClientDisconnected != null)
			{
				this.OnClientDisconnected(this, socket);
			}
		}

		void onDisconnectClient(IAsyncResult ar)
		{
			Socket client = (Socket)ar.AsyncState;
			string key = this._clients.Single((KeyValuePair<string, Socket> kv) => kv.Value == client).Key;
			this._clients.Remove(key);
			client.EndDisconnect(ar);
			client.Close();
			this._logger.Debug("Server disconnected client " + key);
		}

		readonly Dictionary<string, Socket> _clients;

		public delegate void TcpServerSocketHandler(TcpServerSocket server, Socket client);
	}
}
