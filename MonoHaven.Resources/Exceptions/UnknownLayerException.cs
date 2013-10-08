using System;

namespace MonoHaven.Resources
{
	public class UnknownLayerException : Exception
	{
		public UnknownLayerException(string type)
			: base(string.Format("Unknown layer type: {0}", type))
		{
		}
	}
}

