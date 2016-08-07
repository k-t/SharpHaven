using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class TilesetLayerHandler : GenericLayerHandler<TilesetLayer>
	{
		public TilesetLayerHandler() : base("tileset")
		{
		}

		protected override TilesetLayer Deserialize(ByteBuffer buffer)
		{
			var tileset = new TilesetLayer();
			tileset.HasTransitions = buffer.ReadBoolean();
			var flavorCount = buffer.ReadUInt16();
			tileset.FlavorDensity = buffer.ReadUInt16();
			tileset.FlavorObjects = new FlavorObjectData[flavorCount];
			for (int i = 0; i < flavorCount; i++)
			{
				var fob = new FlavorObjectData();
				fob.ResName = buffer.ReadCString();
				fob.ResVersion = buffer.ReadUInt16();
				fob.Weight = buffer.ReadByte();
				tileset.FlavorObjects[i] = fob;
			}
			return tileset;
		}

		protected override void Serialize(ByteBuffer writer, TilesetLayer tileset)
		{
			writer.Write(tileset.HasTransitions);
			writer.Write((ushort)tileset.FlavorObjects.Length);
			writer.Write(tileset.FlavorDensity);
			foreach (var fob in tileset.FlavorObjects)
			{
				writer.WriteCString(fob.ResName);
				writer.Write(fob.ResVersion);
				writer.Write(fob.Weight);
			}
		}
	}
}