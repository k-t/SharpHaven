using System;
using MonoHaven.Resources;

namespace MonoHaven
{
	public class ResLoader
	{
		private static IResourceLoader current;

		public static IResourceLoader Current
		{
			get { return current; }
			set { current = value; }
		}
	}
}

