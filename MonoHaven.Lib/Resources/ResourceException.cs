using System;

namespace SharpHaven.Resources
{
	public class ResourceException : Exception
	{
		public ResourceException(string message)
			: base(message)
		{
		}

		public ResourceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

