using System;
using System.IO;

namespace MonoHaven.Resources
{
	public interface IDataLayerSerializer
	{
		string LayerName { get; }
		Type LayerType { get; }
		object Deserialize(BinaryReader reader, int size);
		void Serialize(BinaryWriter writer, object data);
	}
}

