using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace MonoHaven.Resources
{
	public static class BinaryStreamExtensions
	{
		/// <summary>
		/// Reads a C-style (null-terminated) string from the current stream.
		/// </summary>
		public static string ReadCString(this BinaryReader reader)
		{
			var sb = new StringBuilder();
			while (true)
			{
				int next = reader.Read();
				if (next == -1)
					if (sb.Length != 0)
						throw new ResourceException("Incomplete string at " + sb);
					else
						return string.Empty;
				if (next == 0)
					break;
				sb.Append(Convert.ToChar(next));
			}
			return sb.ToString();
		}

		/// <summary>
		/// Reads a point from the current stream.
		/// </summary>
		public static Point ReadPoint(this BinaryReader reader)
		{
			return new Point(reader.ReadInt16(), reader.ReadInt16());
		}

		/// <summary>
		/// Writes a C-style (null-terminated) string to the current stream.
		/// </summary>
		public static void WriteCString(this BinaryWriter writer, string value)
		{
			writer.Write(value.ToCharArray());
			writer.Write('\0');
		}

		/// <summary>
		/// Writes a point to the current stream.
		/// </summary>
		public static void WritePoint(this BinaryWriter writer, Point value)
		{
			writer.Write((short)value.X);
			writer.Write((short)value.Y);
		}
	}
}
