using System;
using System.IO;

namespace MonoHaven.Resources
{
	public class NinepatchData
	{
		public byte Top { get; set; }
		public byte Bottom { get; set; }
		public byte Left { get; set; }
		public byte Right { get; set; }
	}

	public class NinepatchDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "ninepatch"; }
		}

		public Type LayerType
		{
			get { return typeof(NinepatchData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			return new NinepatchData
			{
				Top = reader.ReadByte(),
				Bottom = reader.ReadByte(),
				Left = reader.ReadByte(),
				Right = reader.ReadByte()
			};
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var ninepatch = (NinepatchData)data;
			writer.Write(ninepatch.Top);
			writer.Write(ninepatch.Bottom);
			writer.Write(ninepatch.Left);
			writer.Write(ninepatch.Right);
		}
	}
}
