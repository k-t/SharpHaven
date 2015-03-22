using SharpFont;

namespace MonoHaven
{
	public static class FontFaces
	{
		public static readonly Face Sans;
		public static readonly Face Serif;

		static FontFaces()
		{
			Sans = App.Resources.Get<Face>("custom/fonts/sans");
			Serif = App.Resources.Get<Face>("custom/fonts/serif");
		}
	}
}
