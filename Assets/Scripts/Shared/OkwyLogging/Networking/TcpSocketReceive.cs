using System.Net.Sockets;

namespace Shared.OkwyLogging.Networking
{
	public delegate void TcpSocketReceive(AbstractTcpSocket tcpSocket, Socket socket, byte[] bytes);
}
