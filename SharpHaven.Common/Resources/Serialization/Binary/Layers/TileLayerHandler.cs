using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class TileLayerHandler : GenericLayerHandler<TileLayer>
	{
		public TileLayerHandler() : base("tile")
		{
		}

		protected override TileLayer Deserialize(ByteBuffer buffer)
		{
			var tile = new TileLayer();
			tile.Type = buffer.ReadChar();
			tile.Id = buffer.ReadByte();
			tile.Weight = buffer.ReadUInt16();
			tile.ImageData = buffer.ReadRemaining();
			return tile;
		}

		protected override void Serialize(ByteBuffer writer, TileLayer tile)
		{
			writer.Write(tile.Type);
			writer.Write(tile.Id);
			writer.Write(tile.Weight);
			writer.Write(tile.ImageData);
		}
	}
}