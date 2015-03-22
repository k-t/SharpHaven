﻿using SharpFont;

namespace MonoHaven.Resources
{
	public class FontFaceFactory : IObjectFactory<Face>
	{
		private static readonly Library FontLibrary = new Library();

		public Face Create(string resName, Resource res)
		{
			var bytes =res.GetLayer<FontData>().Data;
			return FontLibrary.NewMemoryFace(bytes, 0);
		}
	}
}
