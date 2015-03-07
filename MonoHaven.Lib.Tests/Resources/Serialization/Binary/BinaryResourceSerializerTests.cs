using System;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Resources.Serialization.Binary;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Serialization.Binary
{
	[TestFixture]
	public class BinaryResourceSerializerTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new Resource(42, new Object[] {
				new ActionData
				{
					Hotkey = 'h',
					Name = "Name",
					Parent = new ResourceRef("Parent", 42),
					Prerequisite = "Prerequisite",
					Verbs = new[] { "verb1", "verb2" }
				},
				new TextData { Text = "Text1" },
				new ImageData
				{
					Id = 42,
					Offset = new Point(1, 2),
					Z = -10,
					SubZ = -20,
					Data = new byte[] { 1, 2, 3, 4, 5 },
					IsLayered = true
				},
				new TextData { Text = "Text2" }
			});

			Resource output;
			using (var ms = new MemoryStream())
			{
				var serializer = new BinaryResourceSerializer();
				serializer.Serialize(input, ms);
				ms.Position = 0;
				output = serializer.Deserialize(ms);
			}

			Assert.That(output.Version, Is.EqualTo(input.Version));
			Assert.That(output.GetLayers().Count(), Is.EqualTo(input.GetLayers().Count()));
			Assert.That(output.GetLayer<ActionData>(), Is.Not.Null);
			Assert.That(output.GetLayer<ImageData>(), Is.Not.Null);
			Assert.That(output.GetLayers<TextData>().Count(), Is.EqualTo(2));
		}
	}
}
