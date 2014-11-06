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
			writer.AddString("Test");
			writer.AddString("Test2");

			var messageBytes = writer.GetBytes();

			var reader = new MessageReader(1, messageBytes);
			Assert.AreEqual("Test", reader.ReadString());
			Assert.AreEqual("Test2", reader.ReadString());
		}
	}
}

