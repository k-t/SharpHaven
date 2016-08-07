using System.IO;
using NUnit.Framework;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary
{
	public static class BinarySerializerHelper
	{
		public static object Reserialize(this IBinaryLayerHandler handler, object input)
		{
			object output;
			using (var ms = new MemoryStream())
			{
				handler.Serialize(new ByteBuffer(ms), input);
				ms.Position = 0;
				output = handler.Deserialize(new ByteBuffer(ms));
				Assert.That(ms.Position, Is.EqualTo(ms.Length), "Stream should be read till the end");
			}
			return output;
		}
	}
}
