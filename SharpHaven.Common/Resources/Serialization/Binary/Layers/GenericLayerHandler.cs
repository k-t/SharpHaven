using System;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	public abstract class GenericLayerHandler<T> : IBinaryLayerHandler
	{
		protected GenericLayerHandler(string layerName)
		{
			LayerName = layerName;
		}

		public string LayerName { get; }

		public Type LayerType
		{
			get { return typeof(T); }
		}

		protected abstract T Deserialize(ByteBuffer buffer);

		protected abstract void Serialize(ByteBuffer writer, T layer);

		#region IBinaryLayerHandler

		object IBinaryLayerHandler.Deserialize(ByteBuffer buffer)
		{
			return Deserialize(buffer);
		}

		void IBinaryLayerHandler.Serialize(ByteBuffer writer, object layer)
		{
			Serialize(writer, (T)layer);
		}

		#endregion
	}
}
