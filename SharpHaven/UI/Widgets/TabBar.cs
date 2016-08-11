using System;
using System.Collections.Generic;
using Haven;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Text;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class TabBar : Widget
	{
		private const int TabPadding = 2;

		private static readonly Drawable tabImage;
		private static readonly int tabHeight;

		static TabBar()
		{
			tabImage = App.Resources.Get<Drawable>("gfx/tab");
			tabHeight = TabPadding * 2 + Fonts.Default.Height;
		}

		private readonly List<TabButton> buttons;

		public TabBar(Widget parent) : base(parent)
		{
			buttons = new List<TabButton>();
			this.Resize(Width, tabHeight);
		}

		public event Action<int> Click;

		public int ButtonCount
		{
			get { return buttons.Count; }
		}

		public void AddButton(string text)
		{
			var button = new TabButton(this);
			button.Text = text;
			button.Click += OnButtonClick;
			buttons.Add(button);

			UpdateLayout();
		}

		public void RemoveButton(int index)
		{
			if (index < 0 || index >= buttons.Count)
				return;

			var button = buttons[index];
			button.Click -= OnButtonClick;
			button.Remove();
			button.Dispose();
			buttons.RemoveAt(index);

			UpdateLayout();
		}

		public void SetActiveButton(int index)
		{
			for (int i = 0; i < buttons.Count; i++)
				buttons[i].IsActive = (i == index);
		}

		private void OnButtonClick(object sender, EventArgs e)
		{
			var button = (TabButton)sender;
			Click.Raise(buttons.IndexOf(button));
		}

		private void UpdateLayout()
		{
			int x = 0;
			foreach (var button in buttons)
			{
				button.Move(x, 0);
				x += button.Width;
			}
		}

		private class TabButton : Widget
		{
			private Color backColor;
			private bool isActive;
			private string text;
			private TextLine label;

			public TabButton(Widget parent) : base(parent)
			{
				label = new TextLine(Fonts.Default);
				UpdateColors();
			}

			public event EventHandler Click;

			public bool IsActive
			{
				get { return isActive; }
				set
				{
					isActive = value;
					UpdateColors();
				}
			}

			public string Text
			{
				get { return text; }
				set
				{
					text = value;
					UpdateLabel();
				}
			}

			protected override void OnDraw(DrawingContext dc)
			{
				dc.SetColor(backColor);
				dc.Draw(tabImage, 0, 0, Width, Height);
				dc.ResetColor();
				dc.Draw(label, TabPadding, TabPadding);
			}

			protected override void OnMouseButtonDown(MouseButtonEvent e)
			{
				if (e.Button == MouseButton.Left)
					Click.Raise(this, EventArgs.Empty);
			}

			protected override void OnDispose()
			{
				if (label != null)
					label.Dispose();
			}

			private void UpdateLabel()
			{
				label.Clear();
				label.Append(text ?? "");
				this.Resize(label.TextWidth + TabPadding * 2, tabHeight);
			}

			private void UpdateColors()
			{
				label.TextColor = isActive ? Color.White : Color.FromArgb(200, 128, 128, 128);
				backColor = isActive ? Color.White : Color.FromArgb(128, 128, 128, 128);
			}
		}
	}
}
