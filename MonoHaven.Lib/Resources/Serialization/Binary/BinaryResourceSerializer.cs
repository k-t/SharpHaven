using System;
using System.Collections.Generic;
using System.IO;

namespace MonoHaven.Resources.Serialization.Binary
{
	public class BinaryResourceSerializer : IResourceSerializer
	{
		private const string Signature = "Haven Resource 1";

		private readonly Dictionary<string, IBinaryDataLayerSerializer> deserializers;
		private readonly Dictionary<Type, IBinaryDataLayerSerializer> serializers;

		public BinaryResourceSerializer()
		{
			deserializers = new Dictionary<string, IBinaryDataLayerSerializer>();
			serializers = new Dictionary<Type, IBinaryDataLayerSerializer>();
			// register default serializers
			Register(new ActionDataSerializer());
			Register(new AnimDataSerializer());
			Register(new ImageDataSerializer());
			Register(new NegDataSerializer());
			Register(new TextDataSerializer());
			Register(new TileDataSerializer());
			Register(new TilesetDataSerializer());
			Register(new TooltipDataSerializer());
			Register(new FontDataSerializer());
			Register(new NinepatchDataSerializer());
			Register(new AudioDataSerializer());
			Register(new CodeDataSerializer());
			Register(new CodeEntryDataSerializer());
		}

		public void Register(IBinaryDataLayerSerializer serializer)
		{
			deserializers[serializer.LayerName] = serializer;
			serializers[serializer.LayerType] = serializer;
		}

		public Resource Deserialize(Stream inputStream)
		{
			var reader = new BinaryReader(inputStream);

			var sig = new String(reader.ReadChars(Signature.Length));
			if (sig != Signature)
				throw new ResourceException("Invalid signature");

			var version = reader.ReadUInt16();
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

		public void Serialize(Resource res, Stream outputStream)
		{
			var writer = new BinaryWriter(outputStream);
			writer.Write(Signature.ToCharArray());
			writer.Write((ushort)res.Version);
			foreach (var layer in res.GetLayers())
			{
				var serializer = GetSerializer(layer.GetType());
				if (serializer == null)
					throw new ResourceException("Unsupported layer type " + layer.GetType().FullName);
				writer.WriteCString(serializer.LayerName);
				// write each layer to the temporary buffer
				// to be able to prefix layer length
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms))
				{
					serializer.Serialize(bw, layer);
					writer.Write((int)ms.Length);
					ms.WriteTo(outputStream);
				}
			}
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
			return serializer.Deserialize(reader, size);
		}

		private IBinaryDataLayerSerializer GetSerializer(string layerName)
		{
			IBinaryDataLayerSerializer serializer;
			return deserializers.TryGetValue(layerName, out serializer) ? serializer : null;
		}

		private IBinaryDataLayerSerializer GetSerializer(Type layerType)
		{
			IBinaryDataLayerSerializer serializer;
			return serializers.TryGetValue(layerType, out serializer) ? serializer : null;
		}
	}
}
