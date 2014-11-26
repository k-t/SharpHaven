using MonoHaven.Resources;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Layers
{
	[TestFixture]
	public class TilesetDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new TilesetData
			{
				FlavorDensity = 128,
				HasTransitions = true,
				FlavorObjects = new[] {
					new FlavorObjectData { ResName = "res1", ResVersion = 1, Weight  = 1 },
					new FlavorObjectData { ResName = "res2", ResVersion = 2, Weight  = 2 },
					new FlavorObjectData { ResName = "res3", ResVersion = 3, Weight  = 3 },
				}
			};

			var serializer = new TilesetDataSerializer();
			var output = (TilesetData)serializer.Reserialize(input);

			Assert.That(output.FlavorDensity, Is.EqualTo(input.FlavorDensity));
			Assert.That(output.HasTransitions, Is.EqualTo(input.HasTransitions));
			Assert.That(output.FlavorObjects, Is.EquivalentTo(input.FlavorObjects));
		}
	}
}
