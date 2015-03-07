using System;
using System.IO;

namespace MonoHaven.Resources.Serialization.Binary
{
	internal class NinepatchDataSerializer : IBinaryDataLayerSerializer
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