using System;
using System.IO;

namespace MonoHaven.Resources
{
	public interface IDataLayerSerializer
	{
		string LayerName { get; }
		Type LayerType { get; }
		object Deserialize(int size, BinaryReader reader);
		void Serialize(BinaryWriter writer, object data);
	}
}

