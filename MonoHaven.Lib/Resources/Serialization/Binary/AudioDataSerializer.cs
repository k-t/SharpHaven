using System;
using System.IO;
using MonoHaven.Resources.Layers;

namespace MonoHaven.Resources.Serialization.Binary
{
	public class AudioDataSerializer : IBinaryDataLayerSerializer
	{
		public string LayerName
		{
			get { return "audio"; }
		}

		public Type LayerType
		{
			get { return typeof(AudioData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			return new AudioData { Bytes = reader.ReadBytes(size) };
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var audio = (AudioData)data;
			writer.Write(audio.Bytes);
		}
	}
}
