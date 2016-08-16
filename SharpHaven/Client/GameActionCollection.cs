using Haven;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SharpHaven.Client
{
	public class GameActionCollection : IEnumerable<GameAction>, INotifyCollectionChanged
	{
		private readonly Dictionary<string, GameAction> items;

		public GameActionCollection()
		{
			items = new Dictionary<string, GameAction>();
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
			return items.TryGetValue(resName, out action) ? action : null;
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
