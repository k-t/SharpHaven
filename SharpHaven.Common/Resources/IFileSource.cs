namespace SharpHaven.Resources
{
	public interface IFileSource
	{
		byte[] Read(string path);
		void Write(string path, byte[] bytes);
	}
}
