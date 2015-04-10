using System;
using System.ComponentModel;

namespace SharpHaven.Utils
{
	public static class AsyncOperationExtensions
	{
		public static void PostEvent<T>(this AsyncOperation op, Action<T> action, T arg)
		{
			op.Post(_ => action.Raise(arg), null);
		}
	}
}
