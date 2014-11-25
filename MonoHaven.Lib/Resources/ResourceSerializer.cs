using System;
using System.Collections.Generic;
using System.IO;
using C5;

namespace MonoHaven.Resources
{
	public class ResourceSerializer
	{
		private const string Signature = "Haven Resource 1";

		private readonly TreeDictionary<string, IDataLayerSerializer> serializers;

		public ResourceSerializer()
		{
			serializers = new TreeDictionary<string, IDataLayerSerializer>();
			// register default serializers
			Register(new ActionDataSerializer());
			Register(new AnimDataSerializer());
			Register(new ImageDataSerializer());
			Register(new NegDataSerializer());
			Register(new TextDataSerializer());
			Register(new TileDataSerializer());
			Register(new TilesetDataSerializer());
			Register(new TooltipSerializer());
		}

		public void Register(IDataLayerSerializer serializer)
		{
			serializers[serializer.LayerName] = serializer;
		}

		public Resource Deserialize(Stream stream)
		{
			var reader = new BinaryReader(stream);

			var sig = new String(reader.ReadChars(Signature.Length));
			if (sig != Signature)
				throw new ResourceException("Invalid signature");

			var version = reader.ReadInt16();
			var layers = new List<object>();
			while (true)
			{
				var layer = ReadLayer(reader);
				if (layer == null)
					break;
				layers.Add(layer);
			}
			return new Resource(version, layers);
		}

		private object ReadLayer(BinaryReader reader)
		{
			var type = reader.ReadCString();
			if (string.IsNullOrEmpty(type))
				return null;
			var size = reader.ReadInt32();
			var serializer = GetSerializer(type);
			if (serializer == null)
			{
				// skip layer data
				reader.ReadBytes(size);
				return new UnknownDataLayer(type);
			}
			return serializer.Deserialize(size, reader);
		}

		private IDataLayerSerializer GetSerializer(string layerName)
		{
			IDataLayerSerializer serializer;
			return serializers.Find(ref layerName, out serializer) ? serializer : null;
		}
	}
}
