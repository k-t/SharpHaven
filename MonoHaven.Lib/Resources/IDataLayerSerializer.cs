using System.IO;

namespace MonoHaven.Resources
{
	public interface IDataLayerSerializer
	{
		string LayerName { get; }
		object Deserialize(int size, BinaryReader reader);
	}
}

