using System;
using System.IO;
using YamlDotNet.Serialization;

namespace MonoHaven.Resources.Serialization.Yaml
{
	public class YamlResourceSerializer : IResourceSerializer
	{
		private readonly Deserializer deserializer;

		public YamlResourceSerializer()
		{
			deserializer = new Deserializer();
			
			Register("font", typeof(FontData), new FontDataConverter());
			Register("image", typeof(ImageData), new ImageDataConverter());
			Register("ninepatch", typeof(NinepatchData), new NinepatchDataConverter());
		}

		public void Register(string tag, Type type, IYamlTypeConverter converter)
		{
			deserializer.RegisterTagMapping("!" + tag, type);
			deserializer.RegisterTypeConverter(converter);
		}

		public Resource Deserialize(Stream inputStream)
		{
			using (var reader = new StreamReader(inputStream))
			{
				var res = deserializer.Deserialize<YamlResource>(reader);
				return new Resource(res.Version, res.Layers);
			}
		}

		public void Serialize(Resource res, Stream outputStream)
		{
			throw new NotImplementedException();
		}
	}
}
