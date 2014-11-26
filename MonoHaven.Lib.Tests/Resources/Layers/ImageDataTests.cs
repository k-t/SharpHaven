using System.Drawing;
using System.IO;
using MonoHaven.Resources;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Layers
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
			var output = (ImageData)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Z, Is.EqualTo(input.Z));
			Assert.That(output.SubZ, Is.EqualTo(input.SubZ));
			Assert.That(output.Offset, Is.EqualTo(input.Offset));
			Assert.That(output.IsLayered, Is.EqualTo(input.IsLayered));
			Assert.That(output.Data, Is.EquivalentTo(input.Data));
		}
	}
}
