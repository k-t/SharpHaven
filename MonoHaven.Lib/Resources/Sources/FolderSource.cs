using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoHaven.Resources.Serialization.Binary;

namespace MonoHaven.Resources
{
	public class FolderSource : IEnumerableResourceSource
	{
		private const string FileExt = "res";

		private readonly string _path;

		public FolderSource(string path)
		{
			if (!Directory.Exists(path))
				throw new ArgumentException("Specified path doesn't refer to an existing folder");

			_path = Path.GetFullPath(path);

			// add directory separator to handle relative paths correctly
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				_path += Path.DirectorySeparatorChar;
		}

		public string Description
		{
			get { return string.Format("[Folder]{0}", _path); }
		}

		public IEnumerable<string> EnumerateAll()
		{
			return Directory
				.EnumerateFiles(_path, "*." + FileExt, SearchOption.AllDirectories)
				.Select(fileName => ToResourceName(fileName));
		}

		public Resource Get(string resName)
		{
			var serializer = new BinaryResourceSerializer();
			var fileName = ToFileName(resName);
			using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				return serializer.Deserialize(fileStream);
		}

		public void Dispose()
		{
		}

		private string ToFileName(string resName)
		{
			return Path.Combine(_path, resName) + "." + FileExt;
		}

		private string ToResourceName(string fileName)
		{
			var baseUri = new Uri(_path);
			var objectName = Path.GetFileNameWithoutExtension(fileName);
			var objectPath = Path.GetDirectoryName(fileName);
			var objectUri = new Uri(Path.Combine(objectPath, objectName));
			return baseUri.MakeRelativeUri(objectUri).ToString();
		}
	}
}

