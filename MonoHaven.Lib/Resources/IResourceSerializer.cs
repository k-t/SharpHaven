using System.IO;

namespace MonoHaven.Resources
{
	public interface IResourceSerializer
	{
		Resource Deserialize(Stream inputStream);
		void Serialize(Resource res, Stream outputStream);
	}
}
