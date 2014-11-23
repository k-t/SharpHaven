using System;

namespace MonoHaven.Resources
{
	public struct ResourceRef
	{
		private readonly string name;
		private readonly int version;

		public ResourceRef(string name, int version)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			this.name = name;
			this.version = version;
		}

		public string Name
		{
			get { return name; }
		}

		public int Version
		{
			get { return version; }
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ Version.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ResourceRef))
				return false;
			var other = (ResourceRef)obj;
			return string.Equals(Name, other.Name) && Version == other.Version;
		}
	}
}
