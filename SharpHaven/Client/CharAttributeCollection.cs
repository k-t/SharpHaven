using System.Collections.Generic;

namespace SharpHaven.Client
{
	public class CharAttributeCollection
	{
		private readonly Dictionary<string, CharAttribute> dict;

		public CharAttributeCollection()
		{
			dict = new Dictionary<string, CharAttribute>();
		}

		public CharAttribute this[string name]
		{
			get
			{
				CharAttribute attr;
				return dict.TryGetValue(name, out attr) ? attr : null;
			}
		}

		public void AddOrUpdate(string name, int baseValue, int modValue)
		{
			CharAttribute attr;
			if (dict.TryGetValue(name, out attr))
				attr.Update(baseValue, modValue);
			else
			{
				attr = new CharAttribute(name, baseValue, modValue);
				dict[name] = attr;
			}
		}
	}
}
