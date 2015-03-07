using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace MonoHaven.Resources.Serialization.Yaml
{
	internal class NinepatchDataConverter : IYamlTypeConverter
	{
		public bool Accepts(Type type)
		{
			return type == typeof(NinepatchData);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			if (!typeof(NinepatchData).IsAssignableFrom(type))
				return null;

			var data = new NinepatchData();

			if (!(parser.Current is MappingStart))
				throw new YamlException(parser.Current.Start, parser.Current.End, "Invalid format");

			var reader = new EventReader(parser);
			while (!reader.Accept<MappingEnd>())
			{
				var key = reader.Expect<Scalar>().Value;
				var value = reader.Expect<Scalar>().Value;
				switch (key)
				{
					case "top":
						data.Top = byte.Parse(value);
						break;
					case "bottom":
						data.Bottom = byte.Parse(value);
						break;
					case "left":
						data.Left = byte.Parse(value);
						break;
					case "right":
						data.Right = byte.Parse(value);
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
