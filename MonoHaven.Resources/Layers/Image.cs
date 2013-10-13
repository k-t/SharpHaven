using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MonoHaven.Resources
{
	public class Image : ILayer
	{
		public short Id { get; set; }
		public short Z { get; set; }
		public short SubZ { get; set; }
		public byte[] Data { get; set; }
	}
}

