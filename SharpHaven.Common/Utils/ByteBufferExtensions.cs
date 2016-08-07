using System;
using System.Collections.Generic;
using System.Text;
using SharpHaven.Graphics;

namespace SharpHaven.Utils
{
	public static class ByteBufferExtensions
	{
		private static readonly Dictionary<Type, BinaryListType> BinaryListTypes =
			new Dictionary<Type, BinaryListType> {
				{typeof(byte), BinaryListType.Byte },
				{typeof(byte[]), BinaryListType.Bytes },
				{typeof(Color), BinaryListType.Color },
				{typeof(double), BinaryListType.Double },
				{typeof(short), BinaryListType.Int16 },
				{typeof(int), BinaryListType.Int32 },
				{typeof(object[]), BinaryListType.List },
				{typeof(Coord2d), BinaryListType.Coord },
				{typeof(sbyte), BinaryListType.SByte },
				{typeof(float), BinaryListType.Single },
				{typeof(string), BinaryListType.String },
				{typeof(ushort), BinaryListType.UInt16 },
		};

		public static byte[] ReadRemaining(this ByteBuffer buffer)
		{
			return buffer.ReadBytes((int)buffer.Remaining);
		}

		/// <summary>
		/// Reads a C-style (null-terminated) string from the current stream.
		/// </summary>
		public static string ReadCString(this ByteBuffer buffer)
		{
			var bytes = new List<byte>();
			while (true)
			{
				if (buffer.HasRemaining)
				{
					var next = buffer.ReadByte();
					if (next == 0)
						break;
					bytes.Add(next);
				}
				else
				{
					if (bytes.Count != 0)
						throw new Exception("Incomplete string at " + buffer);
					return string.Empty;
				}

			}
			return Encoding.UTF8.GetString(bytes.ToArray());
		}

		/// <summary>
		/// Reads a point from the current stream.
		/// </summary>
		public static Coord2d ReadInt16Coord(this ByteBuffer buffer)
		{
			return new Coord2d(buffer.ReadInt16(), buffer.ReadInt16());
		}

		public static Coord2d ReadInt32Coord(this ByteBuffer buffer)
		{
			return new Coord2d(buffer.ReadInt32(), buffer.ReadInt32());
		}

		public static Color ReadColor(this ByteBuffer buffer)
		{
			var r = buffer.ReadByte();
			var g = buffer.ReadByte();
			var b = buffer.ReadByte();
			var a = buffer.ReadByte();
			return Color.FromArgb(a, r, g, b);
		}

		public static double ReadFloat40(this ByteBuffer buffer)
		{
			return Float40.Decode(buffer.ReadBytes(5)).ToDouble();
		}

		public static object[] ReadList(this ByteBuffer buffer)
		{
			var list = new List<object>();
			while (true)
			{
				var type = buffer.HasRemaining
					? (BinaryListType)buffer.ReadByte()
					: BinaryListType.End;
				switch (type)
				{
					case BinaryListType.End:
						return list.ToArray();
					case BinaryListType.Int32:
						list.Add(buffer.ReadInt32());
						break;
					case BinaryListType.String:
						list.Add(buffer.ReadCString());
						break;
					case BinaryListType.Coord:
						list.Add(buffer.ReadInt32Coord());
						break;
					case BinaryListType.Byte:
						list.Add(buffer.ReadByte());
						break;
					case BinaryListType.UInt16:
						list.Add(buffer.ReadUInt16());
						break;
					case BinaryListType.Color:
						list.Add(buffer.ReadColor());
						break;
					case BinaryListType.List:
						list.Add(buffer.ReadList());
						break;
					case BinaryListType.SByte:
						list.Add(buffer.ReadSByte());
						break;
					case BinaryListType.Int16:
						list.Add(buffer.ReadInt16());
						break;
					case BinaryListType.Nil:
						list.Add(null);
						break;
					case BinaryListType.Bytes:
						int length = buffer.ReadByte();
						if ((length & 128) != 0)
							length = buffer.ReadInt32();
						list.Add(buffer.ReadBytes(length));
						break;
					case BinaryListType.Single:
						list.Add(buffer.ReadSingle());
						break;
					case BinaryListType.Double:
						list.Add(buffer.ReadDouble());
						break;
					case BinaryListType.Uid:
						list.Add(buffer.ReadInt64());
						break;
					default:
						throw new Exception($"Encountered unknown type {type} in TTO list.");
				}
			}
		}

