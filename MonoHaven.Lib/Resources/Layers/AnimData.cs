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

		public object Deserialize(int size, BinaryReader reader)
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
	}
}
