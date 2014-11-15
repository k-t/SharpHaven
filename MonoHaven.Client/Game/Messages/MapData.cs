using System;
using System.Drawing;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using MonoHaven.Network;

namespace MonoHaven.Game.Messages
{
	public class MapData
	{
		public MapData(Point grid, string minimapName, byte[] tiles)
		{
			Grid = grid;
			MinimapName = minimapName;
			Tiles = tiles;
		}

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

		public static MapData ReadFrom(MessageReader reader)
		{
			var msg = new MapData(
				grid: reader.ReadCoord(),
				minimapName: reader.ReadString(),
				tiles: new byte[Constants.GridWidth * Constants.GridHeight]
			);

			var pfl = new byte[256];
			while (true)
			{
				int pidx = reader.ReadByte();
				if (pidx == 255)
					break;
				pfl[pidx] = reader.ReadByte();
			}

			var blob = Unpack(reader.Buffer, reader.Position, reader.Length - reader.Position);
			Array.Copy(blob, msg.Tiles, msg.Tiles.Length);

			// TODO: handle overlays

			return msg;
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
