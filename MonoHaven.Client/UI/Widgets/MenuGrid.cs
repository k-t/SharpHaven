using System;
using System.Collections.Generic;
using System.Linq;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI.Layouts;

namespace MonoHaven.UI.Widgets
{
	public class MenuGrid : Widget
	{
		private const int RowCount = 4;
		private const int ColumnCount = 4;

		private static readonly Drawable backImage;
		private static readonly Drawable nextImage;
		private static readonly Drawable cellImage;

		private GameActionTree actionTree;
		private GameActionComparer actionComparer;
		private readonly MenuButton[,] buttons;
		private MenuButton back;
		private MenuButton next;
		private GameAction current;

		static MenuGrid()
		{
			backImage = App.Resources.Get<Drawable>("gfx/hud/sc-back");
			nextImage = App.Resources.Get<Drawable>("gfx/hud/sc-next");
			cellImage = App.Resources.Get<Drawable>("gfx/hud/invsq");
		}

		public MenuGrid(Widget parent) : base(parent)
		{
			buttons = new MenuButton[RowCount, ColumnCount];

			var layout = new GridLayout();
			for (int row = 0; row < RowCount; row++)
				for (int col = 0; col < ColumnCount; col++)
				{
					var button = new MenuButton(this);
					button.Click += OnButtonClick;
					buttons[row, col] = button;
					layout.AddWidget(button, row, col);
					layout.SetColumnWidth(col, cellImage.Width);
				}
			layout.Spacing = -1;
			layout.UpdateGeometry(0, 0, 0, 0);

			Resize(cellImage.Width * ColumnCount, cellImage.Height * RowCount);
		}

		public event Action<GameAction> Act;

		public GameActionTree Actions
		{
			get { return actionTree; }
			set
			{
				if (actionTree != null)
					actionTree.Changed -= UpdateButtons;

				actionTree = value;

				if (actionTree != null)
				{
					actionTree.Changed += UpdateButtons;
					actionComparer = new GameActionComparer(actionTree);
					current = actionTree.Root;
				}
				UpdateButtons();
			}
		}

		public void Goto(string resName)
		{
			if (Actions == null)
				throw new InvalidOperationException();

			current = actionTree.GetByName(resName) ?? actionTree.Root;
			UpdateButtons();
		}

		private void UpdateButtons()
		{
			if (Actions == null)
				return;

			var children = Actions.GetChildren(current).ToArray();
			Array.Sort(children, actionComparer);
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
					if (i * ColumnCount + j < children.Length)
					{
						var action = children[i * ColumnCount + j];
						buttons[i, j].Image = action.Image;
						buttons[i, j].Tooltip = new Tooltip(action.Tooltip);
						buttons[i, j].Tag = action;
					}
					else
					{
						buttons[i, j].Image = null;
						buttons[i, j].Tag = null;
						buttons[i, j].Tooltip = null;
					}

			if (!string.IsNullOrEmpty(current.Name))
			{
				back = buttons[RowCount - 1, ColumnCount - 1];
				back.Image = backImage;
				back.Tag = null;
				back.Tooltip = null;
			}
			else
				back = null;
		}

		private void OnButtonClick(object sender, MouseButtonEvent e)
		{
			if (sender == back)
				Goto(current.Parent.Name);
			else
			{
				var button = (MenuButton)sender;
				var action = button.Tag as GameAction;
				if (action != null)
				{
					if (Actions.HasChildren(action))
					{
						current = action;
						UpdateButtons();
					}
					else
						Act.Raise((GameAction)button.Tag);
				}
			}
		}

		private class GameActionComparer : IComparer<GameAction>
		{
			private readonly GameActionTree tree;

			public GameActionComparer(GameActionTree tree)
			{
				this.tree = tree;
			}

			public int Compare(GameAction x, GameAction y)
			{
				if (x == y) return 0;
				if (x == null) return -1;
				if (y == null) return 1;
				if (tree.HasChildren(y) != tree.HasChildren(x))
					return tree.HasChildren(y) ? 1 : -1;
				return string.CompareOrdinal(x.Name, y.Name);
			}
		}
	}
}
