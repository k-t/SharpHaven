using System.IO;

namespace MonoHaven.Resources
{
	public class TileData
	{
		public int Id { get; set; }
		public int Weight { get; set; }
		public char Type { get; set; }
		public byte[] ImageData { get; set; }
	}

	public class TileDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "tile"; }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var tile = new TileData();
			tile.Type = reader.ReadChar();
			tile.Id = reader.ReadByte();
			tile.Weight = reader.ReadUInt16();
			tile.ImageData = new byte[size - 4];
			reader.Read(tile.ImageData, 0, tile.ImageData.Length);
			return tile;
		}
	}
}
