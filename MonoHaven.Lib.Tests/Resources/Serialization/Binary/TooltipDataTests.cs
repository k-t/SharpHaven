using MonoHaven.Resources;
using MonoHaven.Resources.Serialization.Binary;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Serialization.Binary
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
			var output = (TooltipData)serializer.Reserialize(input);

			Assert.That(output.Text, Is.EqualTo(input.Text));
		}
	}
}
