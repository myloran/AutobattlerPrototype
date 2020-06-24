using System;

namespace Shared.Addons.OkwyLogging
{
	public class FablAssertException : Exception
	{
		public FablAssertException(string message) : base(message)
		{
		}
	}
}
