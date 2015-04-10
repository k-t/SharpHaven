using NUnit.Framework;

namespace SharpHaven.Resources.Serialization.Binary
{
	[TestFixture]
	public class CodeDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new CodeData
			{
				Name = "java.package.class",
				ByteCode = new byte[] { 1, 2, 3, 4, 5 }
			};

			var serializer = new CodeDataSerializer();
			var output = (CodeData)serializer.Reserialize(input);

			Assert.That(output.Name, Is.EqualTo(input.Name));
			Assert.That(output.ByteCode, Is.EqualTo(input.ByteCode));
		}
	}
}
