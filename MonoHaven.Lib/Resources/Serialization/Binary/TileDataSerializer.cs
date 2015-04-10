using System;
using System.IO;

namespace SharpHaven.Resources.Serialization.Binary
{
	internal class TileDataSerializer : IBinaryDataLayerSerializer
	{
		public string LayerName
		{
			get { return "tile"; }
		}

		public Type LayerType
		{
			get { return typeof(TileData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var tile = new TileData();
			tile.Type = reader.ReadChar();
			tile.Id = reader.ReadByte();
			tile.Weight = reader.ReadUInt16();
			tile.ImageData = new byte[size - 4];
			reader.Read(tile.ImageData, 0, tile.ImageData.Length);
			return tile;
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var tile = (TileData)data;
			writer.Write(tile.Type);
			writer.Write(tile.Id);
			writer.Write(tile.Weight);
			writer.Write(tile.ImageData);
		}
	}
}