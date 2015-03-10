﻿using MonoHaven.Resources;
using MonoHaven.Resources.Serialization.Binary;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Serialization.Binary
{
	[TestFixture]
	public class TileDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new TileData
			{
				Id = 42,
				Type = 'g',
				Weight = 123,
				ImageData = new byte[] { 1, 2, 3, 4, 6 }
			};

			var serializer = new TileDataSerializer();
			var output = (TileData)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Type, Is.EqualTo(input.Type));
			Assert.That(output.Weight, Is.EqualTo(input.Weight));
			Assert.That(output.ImageData, Is.EquivalentTo(input.ImageData));
		}
	}
}