using MonoHaven.Resources;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Layers
{
	[TestFixture]
	public class ActionDataTests
	{
		[Test]
		public void SerializationWorks()
		{
			var input = new ActionData
			{
				Hotkey = 'h',
				Name = "Name",
				Parent = new ResourceRef("Parent", 42),
				Prerequisite = "Prerequisite",
				Verbs = new[] { "verb1", "verb2" }
			};

			var serializer = new ActionDataSerializer();
			var output = (ActionData)serializer.Reserialize(input);

			Assert.That(output.Hotkey, Is.EqualTo(input.Hotkey));
			Assert.That(output.Name, Is.EqualTo(input.Name));
			Assert.That(output.Parent.Name, Is.EqualTo(input.Parent.Name));
			Assert.That(output.Parent.Version, Is.EqualTo(input.Parent.Version));
			Assert.That(output.Prerequisite, Is.EqualTo(input.Prerequisite));
			Assert.That(output.Verbs, Is.EquivalentTo(input.Verbs));
		}
	}
}
