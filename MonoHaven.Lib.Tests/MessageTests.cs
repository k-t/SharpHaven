using NUnit.Framework;
using MonoHaven.Network;

namespace MonoHaven.Tests
{
	[TestFixture]
	public class MessageTests
	{
		[Test]
		public void ReadWriteTest()
		{
			var writer = new Message(1);
			writer.String("Test");
			writer.String("Test2");

			var messageBytes = writer.GetAllBytes();

			var reader = new MessageReader(1, messageBytes);
			Assert.AreEqual("Test", reader.ReadString());
			Assert.AreEqual("Test2", reader.ReadString());
		}
	}
}