		/// <summary>
		/// Writes a C-style (null-terminated) string to the current stream.
		/// </summary>
		public static void WriteCString(this ByteBuffer writer, string value)
		{
			writer.Write(value.ToCharArray());
			writer.Write('\0');
		}

		/// <summary>
		/// Writes a point to the current stream.
		/// </summary>
		public static void WriteInt16Coord(this ByteBuffer writer, int x, int y)
		{
			writer.Write((short)x);
			writer.Write((short)y);
		}

		/// <summary>
		/// Writes a point to the current stream.
		/// </summary>
		public static void WriteInt16Coord(this ByteBuffer writer, Coord2d value)
		{
			writer.WriteInt16Coord(value.X, value.Y);
		}

		public static void WriteInt32Coord(this ByteBuffer writer, Coord2d value)
		{
			writer.Write(value.X);
			writer.Write(value.Y);
		}

		public static void WriteColor(this ByteBuffer writer, Color color)
		{
			writer.Write(color.R);
			writer.Write(color.G);
			writer.Write(color.B);
			writer.Write(color.A);
		}

		public static void WriteList(this ByteBuffer writer, object[] list)
		{
			foreach (var item in list)
			{
				var type = GetBinaryListType(item);
				writer.Write((byte)type);
				switch (type)
				{
					case BinaryListType.Int32:
						writer.Write((int)item);
						break;
					case BinaryListType.String:
						writer.WriteCString((string)item);
						break;
					case BinaryListType.Coord:
						writer.WriteInt32Coord((Coord2d)item);
						break;
					case BinaryListType.Byte:
						writer.Write((byte)item);
						break;
					case BinaryListType.UInt16:
						writer.Write((ushort)item);
						break;
					case BinaryListType.Color:
						writer.WriteColor((Color)item);
						break;
					case BinaryListType.List:
						writer.WriteList((object[])item);
						break;
					case BinaryListType.SByte:
						writer.Write((sbyte)item);
						break;
					case BinaryListType.Int16:
						writer.Write((short)item);
						break;
					case BinaryListType.Bytes:
						var bytes = (byte[])item;
						if (bytes.Length < 128)
							writer.Write((byte)bytes.Length);
						else
						{
							writer.Write((byte)128);
							writer.Write(bytes.Length);
						}
						writer.Write(bytes);
						break;
					case BinaryListType.Single:
						writer.Write((float)item);
						break;
					case BinaryListType.Double:
						writer.Write((double)item);
						break;
					case BinaryListType.Uid:
						writer.Write((long)item);
						break;
				}
			}
			writer.Write((byte)BinaryListType.End);
		}

		public static void WriteFloat40(this ByteBuffer writer, double value)
		{
			writer.Write(Float40.Encode(new Float40(value)));
		}

		private static BinaryListType GetBinaryListType(object obj)
		{
			if (obj == null)
				return BinaryListType.Nil;
			BinaryListType type;
			if (BinaryListTypes.TryGetValue(obj.GetType(), out type))
				return type;
			throw new Exception($"Unsupported list item type {obj.GetType().FullName}");
		}

		private enum BinaryListType
		{
			End = 0,
			Int32 = 1,
			String = 2,
			Coord = 3,
			Byte = 4,
			UInt16 = 5,
			Color = 6,
			List = 8,
			SByte = 9,
			Int16 = 10,
			Nil = 12,
			Uid = 13,
			Bytes = 14,
			Single = 15,
			Double = 16
		}
	}
}
