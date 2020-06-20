using System.Net.Sockets;

namespace Shared.Addons.OkwyLogging.Networking
{
	public delegate void TcpSocketReceive(AbstractTcpSocket tcpSocket, Socket socket, byte[] bytes);
}
