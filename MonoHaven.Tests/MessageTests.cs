using NUnit.Framework;
using System;
using MonoHaven.Network;

namespace MonoHaven.Tests
{
	[TestFixture()]
	public class MessageTests
	{
		[Test()]
		public void ReadWriteTest()
		{
			var writer = new Network.MessageWriter(Message.RMSG_BUFF);
			writer.AddString("Test");
			writer.AddString("Test2");

			var message = writer.GetMessage();

			var reader = new MessageReader(message);
			Assert.AreEqual("Test", reader.ReadString());
			Assert.AreEqual("Test2", reader.ReadString());
		}
	}
}

