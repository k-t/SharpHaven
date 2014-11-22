#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using MonoHaven.Resources;

namespace MonoHaven.Tests
{
	[TestFixture()]
	public class FolderSourceTests
	{
		private const string SamplesPath = "Samples/FolderSourceTests";

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
			GetTestItem testItem)
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
			Assert.IsTrue(result.Contains("logo"));
			Assert.IsTrue(result.Contains("logo2"));
			Assert.IsTrue(result.Contains("SubFolder/cabin-door"));
		}

		private IEnumerable<GetTestItem> GetTestItems
		{
			get
			{
				return new[] {
					new GetTestItem { ResourceName = "logo2", ExpectedVersion = 1 },
					new GetTestItem { ResourceName = "SubFolder/cabin-door", ExpectedVersion = 2 }
				};
			}
		}

		public class GetTestItem
		{
			public string ResourceName { get; set; }
			public int ExpectedVersion { get; set; }
		}
	}
}

