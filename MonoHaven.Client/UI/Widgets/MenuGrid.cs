using System;
using System.Collections.Generic;
using System.Linq;
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

		private readonly MenuNode root;
		private readonly MenuButton[,] buttons;
		private MenuButton back;
		private MenuButton next;
		private MenuNode current;

		static MenuGrid()
		{
			backImage = App.Resources.Get<Drawable>("gfx/hud/sc-back");
			nextImage = App.Resources.Get<Drawable>("gfx/hud/sc-next");
			cellImage = App.Resources.Get<Drawable>("gfx/hud/invsq");
		}

		public MenuGrid(Widget parent) : base(parent)
		{
			root = new MenuNode();
			root.Children.CollectionChanged += (s, e) => UpdateButtons();

			buttons = new MenuButton[RowCount, ColumnCount];
			current = root;

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

			UpdateButtons();
		}

		public ICollection<MenuNode> Nodes
		{
			get { return root.Children; }
		}

		public void Goto(MenuNode node)
		{
			current = node ?? root;
			UpdateButtons();
		}

		private void UpdateButtons()
		{
			var children = current.Children.ToArray();
			Array.Sort(children);
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
					if (i * ColumnCount + j < children.Length)
					{
						var node = children[i * ColumnCount + j];
						buttons[i, j].Image = node.Image;
						buttons[i, j].Tooltip = new Tooltip(node.Tooltip);
						buttons[i, j].Node = node;
					}
					else
					{
						buttons[i, j].Image = null;
						buttons[i, j].Node = null;
						buttons[i, j].Tooltip = null;
					}

			if (current.Parent != null)
			{
				back = buttons[RowCount - 1, ColumnCount - 1];
				back.Image = backImage;
				back.Node = null;
				back.Tooltip = null;
			}
			else
				back = null;
		}

		private void OnButtonClick(object sender, MouseButtonEvent e)
		{
			if (sender == back)
				Goto(current.Parent);
			else
			{
				var button = (MenuButton)sender;
				if (button.Node != null)
				{
					if (button.Node.Children.Any())
					{
						current = button.Node;
						UpdateButtons();
					}
					else
						button.Node.Activate();
				}
			}
		}
	}
}
