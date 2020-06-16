using System.Net;

namespace Shared.OkwyLogging.Networking
{
	public static class IPAddressStringExtension
	{
		public static IPAddress ResolveHost(this string host)
		{
			return Dns.GetHostEntry(host).AddressList[0];
		}
	}
}
