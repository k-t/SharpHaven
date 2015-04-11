using SharpFont;

namespace SharpHaven
{
	public static class FontFaces
	{
		public static readonly Face Sans;
		public static readonly Face Serif;

		static FontFaces()
		{
			Sans = App.Resources.Get<Face>("fonts/sans");
			Serif = App.Resources.Get<Face>("fonts/serif");
		}
	}
}
