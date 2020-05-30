using System;
using System.Net.Sockets;

namespace Okwy.Networking
{
	public delegate void TcpSocketReceive(AbstractTcpSocket tcpSocket, Socket socket, byte[] bytes);
}
