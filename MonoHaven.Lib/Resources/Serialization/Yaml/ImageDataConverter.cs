using System;
using System.Drawing;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace MonoHaven.Resources.Serialization.Yaml
{
	internal class ImageDataConverter : IYamlTypeConverter
	{
		public bool Accepts(Type type)
		{
			return type == typeof(ImageData);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			if (!typeof(ImageData).IsAssignableFrom(type))
				return null;

			// set default image properties
			var data = new ImageData {
				Id = -1,
				IsLayered = false,
				Offset = Point.Empty,
				Z = 0,
				SubZ = 0
			};

			if (!(parser.Current is MappingStart))
				throw new YamlException(parser.Current.Start, parser.Current.End, "Invalid format");

			var reader = new EventReader(parser);
			while (!reader.Accept<MappingEnd>())
			{
				var key = reader.Expect<Scalar>().Value;
				var value = reader.Expect<Scalar>().Value;
				switch (key)
				{
					case "file":
						data.Data = File.ReadAllBytes(value);
						break;
				}
			}
			reader.Expect<MappingEnd>();

			return data;
		}

		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			throw new NotImplementedException();
		}
	}
}
