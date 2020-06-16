using System;

namespace Shared.OkwyLogging
{
	public class FablAssertException : Exception
	{
		public FablAssertException(string message) : base(message)
		{
		}
	}
}
