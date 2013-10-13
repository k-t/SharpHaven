using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace MonoHaven.Resources
{
	public class ZipSource : IResourceSource
	{
		private readonly string _path;
		private readonly ZipFile _zip;

		public ZipSource(string path)
		{
			_path = path;
			_zip = new ZipFile(path);
		}

		public string Name
		{
			get { return string.Format("[JAR][{0}]", Path.GetFileName(_path)); }
		}

		public Resource Get(string resourceName)
		{
			var serializer = new ResourceSerializer();

			var entryName = string.Format("res/{0}.res", resourceName);
			var entry = _zip.GetEntry(entryName);
			if (entry == null)
				throw new ResourceLoadException(string.Format("Entry '{0}' not found", entryName));

			return serializer.Deserialize(_zip.GetInputStream(entry));
		}

		public void Dispose()
		{
			if (_zip != null)
				_zip.Close();
		}
	}
}

