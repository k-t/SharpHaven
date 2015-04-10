using NUnit.Framework;

namespace SharpHaven.Resources.Serialization.Binary
{
	[TestFixture]
	public class CodeEntryDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new CodeEntryData
			{
				Entries = new [] {
					new CodeEntry("name1", "class.name1"),
					new CodeEntry("name2", "class.name2"),
					new CodeEntry("name3", "classname3")
				}
			};

			var serializer = new CodeEntryDataSerializer();
			var output = (CodeEntryData)serializer.Reserialize(input);

			Assert.That(output.Entries, Is.EqualTo(input.Entries));
		}
	}
}
