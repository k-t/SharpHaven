using Haven.Resources;

namespace SharpHaven.Resources
{
	public interface IObjectFactory<out T> where T : class
	{
		T Create(string resName, Resource res);
	}
}
