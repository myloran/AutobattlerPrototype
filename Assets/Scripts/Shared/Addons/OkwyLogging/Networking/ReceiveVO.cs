﻿using System.Net.Sockets;

namespace Shared.Addons.OkwyLogging.Networking
{
	public class ReceiveVO
	{
		public ReceiveVO(Socket socket, byte[] bytes)
		{
			this.socket = socket;
			this.bytes = bytes;
		}

		public readonly Socket socket;

		public readonly byte[] bytes;
	}
}
