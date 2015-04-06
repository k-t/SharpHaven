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
		private const int PageSize = RowCount * ColumnCount;

		private static readonly Drawable backImage;
		private static readonly Drawable nextImage;
		private static readonly Drawable cellImage;

		private readonly MenuNode root;
		private readonly MenuButton[,] buttons;
		private MenuButton back;
		private MenuButton next;
		private MenuNode current;
		private int currentOffset;

		static MenuGrid()
		{
			backImage = App.Resources.Get<Drawable>("gfx/hud/sc-back");
			nextImage = App.Resources.Get<Drawable>("gfx/hud/sc-next");
			cellImage = App.Resources.Get<Drawable>("gfx/hud/invsq");
		}

		public MenuGrid(Widget parent) : base(parent)
		{
			buttons = new MenuButton[RowCount, ColumnCount];

			root = new MenuNode();
			root.Children.CollectionChanged += (s, e) => UpdateButtons();

			// create buttons
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

			Current = root;
		}

		public MenuNode Current
		{
			get { return current; }
			set
			{
				current = value ?? root;
				currentOffset = 0;
				UpdateButtons();
			}
		}

		public ICollection<MenuNode> Nodes
		{
			get { return root.Children; }
		}

		private void UpdateButtons()
		{
			var all = current.Children.ToList();
			all.Sort();
			var page = all.Skip(currentOffset).Take(PageSize).ToList();

			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
					if (i * ColumnCount + j < page.Count)
					{
						var node = page[i * ColumnCount + j];
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

			if (page.Count >= PageSize - 1)
			{
				next = buttons[RowCount - 1, ColumnCount - 2];
				next.Image = nextImage;
				next.Node = null;
				next.Tooltip = null;
			}
		}

		private void OnButtonClick(object sender, MouseButtonEvent e)
		{
			if (sender == back)
				Current = Current.Parent;
			else if (sender == next)
			{
				currentOffset += (RowCount * ColumnCount) - 2;
				UpdateButtons();
			}
			else
			{
				var button = (MenuButton)sender;
				if (button.Node != null)
				{
					if (button.Node.Children.Any())
						Current = button.Node;
					else
						button.Node.Activate();
				}
			}
		}
	}
}
