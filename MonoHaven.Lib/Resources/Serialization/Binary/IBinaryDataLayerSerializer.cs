using System;
using System.IO;

namespace MonoHaven.Resources.Serialization.Binary
{
	public interface IBinaryDataLayerSerializer
	{
		string LayerName { get; }
		Type LayerType { get; }
		object Deserialize(BinaryReader reader, int size);
		void Serialize(BinaryWriter writer, object data);
	}
}

