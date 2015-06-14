using System;
using System.Collections.Generic;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Resources;
using SharpHaven.Utils;

namespace SharpHaven.Client
{
	public class ClientResources
	{
		private readonly Dictionary<int, string> dict;

		public ClientResources()
		{
			dict = new Dictionary<int, string>();
		}

		public Delayed<Resource> Get(int id)
		{
			return Get(id, App.Resources.Load);
		}

		public Delayed<T> Get<T>(int id) where T : class
		{
			return Get(id, n => App.Resources.Get<T>(n));
		}

		public Delayed<ISprite> GetSprite(int id, byte[] spriteState = null)
		{
			return Get(id, n => App.Resources.GetSprite(n, spriteState));
		}

		public void Load(ushort resId, string resName)
		{
			dict[resId] = resName;
		}

		private Delayed<T> Get<T>(int id, Func<string, T> getter)
			where T : class
		{
			string resName;
			return dict.TryGetValue(id, out resName)
				? new Delayed<T>(getter(resName))
				: new Delayed<T>((out T res) =>
				{
					if (dict.TryGetValue(id, out resName))
					{
						res = getter(resName);
						return true;
					}
					res = null;
					return false;
				});
		}
	}
}
