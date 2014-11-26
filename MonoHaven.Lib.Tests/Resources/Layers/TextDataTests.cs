using MonoHaven.Resources;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Layers
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
