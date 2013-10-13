using System;

namespace MonoHaven.Resources
{
	public class UnknownLayer : ILayer
	{
		public UnknownLayer(string type)
		{
			Type = type;
		}

		public string Type { get; private set; }
	}
}

