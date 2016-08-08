using System;
using System.IO;

namespace SharpHaven.Utils
{
	public class ByteBuffer : IDisposable
	{
		private readonly Stream stream;
		private readonly long initialPosition;
		private readonly long? length;
		private BinaryReader reader;
		private BinaryWriter writer;

		public ByteBuffer() : this(new MemoryStream(), null)
		{
		}

		public ByteBuffer(ByteBuffer buffer, int length)
			: this(buffer.stream, length)
		{
		}

		public ByteBuffer(byte[] bytes)
			: this(bytes, 0, bytes.Length)
		{
		}

		public ByteBuffer(byte[] bytes, int offset, int length)
			: this(new MemoryStream(bytes, offset, length))
		{
		}

		public ByteBuffer(Stream stream)
			: this(stream, stream.Length - stream.Position)
		{
		}

		public ByteBuffer(Stream stream, long? length)
		{
			this.stream = stream;
			this.length = length;
			this.initialPosition = stream.Position;
		}

		public long Length
		{
			get { return length ?? stream.Length; }
		}

		public long Remaining
		{
			get { return (initialPosition + Length) - stream.Position; }
		}

		public bool HasRemaining
		{
			get { return Remaining > 0; }
		}

		public long Position
		{
			get { return stream.Position; }
			set { stream.Position = value; }
		}

		private BinaryReader Reader
		{
			get
			{
				if (reader == null)
				{
					if (!stream.CanRead)
						throw new InvalidOperationException("Buffer is not readable");
					reader = new BinaryReader(stream);
				}
				return reader;
			}
		}

		private BinaryWriter Writer
		{
			get
			{
				if (writer == null)
				{
					if (!stream.CanWrite)
						throw new InvalidOperationException("Buffer is not writable");
					writer = new BinaryWriter(stream);
				}
				return writer;
			}
		}

		public void Rewind()
		{
			stream.Position = initialPosition;
		}

		public byte ReadByte()
		{
			return Reader.ReadByte();
		}

		public sbyte ReadSByte()
		{
			return Reader.ReadSByte();
		}

		public byte[] ReadBytes(int count)
		{
			return Reader.ReadBytes(count);
		}

		public bool ReadBoolean()
		{
			return Reader.ReadBoolean();
		}

		public char ReadChar()
		{
			return Reader.ReadChar();
		}

		public char[] ReadChars(int count)
		{
			return Reader.ReadChars(count);
		}

		public short ReadInt16()
		{
			return Reader.ReadInt16();
		}

		public int ReadInt32()
		{
			return Reader.ReadInt32();
		}

		public long ReadInt64()
		{
			return Reader.ReadInt64();
		}

		public ushort ReadUInt16()
		{
			return Reader.ReadUInt16();
		}

		public uint ReadUInt32()
		{
			return Reader.ReadUInt32();
		}

		public ulong ReadUInt64()
		{
			return Reader.ReadUInt64();
		}

		public float ReadSingle()
		{
			return Reader.ReadSingle();
		}

		public double ReadDouble()
		{
			return Reader.ReadDouble();
		}

		public void Write(byte value)
		{
			Writer.Write(value);
		}

		public void Write(byte[] value)
		{
			Writer.Write(value);
		}

		public void Write(byte[] value, int index, int count)
		{
			Writer.Write(value, index, count);
		}

		public void Write(bool value)
		{
			Writer.Write(value);
		}

		public void Write(char value)
		{
			Writer.Write(value);
		}

		public void Write(char[] value)
		{
			Writer.Write(value);
		}

		public void Write(short value)
		{
			Writer.Write(value);
		}

		public void Write(int value)
		{
			Writer.Write(value);
		}

		public void Write(long value)
		{
			Writer.Write(value);
		}

		public void Write(ushort value)
		{
			Writer.Write(value);
		}

		public void Write(uint value)
		{
			Writer.Write(value);
		}

		public void Write(ulong value)
		{
			Writer.Write(value);
		}

		public void Write(float value)
		{
			Writer.Write(value);
		}

		public void Write(double value)
		{
			Writer.Write(value);
		}

		public void Dispose()
		{
			if (stream != null)
				stream.Dispose();
		}
	}
}
