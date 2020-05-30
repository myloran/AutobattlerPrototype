using System;

namespace Okwy.Logging
{
	public class FablAssertException : Exception
	{
		public FablAssertException(string message) : base(message)
		{
		}
	}
}
