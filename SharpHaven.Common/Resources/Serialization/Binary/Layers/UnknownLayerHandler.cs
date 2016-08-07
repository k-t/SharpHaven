using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	public class UnknownLayerHandler : GenericLayerHandler<UnknownLayer>
	{
		public UnknownLayerHandler(UnknownLayer layer) : this(layer.LayerName)
		{
		}

		public UnknownLayerHandler(string layerName) : base(layerName)
		{
		}

		protected override UnknownLayer Deserialize(ByteBuffer buffer)
		{
			return new UnknownLayer(LayerName, buffer.ReadRemaining());
		}

		protected override void Serialize(ByteBuffer writer, UnknownLayer layer)
		{
			writer.Write(layer.Bytes);
		}
	}
}
