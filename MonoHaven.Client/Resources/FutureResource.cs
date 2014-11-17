using System;

namespace MonoHaven.Resources
{
	public class FutureResource
	{
		private Resource resource;
		private Func<Resource> getter;

		public FutureResource(Func<Resource> getter)
		{
			this.getter = getter;
		}

		public Resource Value
		{
			get
			{
				if (resource == null)
					resource = getter();
				return resource;
			}
		}
	}
}
