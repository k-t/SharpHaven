using System;
using System.Collections.Generic;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Text;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class Chat : Widget
	{
		private readonly string title;
		private readonly List<TextLine> lines;
		private readonly Scrollbar scrollbar;

		public Chat(Widget parent, string title, bool closable) : base(parent)
		{
			this.title = title;
			this.lines = new List<TextLine>();
			this.scrollbar = new Scrollbar(this);
		}

		public event Action<string> MessageOut;

		public string Title
		{
			get { return title; }
		}

		private int DisplayLineCount
		{
			get { return Height / Fonts.Text.Height; }
		}

		public void AddMessage(string text, Color? color)
		{
			var line = new TextLine(Fonts.Text);
			line.TextColor = Color.White;
			line.Append(text);
			lines.Add(line);

			scrollbar.Max = lines.Count - DisplayLineCount;
		}

		public void SendMessage(string text)
		{
			MessageOut.Raise(text);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			int x = 5;
			int y = 5;

			for (int i = 0; i < DisplayLineCount; i++)
			{
				int scrollIndex = scrollbar.Value + i;
				if (scrollIndex >= lines.Count)
					break;

				dc.Draw(lines[scrollIndex], x, y);
				y += lines[scrollIndex].Font.Height;
			}
		}

		protected override void OnDispose()
		{
			foreach (var line in lines)
				line.Dispose();
		}

		protected override void OnMouseWheel(MouseWheelEvent e)
		{
			scrollbar.Value -= e.Delta;
		}

		protected override void OnSizeChanged()
		{
			scrollbar.Move(Width - scrollbar.Width, 1);
			scrollbar.Resize(scrollbar.Width, Height - 2);
		}
	}
}
