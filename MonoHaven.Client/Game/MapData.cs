using System;
using System.Drawing;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using MonoHaven.Network;

namespace MonoHaven.Game
{
	public class MapData
	{
		private Point grid;
		private string minimapName;
		private byte[] tiles;

		private MapData()
		{
		}

		public Point Grid
		{
			get { return grid; }
		}

		public string MinimapName
		{
			get { return minimapName; }
		}

		public byte[] Tiles
		{
			get { return tiles; }
		}

		public static MapData FromMessage(MessageReader msg)
		{
			var result = new MapData();
			result.grid = msg.ReadCoord();
			result.minimapName = msg.ReadString();
			result.tiles = new byte[Constants.GridWidth * Constants.GridHeight];

			var pfl = new byte[256];
			while (true)
			{
				int pidx = msg.ReadByte();
				if (pidx == 255)
					break;
				pfl[pidx] = msg.ReadByte();
			}

			var blob = Unpack(msg.Buffer, msg.Position, msg.Length - msg.Position);
			Array.Copy(blob, result.tiles, result.tiles.Length);

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
