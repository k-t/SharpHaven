using System.IO;

namespace SharpHaven.Resources
{
	public interface IResourceSerializer
	{
		Resource Deserialize(Stream inputStream);
		void Serialize(Resource res, Stream outputStream);
	}
}
