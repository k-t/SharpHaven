using System;
using Haven.Utils;

namespace Haven.Resources.Serialization.Binary
{
	public interface IBinaryLayerHandler
	{
		string LayerName { get; }
		Type LayerType { get; }
		object Deserialize(BinaryDataReader reader);
		void Serialize(BinaryDataWriter writer, object layer);
	}
}

