using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using MonoHaven.Resources.Layers;

namespace MonoHaven.Resources
{
	public class ResourceSerializer
	{
		private delegate IDataLayer LayerReader(int size, BinaryReader reader);
		private const string Signature = "Haven Resource 1";

		private readonly SortedList<string, LayerReader> dataReaders;

		public ResourceSerializer()
		{
			dataReaders = new SortedList<string, LayerReader>();
			dataReaders.Add("image", ReadImageData);
			dataReaders.Add("tile", ReadTileData);
			dataReaders.Add("tileset", ReadTilesetData);
			dataReaders.Add("neg", ReadNegData);
			dataReaders.Add("anim", ReadAnimData);
		}

		public Resource Deserialize(Stream stream)
		{
			var reader = new BinaryReader(stream);

			var sig = new String(reader.ReadChars(Signature.Length));
			if (sig != Signature)
				throw new ResourceLoadException("Invalid signature");

			short version = reader.ReadInt16();

			var layers = new List<IDataLayer>();
			while (true)
			{
				var layer = ReadLayer(reader);
				if (layer == null)
					break;
				layers.Add(layer);
			}

			return new Resource(version, layers);
		}

		private IDataLayer ReadLayer(BinaryReader reader)
		{
			var type = ReadString(reader);
			if (string.IsNullOrEmpty(type))
				return null;
			var size = reader.ReadInt32();
			var layerReader = FindDataReader(type);
			if (layerReader == null)
			{
				// skip layer data
				reader.ReadBytes(size);
				return new UnknownDataLayer(type);
			}
			return layerReader(size, reader);
		}

		private LayerReader FindDataReader(string dataType)
		{
			LayerReader reader;
			return dataReaders.TryGetValue(dataType, out reader) ? reader : null;
		}

		private static IDataLayer ReadImageData(int size, BinaryReader reader)
		{
			var img = new ImageData();
			img.Z = reader.ReadInt16();
			img.SubZ = reader.ReadInt16();
			/* Obsolete flag 1: Layered */
			reader.ReadByte();
			img.Id = reader.ReadInt16();
			img.DrawOffset = new Point(reader.ReadInt16(), reader.ReadInt16());
			img.Data = new byte[size - 11];
			reader.Read(img.Data, 0, img.Data.Length);
			return img;
		}

		private static IDataLayer ReadTileData(int size, BinaryReader reader)
		{
			var tile = new TileData();
			tile.Type = reader.ReadChar();
			tile.Id = reader.ReadByte();
			tile.Weight = reader.ReadUInt16();
			tile.ImageData = new byte[size - 4];
			reader.Read(tile.ImageData, 0, tile.ImageData.Length);
			return tile;
		}

		private static IDataLayer ReadTilesetData(int size, BinaryReader reader)
		{
			var tileset = new TilesetData();
			tileset.HasTransitions = reader.ReadBoolean();
			var flavorCount = reader.ReadUInt16();
			tileset.FlavorDensity = reader.ReadUInt16();
			tileset.FlavorObjects = new FlavorObjectData[flavorCount];
			for (int i = 0; i < flavorCount; i++)
			{
				var fob = new FlavorObjectData();
				fob.ResName = ReadString(reader);
				fob.ResVersion = reader.ReadUInt16();
				fob.Weight = reader.ReadByte();
				tileset.FlavorObjects[i] = fob;
			}
			return tileset;
		}

		private static IDataLayer ReadNegData(int size, BinaryReader reader)
		{
			var neg = new NegData();
			neg.Center = ReadPoint(reader);
			ReadPoint(reader); /* bc */
			ReadPoint(reader); /* bs */
			ReadPoint(reader); /* sz */
			var en = reader.ReadByte();
			for (int i = 0; i < en; i++)
			{
				reader.ReadByte(); /* epid */
				var cnt = reader.ReadUInt16();
				for (int j = 0; j < cnt; j++)
					ReadPoint(reader); /* ep[epid][j] */
			}
			return neg;
		}

		private static IDataLayer ReadAnimData(int size, BinaryReader reader)
		{
			var anim = new AnimData();
			anim.Id = reader.ReadInt16();
			anim.Duration = reader.ReadUInt16();
			var frameCount = reader.ReadUInt16();
			if (frameCount * 2 != size - 6)
				throw new ResourceLoadException("Invalid anim descriptor");
			anim.Frames = new short[frameCount];
			for (int i = 0; i < frameCount; i++)
				anim.Frames[i] = reader.ReadInt16();
			return anim;
		}

		private static string ReadString(BinaryReader reader)
		{
			var sb = new StringBuilder();
			while (true)
			{
				int next = reader.Read();
				if (next == -1)
					if (sb.Length != 0)
						throw new ResourceLoadException("Incomplete resource at " + sb.ToString());
				else
					return string.Empty;
				else if (next == 0)
					break;
				sb.Append(Convert.ToChar(next));
			}
			return sb.ToString();
		}

		private static Point ReadPoint(BinaryReader reader)
		{
			return new Point(reader.ReadInt16(), reader.ReadInt16());
		}
	}
}
