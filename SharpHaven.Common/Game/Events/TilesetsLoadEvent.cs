using System.Collections.Generic;

namespace SharpHaven.Game.Events
{
	public class TilesetsLoadEvent
	{
		public IEnumerable<TilesetBinding> Tilesets { get; set; }
	}

	public class TilesetBinding
	{
		public byte Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public ushort Version
		{
			get;
			set;
		}
	}
}
