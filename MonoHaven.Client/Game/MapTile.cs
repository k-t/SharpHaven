using System.Collections.Generic;
using System.Drawing;
using C5;
using MonoHaven.Graphics;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class MapTile
	{
		private readonly Map map;
		private readonly Point coord;
		private readonly byte type;
		private readonly TextureSlice texture;
		private TextureSlice[] transitions;

		public MapTile(Map map, Point coord, byte type, TextureSlice texture)
		{
			this.map = map;
			this.coord = coord;
			this.type = type;
			this.texture = texture;
		}

		public byte Type
		{
			get { return type; }
		}

		public TextureSlice Texture
		{
			get { return texture; }
		}

		public TextureSlice[] Transitions
		{
			get
			{
				if (transitions == null)
					transitions = GenerateTransitions();
				return transitions;
			}
		}

		private TextureSlice[] GenerateTransitions()
		{
			var rng = new C5Random(RandomUtils.GetSeed());
			var tr = new int[3, 3];
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if ((x == 0) && (y == 0))
						continue;
					var t = map.GetTile(coord.X + x, coord.Y + y);
					if (t == null)
						return new TextureSlice[0];
					tr[x + 1, y + 1] = t.Type;
				}
			}
			if (tr[0, 0] >= tr[1, 0]) tr[0, 0] = -1;
			if (tr[0, 0] >= tr[0, 1]) tr[0, 0] = -1;
			if (tr[2, 0] >= tr[1, 0]) tr[2, 0] = -1;
			if (tr[2, 0] >= tr[2, 1]) tr[2, 0] = -1;
			if (tr[0, 2] >= tr[0, 1]) tr[0, 2] = -1;
			if (tr[0, 2] >= tr[1, 2]) tr[0, 2] = -1;
			if (tr[2, 2] >= tr[2, 1]) tr[2, 2] = -1;
			if (tr[2, 2] >= tr[1, 2]) tr[2, 2] = -1;
			int[] bx = { 0, 1, 2, 1 };
			int[] by = { 1, 0, 1, 2 };
			int[] cx = { 0, 2, 2, 0 };
			int[] cy = { 0, 0, 2, 2 };
			var buf = new List<TextureSlice>();
			for (int i = type - 1; i >= 0; i--)
			{
				var set = map.GetTileset((byte)i);
				if (set == null || set.BorderTransitions == null || set.CrossTransitions == null)
					continue;
				int bm = 0, cm = 0;
				for (int o = 0; o < 4; o++)
				{
					if (tr[bx[o], by[o]] == i)
						bm |= 1 << o;
					if (tr[cx[o], cy[o]] == i)
						cm |= 1 << o;
				}
				if (bm != 0)
					buf.Add(set.BorderTransitions[bm - 1].PickRandom(rng));
				if (cm != 0)
					buf.Add(set.CrossTransitions[cm - 1].PickRandom(rng));
			}
			return buf.ToArray();
		}
	}
}
