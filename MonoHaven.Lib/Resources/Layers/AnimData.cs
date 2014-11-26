using System;
using System.IO;

namespace MonoHaven.Resources
{
	public class AnimData
	{
		public short Id { get; set; }
		public ushort Duration { get; set; }
		public short[] Frames { get; set; }
	}

	public class AnimDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "anim"; }
		}

		public Type LayerType
		{
			get { return typeof(AnimData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var anim = new AnimData();
			anim.Id = reader.ReadInt16();
			anim.Duration = reader.ReadUInt16();
			var frameCount = reader.ReadUInt16();
			if (frameCount * 2 != size - 6)
				throw new ResourceException("Invalid anim descriptor");
			anim.Frames = new short[frameCount];
			for (int i = 0; i < frameCount; i++)
				anim.Frames[i] = reader.ReadInt16();
			return anim;
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var anim = (AnimData)data;
			writer.Write(anim.Id);
			writer.Write(anim.Duration);
			writer.Write((ushort)anim.Frames.Length);
			foreach (var frame in anim.Frames)
				writer.Write(frame);
		}
	}
}
