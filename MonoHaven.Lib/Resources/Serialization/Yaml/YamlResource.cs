using YamlDotNet.Serialization;

namespace MonoHaven.Resources.Serialization.Yaml
{
	internal class YamlResource
	{
		[YamlMember(Alias = "version")]
		public int Version { get; set; }

		[YamlMember(Alias = "layers")]
		public object[] Layers { get; set; }
	}
}
