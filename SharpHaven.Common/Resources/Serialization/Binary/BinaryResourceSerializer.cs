using System;
using System.Collections.Generic;
using System.IO;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary
{
	public class BinaryResourceSerializer : IResourceSerializer
	{
		private const string Signature = "Haven Resource 1";

		private readonly IBinaryLayerHandlerProvider handlerProvider;

		public BinaryResourceSerializer() : this(new BinaryLayerHandlerProvider())
		{
		}

		public BinaryResourceSerializer(IBinaryLayerHandlerProvider handlerProvider)
		{
			this.handlerProvider = handlerProvider;
		}

		public Resource Deserialize(Stream inputStream)
		{
			var buffer = new ByteBuffer(inputStream);

			var sig = new String(buffer.ReadChars(Signature.Length));
			if (sig != Signature)
				throw new ResourceException("Invalid signature");

			var version = buffer.ReadUInt16();
			var layers = new List<object>();
			while (true)
			{
				var layer = ReadLayer(buffer);
				if (layer == null)
					break;
				layers.Add(layer);
			}
			return new Resource(version, layers);
		}

		public void Serialize(Resource res, Stream outputStream)
		{
			var buffer = new ByteBuffer(outputStream);
			buffer.Write(Signature.ToCharArray());
			buffer.Write((ushort)res.Version);
			foreach (var layer in res.GetLayers())
			{
				var serializer = handlerProvider.Get(layer);
				if (serializer == null)
					throw new ResourceException("Unsupported layer type " + layer.GetType().FullName);
				buffer.WriteCString(serializer.LayerName);
				// write each layer to the temporary buffer
				// to be able to prefix layer length
				using (var ms = new MemoryStream())
				using (var layerBuffer = new ByteBuffer(ms))
				{
					serializer.Serialize(layerBuffer, layer);
					buffer.Write((int)ms.Length);
					buffer.Write(ms.ToArray());
				}
			}
		}

		private object ReadLayer(ByteBuffer buffer)
		{
			var layerName = buffer.ReadCString();
			if (string.IsNullOrEmpty(layerName))
				return null;
			var size = buffer.ReadInt32();
			var layerBuffer = new ByteBuffer(buffer, size);
			return handlerProvider.GetByName(layerName).Deserialize(layerBuffer);
		}
	}
}
