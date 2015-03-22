using OpenTK;

namespace MonoHaven
{
	public static class Cursors
	{
		static Cursors()
		{
			Default = App.Resources.Get<MouseCursor>("gfx/hud/curs/arw");
		}

		public static MouseCursor Default
		{
			get;
			private set;
		}
	}
}
