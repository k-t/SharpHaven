using System;
using System.Collections.Generic;

namespace MonoHaven.Utils
{
	public class TypeMatcher
	{
		private readonly Dictionary<Type, Action<object>> cases =
			new Dictionary<Type, Action<object>>();

		public TypeMatcher Case<T>(Action<T> action)
		{
			cases.Add(typeof(T), (x) => action((T)x));
			return this;
		}

		public void Match(object x)
		{
			Action<object> action;
			if (cases.TryGetValue(x.GetType(), out action))
				action(x);
		}
	}
}
