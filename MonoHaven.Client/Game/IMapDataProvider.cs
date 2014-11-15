using System;
using MonoHaven.Game.Messages;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public interface IMapDataProvider
	{
		void RequestData(int gx, int gy);
		event Action<MapDataMessage> DataAvailable;
		event Action<byte, Tileset> TilesetAvailable;
	}
}
