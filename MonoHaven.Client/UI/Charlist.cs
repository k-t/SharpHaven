using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Utils;
using OpenTK;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class Charlist : Widget
	{
		private const int Margin = 6;

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
			background = App.Instance.Resources.GetTexture("gfx/hud/avakort");
			scrollUp = App.Instance.Resources.GetTexture("gfx/hud/slen/sau");
			scrollDown = App.Instance.Resources.GetTexture("gfx/hud/slen/sad");
		}

		public Charlist(Widget parent, int listHeight) : base(parent)
		{
			SetSize(background.Width, 40 + (background.Height * listHeight) + (Margin * (listHeight - 1)));

			this.items = new List<ListItem>(listHeight);
			this.listHeight = listHeight;

			btnScrollUp = new Button(this, 100);
			btnScrollUp.Image = scrollUp;
			btnScrollUp.Visible = false;
			btnScrollUp.Clicked += () => Scroll(-1);

			btnScrollDown = new Button(this, 100);
			btnScrollDown.Image = scrollDown;
			btnScrollDown.SetLocation(0, Height - 19);
			btnScrollDown.Visible = false;
			btnScrollDown.Clicked += () => Scroll(1);
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

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			Scroll(-e.Delta);
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
					item.SetLocation(0, y);
					y += item.Height + Margin;
				}
			}
		}

		private class ListItem : Widget
		{
			private readonly TextBlock nameTextBlock;
			private readonly AvatarView avatar;
			private readonly Button btnPlay;

			public ListItem(Widget parent, string charName, IEnumerable<Delayed<ISprite>> layers)
				: base(parent)
			{
				SetSize(background.Width, background.Height);
				
				nameTextBlock = new TextBlock(Fonts.Heading);
				nameTextBlock.TextColor = Color.White;
				nameTextBlock.Append(charName);

				btnPlay = new Button(this, 100);
				btnPlay.Text = "Play";
				btnPlay.SetLocation(Width - 105, Height - 24);
				btnPlay.Clicked += () => Selected.Raise();

				avatar = new AvatarView(this, layers);
				var padding = (Height - avatar.Height) / 2;
				avatar.SetLocation(padding, padding);
			}

			public event Action Selected;

			protected override void OnDraw(DrawingContext dc)
			{
				dc.Draw(background, 0, 0);
				dc.Draw(nameTextBlock, avatar.Bounds.Right + 5, avatar.Y);
			}

			protected override void OnMouseWheel(MouseWheelEventArgs e)
			{
				Parent.MouseWheel(e);
			}

			protected override void OnDispose()
			{
				nameTextBlock.Dispose();
			}
		}
	}
}
