using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MonoHaven.Resources
{
	public class Image : Layer
	{
		public short Id { get; private set; }
		public short Z { get; private set; }
		public short SubZ { get; private set; }
		public byte[] Data { get; private set; }

		public static Image Make(byte[] data)
		{
			var result = new Image();

			using (var stream = new MemoryStream(data))
			{
				var reader = new BinaryReader(stream);
				result.Z = reader.ReadInt16();
				result.SubZ = reader.ReadInt16();
				/* Obsolete flag 1: Layered */
				reader.ReadByte();
				result.Id = reader.ReadInt16();
				//short x = reader.ReadInt16();
				//short y = reader.ReadInt16();
			}

			result.Data = new byte[data.Length - 11];
			Array.Copy(data, 11, result.Data, 0, data.Length - 11);

			return result;
		}
	}
}

