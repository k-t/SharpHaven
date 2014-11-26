using System;
using System.Collections.Generic;
using System.IO;

namespace MonoHaven.Resources
{
	public class ResourceSerializer
	{
		private const string Signature = "Haven Resource 1";

		private readonly Dictionary<string, IDataLayerSerializer> deserializers;
		private readonly Dictionary<Type, IDataLayerSerializer> serializers;

		public ResourceSerializer()
		{
			deserializers = new Dictionary<string, IDataLayerSerializer>();
			serializers = new Dictionary<Type, IDataLayerSerializer>();
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
		}

		public void Register(IDataLayerSerializer serializer)
		{
			deserializers[serializer.LayerName] = serializer;
			serializers[serializer.LayerType] = serializer;
		}

		public Resource Deserialize(Stream stream)
		{
			var reader = new BinaryReader(stream);

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

		public void Serialize(Stream stream, Resource res)
		{
			var writer = new BinaryWriter(stream);
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
					ms.WriteTo(stream);
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
			return serializer.Deserialize(size, reader);
		}

		private IDataLayerSerializer GetSerializer(string layerName)
		{
			IDataLayerSerializer serializer;
			return deserializers.TryGetValue(layerName, out serializer) ? serializer : null;
		}

		private IDataLayerSerializer GetSerializer(Type layerType)
		{
			IDataLayerSerializer serializer;
			return serializers.TryGetValue(layerType, out serializer) ? serializer : null;
		}
	}
}
