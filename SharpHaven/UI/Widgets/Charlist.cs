using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Sprites;
using SharpHaven.Graphics.Text;
using SharpHaven.Input;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class Charlist : Widget
	{
		private const int Spacing = 6;

		private static readonly Drawable background;
		private static readonly Drawable scrollUp;
		private static readonly Drawable scrollDown;

		private readonly List<ListItem> items;
		private readonly int listHeight;
		private readonly Button btnScrollUp;
		private readonly Button btnScrollDown;
		private int scrollOffset;
		
		static Charlist()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/avakort");
			scrollUp = App.Resources.Get<Drawable>("gfx/hud/slen/sau");
			scrollDown = App.Resources.Get<Drawable>("gfx/hud/slen/sad");
		}

		public Charlist(Widget parent, int listHeight) : base(parent)
		{
			this.Resize(background.Width, 40 + (background.Height * listHeight) + (Spacing * (listHeight - 1)));

			this.items = new List<ListItem>(listHeight);
			this.listHeight = listHeight;

			btnScrollUp = new Button(this, 100);
			btnScrollUp.Image = scrollUp;
			btnScrollUp.Visible = false;
			btnScrollUp.Click += () => Scroll(-1);

			btnScrollDown = new Button(this, 100);
			btnScrollDown.Image = scrollDown;
			btnScrollDown.Move(0, Height - 19);
			btnScrollDown.Visible = false;
			btnScrollDown.Click += () => Scroll(1);
		}

		public event Action<string> CharacterSelected;

		public void AddChar(string name, IEnumerable<Delayed<ISprite>> layers)
		{
			var item = new ListItem(this, name, layers);
			item.Selected += () => CharacterSelected.Raise(name);
			items.Add(item);
			UpdateItems();
			btnScrollDown.Visible = btnScrollUp.Visible = items.Count > listHeight;
		}

		protected override void OnMouseWheel(MouseWheelEvent e)
		{
			Scroll(-e.Delta);
			e.Handled = true;
		}

		private void Scroll(int amount)
		{
			scrollOffset += amount;
			scrollOffset = MathHelper.Clamp(scrollOffset, 0, items.Count - listHeight);
			UpdateItems();
		}

		private void UpdateItems()
		{
			int y = 20;
			for (int i = 0; i < items.Count; i++)
			{
				var item = items[i];
				item.Visible = (i >= scrollOffset && i < scrollOffset + listHeight);
				if (item.Visible)
				{
					item.Move(0, y);
					y += item.Height + Spacing;
				}
			}
		}

		private class ListItem : Widget
		{
			private readonly TextLine nameTextLine;
			private readonly AvatarView avatar;
			private readonly Button btnPlay;

			public ListItem(Widget parent, string charName, IEnumerable<Delayed<ISprite>> layers)
				: base(parent)
			{
				Size = background.Size;
				
				nameTextLine = new TextLine(Fonts.Heading);
				nameTextLine.TextColor = Color.White;
				nameTextLine.Append(charName);

				btnPlay = new Button(this, 100);
				btnPlay.Text = "Play";
				btnPlay.Move(Width - 105, Height - 24);
				btnPlay.Click += () => Selected.Raise();

				avatar = new AvatarView(this);
				avatar.Avatar = new Avatar(layers);
				var padding = (Height - avatar.Height) / 2;
				avatar.Move(padding, padding);
			}

			public event Action Selected;

			protected override void OnDraw(DrawingContext dc)
			{
				dc.Draw(background, 0, 0);
				dc.Draw(nameTextLine, avatar.Bounds.Right + 5, avatar.Y);
			}

			protected override void OnDispose()
			{
				nameTextLine.Dispose();
			}
		}
	}
}
