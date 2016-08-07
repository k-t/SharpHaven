using NUnit.Framework;
using SharpHaven.Net;
using SharpHaven.Utils;

namespace SharpHaven
{
	[TestFixture]
	public class MessageTests
	{
		[Test]
		public void ReadWriteTest()
		{
			var input = new Message(1);
			input.String("Test");
			input.String("Test2");

			input.Buffer.Rewind();
			var output = new Message(1, input.Buffer.ReadRemaining());

			Assert.AreEqual("Test", output.Buffer.ReadCString());
			Assert.AreEqual("Test2", output.Buffer.ReadCString());
		}
	}
}

