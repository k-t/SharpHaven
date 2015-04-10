using NUnit.Framework;

namespace SharpHaven.Resources.Serialization.Binary
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
			var output = (TextData)BinarySerializerHelper.Reserialize(serializer, input);

			Assert.That(output.Text, Is.EqualTo(input.Text));
		}
	}
}
