using MonoHaven.Resources;
using MonoHaven.Resources.Serialization.Binary;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Serialization.Binary
{
	[TestFixture]
	public class TextDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new TextData
			{
				Text = "Text"
			};

			var serializer = new TextDataSerializer();
			var output = (TextData)serializer.Reserialize(input);

			Assert.That(output.Text, Is.EqualTo(input.Text));
		}
	}
}
