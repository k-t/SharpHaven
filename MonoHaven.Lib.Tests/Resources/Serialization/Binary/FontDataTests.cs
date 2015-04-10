using NUnit.Framework;

namespace SharpHaven.Resources.Serialization.Binary
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
			var output = (FontData)BinarySerializerHelper.Reserialize(serializer, input);

			Assert.That(output.Data, Is.EquivalentTo(input.Data));
		}
	}
}
