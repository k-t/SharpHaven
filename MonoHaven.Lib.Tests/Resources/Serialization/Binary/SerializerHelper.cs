using System.IO;
using MonoHaven.Resources;
using MonoHaven.Resources.Serialization.Binary;
using NUnit.Framework;

namespace MonoHaven.Tests.Resources.Serialization.Binary
{
	public static class BinarySerializerHelper
	{
		public static object Reserialize(this IBinaryDataLayerSerializer serializer, object input)
		{
			object output;
			using (var ms = new MemoryStream())
			{
				serializer.Serialize(new BinaryWriter(ms), input);
				ms.Position = 0;
				output = serializer.Deserialize(new BinaryReader(ms), (int)ms.Length);
				Assert.That(ms.Position, Is.EqualTo(ms.Length), "Stream should be read till the end");
			}
			return output;
		}
	}
}
