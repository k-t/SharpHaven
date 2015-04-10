using System.Drawing;
using NUnit.Framework;

namespace SharpHaven.Resources.Serialization.Binary
{
	[TestFixture]
	public class ImageDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new ImageData
			{
				Id = 42,
				Offset = new Point(1, 2),
				Z = -10,
				SubZ = -20,
				Data = new byte[] { 1, 2, 3, 4, 5 },
				IsLayered = true
			};

			var serializer = new ImageDataSerializer();
			var output = (ImageData)BinarySerializerHelper.Reserialize(serializer, input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Z, Is.EqualTo(input.Z));
			Assert.That(output.SubZ, Is.EqualTo(input.SubZ));
			Assert.That(output.Offset, Is.EqualTo(input.Offset));
			Assert.That(output.IsLayered, Is.EqualTo(input.IsLayered));
			Assert.That(output.Data, Is.EquivalentTo(input.Data));
		}
	}
}
