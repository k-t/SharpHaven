#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Drawing;
using System.IO;
using System.Reflection;

namespace MonoHaven.Resources
{
	public class EmbeddedResource
	{
		private const string Prefix = "MonoHaven.Content.";

		public static Stream Get(string path)
		{
			return Assembly.GetExecutingAssembly()
				.GetManifestResourceStream(Prefix + path);
		}

		public static Bitmap GetImage(string path)
		{
			using (var stream = Get(path))
				return (Bitmap)Image.FromStream(stream);
		}

		public static byte[] GetBytes(string path)
		{
			using (var stream = Get(path))
			{
				var data = new byte[stream.Length];
				stream.Read(data, 0, data.Length);
				return data;
			}
		}
	}
}
