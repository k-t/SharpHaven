using System;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public interface IMapDataProvider
	{
		void RequestTiles(int gx, int gy);
		event Action<MapData> DataAvailable;
		event Action<byte, Tileset> TilesetAvailable;
	}
}
