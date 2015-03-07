using System;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace MonoHaven.Resources.Serialization.Yaml
{
	internal class FontDataConverter : IYamlTypeConverter
	{
		public bool Accepts(Type type)
		{
			return type == typeof(FontData);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			if (!typeof(FontData).IsAssignableFrom(type))
				return null;

			var data = new FontData();

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
		}
	}
}
