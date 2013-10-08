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
		
		public Stream Get(string resourceName)
		{
			var entryName = string.Format("res/{0}.res", resourceName);
			var entry = _zip.GetEntry(entryName);
			if (entry == null)
				throw new ResourceLoadException(string.Format("Entry '{0}' not found", entryName));
			return _zip.GetInputStream(entry);;
		}

		public string Name
		{
			get { return string.Format("[JAR][{0}]", Path.GetFileName(_path)); }
		}

		public void Dispose()
		{
			if (_zip != null)
				_zip.Close();
		}
	}
}

