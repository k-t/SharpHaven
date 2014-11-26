using MonoHaven.Resources;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Layers
{
	[TestFixture]
	public class FontDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new FontData
			{
				Data = new byte[] { 1, 2, 3, 4, 5 }
			};

			var serializer = new FontDataSerializer();
			var output = (FontData)serializer.Reserialize(input);

			Assert.That(output.Data, Is.EquivalentTo(input.Data));
		}
	}
}
