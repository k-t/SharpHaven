using MonoHaven.Resources;
using OpenTK;

namespace MonoHaven
{
	public static class Cursors
	{
		static Cursors()
		{
			Default = App.Instance.Resources.GetCursor("gfx/hud/curs/arw");
		}

		public static MouseCursor Default
		{
			get;
			private set;
		}
	}
}
