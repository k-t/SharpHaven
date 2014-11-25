using System;

namespace MonoHaven.Resources
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

