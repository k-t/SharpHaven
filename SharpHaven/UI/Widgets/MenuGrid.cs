using System.Collections.Generic;
using System.Linq;
using SharpHaven.Graphics;
using SharpHaven.Input;
using SharpHaven.UI.Layouts;

namespace SharpHaven.UI.Widgets
{
	public class MenuGrid : Widget
	{
		private const int PageRows = 4;
		private const int PageColumns = 4;
		private const int PageSize = PageRows * PageColumns;

		private static readonly Drawable backImage;
		private static readonly Drawable nextImage;
		private static readonly Drawable cellImage;

		private readonly MenuNode root;
		private readonly MenuButton[] buttons;
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
			buttons = new MenuButton[PageSize];

			root = new MenuNode();
			root.Children.CollectionChanged += (s, e) => UpdateButtons();

			// create buttons
			var layout = new GridLayout();
			for (int i = 0; i < PageSize; i++)
			{
				var button = new MenuButton(this);
				button.Click += OnButtonClick;
				buttons[i] = button;

				int row = i / PageRows;
				int col = i % PageRows;
				layout.AddWidget(button, row, col);
				layout.SetColumnWidth(col, cellImage.Width);
			}
			layout.Spacing = -1;
			layout.UpdateGeometry(0, 0, 0, 0);

			Resize(cellImage.Width * PageColumns, cellImage.Height * PageRows);

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

			for (int i = 0; i < PageSize; i++)
				if (i < page.Count)
				{
					var node = page[i];
					buttons[i].Image = node.Image;
					buttons[i].Tooltip = new Tooltip(node.Tooltip);
					buttons[i].Node = node;
				}
				else
				{
					buttons[i].Image = null;
					buttons[i].Node = null;
					buttons[i].Tooltip = null;
				}

			if (current.Parent != null)
			{
				back = buttons[PageSize - 1];
				back.Image = backImage;
				back.Node = null;
				back.Tooltip = null;
			}
			else
				back = null;

			if (page.Count >= PageSize - 1)
			{
				next = buttons[PageSize - 2];
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
				currentOffset += (PageRows * PageColumns) - 2;
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
