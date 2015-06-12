using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SharpHaven.Resources
{
	[TestFixture]
	public class FolderSourceTests
	{
		private const string SamplesPath = "Resources/Samples";

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowsExceptionWhenFolderNotExists(
			[Values("./NotExistingFolder", SamplesPath + "/logo.res")]
			string path)
		{
			new FolderSource(path);
		}

		[Test]
		public void GetWorks(
			[ValueSource("GetTestItems")]
			TestItem testItem)
		{
			var source = new FolderSource(SamplesPath);
			var resource = source.Get(testItem.ResourceName);

			Assert.IsNotNull(resource);
			Assert.AreEqual(testItem.ExpectedVersion, resource.Version);
		}

		[Test]
		public void EnumerateAllWorks()
		{
			var source = new FolderSource(SamplesPath);

			var result = source.EnumerateAll().ToList();

			Assert.AreEqual(3, result.Count);
			Assert.IsTrue(result.Contains("pagina"));
			Assert.IsTrue(result.Contains("tooltip"));
			Assert.IsTrue(result.Contains("SubFolder/font"));
		}

		private IEnumerable<TestItem> GetTestItems
		{
			get
			{
				return new[] {
					new TestItem { ResourceName = "pagina", ExpectedVersion = 1 },
					new TestItem { ResourceName = "SubFolder/font", ExpectedVersion = 2 }
				};
			}
		}

		public class TestItem
		{
			public string ResourceName { get; set; }
			public int ExpectedVersion { get; set; }
		}
	}
}
