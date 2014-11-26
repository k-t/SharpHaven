using System.IO;
using MonoHaven.Resources;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources
{
	public static class SerializerHelper
	{
		public static object Reserialize(this IDataLayerSerializer serializer, object input)
		{
			object output;
			using (var ms = new MemoryStream())
			{
				serializer.Serialize(new BinaryWriter(ms), input);
				ms.Position = 0;
				output = serializer.Deserialize((int)ms.Length, new BinaryReader(ms));
				Assert.That(ms.Position, Is.EqualTo(ms.Length), "Stream should be read till the end");
			}
			return output;
		}
	}
}
