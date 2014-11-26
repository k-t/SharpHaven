using System.Drawing;
using MonoHaven.Resources;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Layers
{
	[TestFixture]
	public class NegDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new NegData
			{
				Center = new Point(12, 21),
				Bc = new Point(1, 2),
				Bs = new Point(3, 4),
				Sz = new Point(5, 6),
				Ep = new Point[8][]
			};
			input.Ep[2] = new[] { new Point(45, 54), new Point(46, 64) };
			input.Ep[6] = new[] { new Point(47, 74), new Point(48, 84) };

			var serializer = new NegDataSerializer();
			var output = (NegData)serializer.Reserialize(input);

			Assert.That(output.Center, Is.EqualTo(input.Center));
			Assert.That(output.Bc, Is.EqualTo(input.Bc));
			Assert.That(output.Bs, Is.EqualTo(input.Bs));
			Assert.That(output.Sz, Is.EqualTo(input.Sz));
			for (int i = 0; i < 8; i++)
				Assert.That(output.Ep[i], Is.EqualTo(input.Ep[i]));
		}
	}
}
