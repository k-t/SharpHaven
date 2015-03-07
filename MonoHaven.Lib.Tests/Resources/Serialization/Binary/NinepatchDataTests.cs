using MonoHaven.Resources;
using MonoHaven.Resources.Serialization.Binary;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Serialization.Binary
{
	[TestFixture]
	public class NinepatchDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new NinepatchData
			{
				Top = 1,
				Bottom = 2,
				Left = 3,
				Right = 4
			};

			var serializer = new NinepatchDataSerializer();
			var output = (NinepatchData)serializer.Reserialize(input);

			Assert.That(output.Top, Is.EqualTo(input.Top));
			Assert.That(output.Bottom, Is.EqualTo(input.Bottom));
			Assert.That(output.Left, Is.EqualTo(input.Left));
			Assert.That(output.Right, Is.EqualTo(input.Right));
		}
	}
}
