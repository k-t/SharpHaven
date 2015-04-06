using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using C5;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerMenuGrid : ServerWidget
	{
		private readonly HashDictionary<GameAction, MenuNode> nodes;
		private MenuGrid widget;

		public ServerMenuGrid(ushort id, ServerWidget parent) : base(id, parent)
		{
			nodes = new HashDictionary<GameAction, MenuNode>();
			SetHandler("goto", Goto);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerMenuGrid(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			widget = Session.State.Screen.MenuGrid;
			widget.Visible = true;
			
			foreach (var action in Session.State.Actions)
				CreateMenuNode(action);

			Session.State.Actions.CollectionChanged += OnActionCollectionChanged;
		}

		protected override void OnDestroy()
		{
			widget.Visible = false;

			foreach (var node in nodes.Keys.ToArray())
				RemoveMenuNode(node);

			Session.State.Actions.CollectionChanged -= OnActionCollectionChanged;
		}

		private void Goto(object[] args)
		{
			var resName = (string)args[0];

			if (!string.IsNullOrEmpty(resName))
			{
				var action = Session.State.Actions.Get(resName);
				MenuNode node;
				if (action != null && nodes.Find(ref action, out node))
					widget.Goto(node);
			}
			else
				widget.Goto(null);
		}

		private MenuNode CreateMenuNode(GameAction action)
		{
			var node = new MenuNode();
			node.Name = action.Name;
			node.Image = action.Image;
			node.Tooltip = action.Tooltip;
			node.Activated += OnNodeActivated;
			nodes.Add(action, node);

			if (action.HasParent)
			{
				var parentNode = nodes.FirstOrDefault(x => x.Key.ResName == action.Parent.Name).Value;
				if (parentNode == null)
				{
					var parent = App.Resources.Get<GameAction>(action.Parent.Name);
					parentNode = CreateMenuNode(parent);
				}
				parentNode.Children.Add(node);
			}
			else
				widget.Nodes.Add(node);

			return node;
		}

		private void RemoveMenuNode(GameAction action)
		{
			MenuNode node;
			if (nodes.Remove(action, out node))
			{
				node.Activated -= OnNodeActivated;
				node.Parent.Children.Remove(node);
			}
		}

		private void OnNodeActivated(object sender, EventArgs e)
		{
			var node = (MenuNode)sender;
			var pair = nodes.FirstOrDefault(x => x.Value == node);
			if (pair.Key != null)
				SendMessage("act", pair.Key.Verbs.ToArray<object>());
		}

		private void OnActionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (GameAction action in e.NewItems)
						CreateMenuNode(action);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (GameAction action in e.OldItems)
						RemoveMenuNode(action);
					break;
			}
		}
	}
}
