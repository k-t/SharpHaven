using MonoHaven.Resources;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Layers
{
	[TestFixture]
	public class AnimDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new AnimData
			{
				Id = 42,
				Duration = 900,
				Frames = new short[] { 1, 2, 3, 4, 5 }
			};

			var serializer = new AnimDataSerializer();
			var output = (AnimData)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Duration, Is.EqualTo(input.Duration));
			Assert.That(output.Frames, Is.EquivalentTo(input.Frames));
		}
	}
}
