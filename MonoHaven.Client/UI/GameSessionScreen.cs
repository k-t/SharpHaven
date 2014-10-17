using System;
using System.IO;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI
{
	public class GameSessionScreen : BaseScreen
	{
		private readonly Map map = new Map();
		private readonly MapView mapView;

		public GameSessionScreen(IScreenHost host)
			: base(host)
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

			this.mapView = new MapView(map) { Width = host.Width, Height = host.Height };
			this.AddWidget(mapView);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			this.mapView.Width = Host.Width;
			this.mapView.Height = Host.Height;
		}
	}
}
