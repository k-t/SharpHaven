using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MonoHaven.Graphics;
using System;
using System.Linq;

namespace MonoHaven.UI.Widgets
{
	public class MenuNode : IComparable<MenuNode>
	{
		private MenuNode parent;
		private readonly ObservableCollection<MenuNode> children;

		public MenuNode()
		{
			children = new ObservableCollection<MenuNode>();
			children.CollectionChanged += OnChildrenChanged;
		}

		public event EventHandler Activated;

		public MenuNode Parent
		{
			get { return parent; }
		}

		public ObservableCollection<MenuNode> Children
		{
			get { return children; }
		}

		public string Name
		{
			get;
			set;
		}

		public Drawable Image
		{
			get;
			set;
		}

		public string Tooltip
		{
			get;
			set;
		}

		public void Activate()
		{
			Activated.Raise(this, EventArgs.Empty);
		}

		private void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (MenuNode node in e.NewItems)
						node.parent = this;
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (MenuNode node in e.NewItems)
						node.parent = null;
					break;
			}
		}

		public int CompareTo(MenuNode other)
		{
			if (other.Children.Any() != Children.Any())
				return Children.Any() ? -1 : 1;
			return string.CompareOrdinal(Name, other.Name);

		}
	}
}
