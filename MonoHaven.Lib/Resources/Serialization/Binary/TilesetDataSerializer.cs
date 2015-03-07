using System;
using System.IO;

namespace MonoHaven.Resources.Serialization.Binary
{
	internal class TilesetDataSerializer : IBinaryDataLayerSerializer
	{
		public string LayerName
		{
			get { return "tileset"; }
		}

		public Type LayerType
		{
			get { return typeof(TilesetData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var tileset = new TilesetData();
			tileset.HasTransitions = reader.ReadBoolean();
			var flavorCount = reader.ReadUInt16();
			tileset.FlavorDensity = reader.ReadUInt16();
			tileset.FlavorObjects = new FlavorObjectData[flavorCount];
			for (int i = 0; i < flavorCount; i++)
			{
				var fob = new FlavorObjectData();
				fob.ResName = reader.ReadCString();
				fob.ResVersion = reader.ReadUInt16();
				fob.Weight = reader.ReadByte();
				tileset.FlavorObjects[i] = fob;
			}
			return tileset;
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var tileset = (TilesetData)data;
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