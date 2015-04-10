using NUnit.Framework;

namespace SharpHaven.Resources.Serialization.Binary
{
	[TestFixture]
	public class TooltipDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new TooltipData
			{
				Text = "Text"
			};

			var serializer = new TooltipDataSerializer();
			var output = (TooltipData)BinarySerializerHelper.Reserialize(serializer, input);

			Assert.That(output.Text, Is.EqualTo(input.Text));
		}
	}
}
