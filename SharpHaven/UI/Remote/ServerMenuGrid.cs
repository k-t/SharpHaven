using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Haven;
using Haven.Utils;
using SharpHaven.Client;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerMenuGrid : ServerWidget
	{
		private readonly Dictionary<GameAction, MenuNode> nodes;
		private MenuGrid widget;

		public ServerMenuGrid(ushort id, ServerWidget parent) : base(id, parent)
		{
			nodes = new Dictionary<GameAction, MenuNode>();
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

		protected override void OnInit(Point2D position, object[] args)
		{
			widget = Session.Screen.MenuGrid;
			widget.Visible = true;

			foreach (var action in Session.Actions)
				CreateMenuNode(action);

			Session.Actions.CollectionChanged += OnActionCollectionChanged;
		}

		protected override void OnDestroy()
		{
			widget.Visible = false;

			foreach (var node in nodes.Keys.ToArray())
				RemoveMenuNode(node);

			Session.Actions.CollectionChanged -= OnActionCollectionChanged;
		}

		private void Goto(object[] args)
		{
			var resName = (string)args[0];
			if (!string.IsNullOrEmpty(resName))
			{
				var action = Session.Actions.Get(resName);
				MenuNode node;
				if (action != null && nodes.TryGetValue(action, out node))
					widget.Current = null;
			}
			else
				widget.Current = null;
		}

		private MenuNode CreateMenuNode(GameAction action)
		{
			var node = new MenuNode();
			node.Name = action.Name;
			node.Image = action.Image;
			node.Tag = action;
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
			var action = (GameAction)node.Tag;
			SendMessage("act", action.Verbs.ToArray<object>());
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
