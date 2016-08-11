using System.Collections.Generic;
using System.Linq;

namespace Haven.Resources.Serialization.Ini
{
	public class IniResource
	{
		private readonly List<IniLayer> layers = new List<IniLayer>();

		public ICollection<IniLayer> Layers
		{
			get { return layers; }
		}

		public int Version { get; set; }

		public Resource ToResource()
		{
			return new Resource(Version, Layers.Select(x => x.Data));
		}
	}
}
