using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoHaven.Core
{
	public class PrioQueue<T> where T : IPrioritized
	{
		private readonly LinkedList<T> _list = new LinkedList<T>();

		public void Add(T item)
		{
			_list.AddLast(item);
		}

		public T Peek()
		{
			T highest = default(T);
			bool first = true;
			foreach (var item in _list)
			{
				if (first || highest.Priority < item.Priority)
				{
					highest = item;
					first = false;
				}
			}
			return highest;
		}

		public T Poll()
		{
			T item = Peek();
			_list.Remove(item);
			return item;
		}
	}
}

