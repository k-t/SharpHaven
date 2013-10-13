using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoHaven.Resources
{
	public class ResourceSerializer
	{
		private delegate ILayer LayerReader(int size, BinaryReader reader);
		private const string Signature = "Haven Resource 1";

		private readonly SortedList<string, LayerReader> _layerReaders;

		public ResourceSerializer()
		{
			_layerReaders = new SortedList<string, LayerReader>();
			_layerReaders.Add("image", ReadImageLayer);
		}

		public Resource Deserialize(Stream stream)
		{
			var reader = new BinaryReader(stream);

			var sig = new String(reader.ReadChars(Signature.Length));
			if (sig != Signature)
				throw new ResourceLoadException("Invalid signature");

			short version = reader.ReadInt16();

			var layers = new List<ILayer>();
			while (true)
			{
				var layer = ReadLayer(reader);
				if (layer == null)
					break;
				layers.Add(layer);
			}

			return new Resource(version, layers);
		}

		private ILayer ReadLayer(BinaryReader reader)
		{
			var type = ReadString(reader);
			if (string.IsNullOrEmpty(type))
				return null;
			var size = reader.ReadInt32();
			var layerReader = FindLayerReader(type);
			if (layerReader == null)
			{
				// skip layer data
				reader.ReadBytes(size);
				return new UnknownLayer(type);
			}
			return layerReader(size, reader);
		}

		private LayerReader FindLayerReader(string type)
		{
			LayerReader reader;
			return _layerReaders.TryGetValue(type, out reader) ? reader : null;
		}

		private static ILayer ReadImageLayer(int size, BinaryReader reader)
		{
			var layer = new Image();
			layer.Z = reader.ReadInt16();
			layer.SubZ = reader.ReadInt16();
			/* Obsolete flag 1: Layered */
			reader.ReadByte();
			layer.Id = reader.ReadInt16();
			short x = reader.ReadInt16();
			short y = reader.ReadInt16();
			layer.Data = new byte[size - 11];
			reader.Read(layer.Data, 0, size - 11);
			return layer;
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
	}
}
