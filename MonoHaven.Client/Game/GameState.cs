using System;
using System.IO;

namespace MonoHaven.Game
{
	public class GameState
	{
		private readonly Map map = new Map();
		
		public GameState()
		{
			foreach (var file in Directory.EnumerateFiles(@"d:\Projects\RunHH\etc\map\", "*.map"))
			{
				var parts = Path.GetFileNameWithoutExtension(file).Split('_');
				var x = int.Parse(parts[0]);
				var y = int.Parse(parts[1]);
				var bytes = File.ReadAllBytes(file);
				var tiles = new byte[Constants.GridWidth * Constants.GridHeight];
				Array.Copy(bytes, tiles, tiles.Length);
				map.AddGrid(x, y, tiles);
			}
		}

		public Map Map
		{
			get { return map; }
		}
	}
}
