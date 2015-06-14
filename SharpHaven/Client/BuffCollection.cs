using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpHaven.Client
{
	public class BuffCollection : IEnumerable<Buff>
	{
		private readonly Dictionary<int, Buff> dict;

		public BuffCollection()
		{
			dict = new Dictionary<int, Buff>();
		}

		public event Action Changed;

		public void AddOrUpdate(Buff buff)
		{
			dict[buff.Id] = buff;
			Changed.Raise();
		}

		public void Remove(int buffId)
		{
			if (dict.Remove(buffId));
				Changed.Raise();
		}

		public void Clear()
		{
			dict.Clear();
			Changed.Raise();
		}

		public IEnumerator<Buff> GetEnumerator()
		{
			return dict.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
