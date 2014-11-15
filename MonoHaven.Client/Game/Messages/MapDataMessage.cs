using System;
using System.Drawing;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using MonoHaven.Network;

namespace MonoHaven.Game.Messages
{
	public class MapDataMessage
	{
		public Point Grid
		{
			get;
			private set;
		}

		public string MinimapName
		{
			get;
			private set;
		}

		public byte[] Tiles
		{
			get;
			private set;
		}

		public static MapDataMessage ReadFrom(MessageReader msg)
		{
			var result = new MapDataMessage();
			result.Grid = msg.ReadCoord();
			result.MinimapName = msg.ReadString();
			result.Tiles = new byte[Constants.GridWidth * Constants.GridHeight];

			var pfl = new byte[256];
			while (true)
			{
				int pidx = msg.ReadByte();
				if (pidx == 255)
					break;
				pfl[pidx] = msg.ReadByte();
			}

			var blob = Unpack(msg.Buffer, msg.Position, msg.Length - msg.Position);
			Array.Copy(blob, result.Tiles, result.Tiles.Length);

			// TODO: handle overlays

			return result;
		}

		private static byte[] Unpack(byte[] input, int offset, int length)
		{
			var buf = new byte[4096];
			var inflater = new Inflater();
			using (var output = new MemoryStream())
			{
				inflater.SetInput(input, offset, length);
				int n;
				while ((n = inflater.Inflate(buf)) != 0)
					output.Write(buf, 0, n);

				if (!inflater.IsFinished)
					throw new Exception("Got unterminated map blob");

				return output.ToArray();
			}
		}
	}
}
