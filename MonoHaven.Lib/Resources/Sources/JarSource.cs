#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace MonoHaven.Resources
{
	public class JarSource : IEnumerableResourceSource
	{
		private const string NamePrefix = "res/";
		private const string NameSuffix = ".res";

		private readonly string _path;
		private readonly ZipFile _zip;

		public JarSource(string path)
		{
			_path = path;
			_zip = new ZipFile(path);
		}

		public string Description
		{
			get { return string.Format("[JAR]{0}", Path.GetFileName(_path)); }
		}

		public Resource Get(string resourceName)
		{
			var serializer = new ResourceSerializer();
			var entryName = GetEntryName(resourceName);
			var entry = _zip.GetEntry(entryName);
			if (entry == null)
				throw new ResourceLoadException(string.Format("Entry '{0}' not found", entryName));
			return serializer.Deserialize(_zip.GetInputStream(entry));
		}

		public IEnumerable<string> EnumerateAll()
		{
			foreach (ZipEntry entry in _zip)
				if (IsResourceName(entry.Name))
					yield return GetResourceName(entry.Name);
		}

		public void Dispose()
		{
			_zip.Close();
		}

		private static string GetEntryName(string resourceName)
		{
			return NamePrefix + resourceName + NameSuffix;
		}

		private static bool IsResourceName(string entryName)
		{
			return entryName.StartsWith(NamePrefix) &&
				entryName.EndsWith(NameSuffix);
		}

		private static string GetResourceName(string entryName)
		{
			int startIndex = NamePrefix.Length;
			int count = entryName.Length - NameSuffix.Length - startIndex;
			return entryName.Substring(startIndex, count);
		}
	}
}

