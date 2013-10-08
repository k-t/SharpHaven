using System;
using System.IO;

namespace MonoHaven.Resources
{
	public interface ILayerFactory
	{
		Layer Make(string typeName, byte[] data);
	}
}

