using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using SharpHaven.Resources.Serialization.Binary;

namespace SharpHaven.Resources
{
	public class JarSource : IEnumerableResourceSource
	{
		private const string NamePrefix = "res/";
		private const string NameSuffix = ".res";

		private readonly ZipFile zip;

		public JarSource(string path)
		{
			zip = new ZipFile(path);
		}

		public string Description
		{
			get { return $"[JAR]{zip.Name}"; }
		}

		public Resource Get(string resourceName)
		{
			var serializer = new BinaryResourceSerializer();
			var entryName = GetEntryName(resourceName);
			var entry = zip.GetEntry(entryName);
			if (entry == null)
				throw new ResourceException($"Entry '{entryName}' not found");
			return serializer.Deserialize(zip.GetInputStream(entry));
		}

		public IEnumerable<string> EnumerateAll()
		{
			foreach (ZipEntry entry in zip)
				if (IsResourceName(entry.Name))
					yield return GetResourceName(entry.Name);
		}

		public void Dispose()
		{
			zip.Close();
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

