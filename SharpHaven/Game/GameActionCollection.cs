using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using C5;

namespace SharpHaven.Game
{
	public class GameActionCollection : IEnumerable<GameAction>, INotifyCollectionChanged
	{
		private readonly TreeDictionary<string, GameAction> items;

		public GameActionCollection()
		{
			items = new TreeDictionary<string, GameAction>();
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public void Add(string resName)
		{
			var action = App.Resources.Get<GameAction>(resName);
			items[resName] = action;
			RaiseCollectionChanged(NotifyCollectionChangedAction.Add, action);
		}

		public GameAction Get(string resName)
		{
			GameAction action;
			return items.Find(ref resName, out action) ? action : null;
		}

		public void Remove(string resName)
		{
			GameAction action;
			if (items.Remove(resName, out action))
				RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, action);
		}

		public IEnumerator<GameAction> GetEnumerator()
		{
			return items.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void RaiseCollectionChanged(NotifyCollectionChangedAction action, object item)
		{
			if (CollectionChanged != null)
				CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item));
		}
	}
}
