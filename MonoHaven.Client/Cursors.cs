using MonoHaven.Resources;
using OpenTK;

namespace MonoHaven
{
	public static class Cursors
	{
		static Cursors()
		{
			Default = ResourceManager.LoadCursor("gfx/hud/curs/arw");
		}

		public static MouseCursor Default
		{
			get;
			private set;
		}
	}
}
