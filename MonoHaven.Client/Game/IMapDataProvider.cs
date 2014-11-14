using System;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public interface IMapDataProvider
	{
		void RequestData(int gx, int gy);
		event Action<MapData> DataAvailable;
		event Action<byte, Tileset> TilesetAvailable;
	}
}
