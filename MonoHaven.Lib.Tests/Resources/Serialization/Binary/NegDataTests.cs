using System.Drawing;
using NUnit.Framework;

namespace SharpHaven.Resources.Serialization.Binary
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
				Hitbox = Rectangle.FromLTRB(1, 2, 3, 4),
				Sz = new Point(5, 6),
				Ep = new Point[8][]
			};
			input.Ep[2] = new[] { new Point(45, 54), new Point(46, 64) };
			input.Ep[6] = new[] { new Point(47, 74), new Point(48, 84) };

			var serializer = new NegDataSerializer();
			var output = (NegData)BinarySerializerHelper.Reserialize(serializer, input);

			Assert.That(output.Center, Is.EqualTo(input.Center));
			Assert.That(output.Hitbox, Is.EqualTo(input.Hitbox));
			Assert.That(output.Sz, Is.EqualTo(input.Sz));
			for (int i = 0; i < 8; i++)
				Assert.That(output.Ep[i], Is.EqualTo(input.Ep[i]));
		}
	}
}
