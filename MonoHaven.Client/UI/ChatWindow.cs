using System.Collections.Generic;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class ChatWindow : Widget
	{
		private readonly List<Chat> tabs;
		private int tabPosition = 15;

		public ChatWindow(Widget parent) : base(parent)
		{
			tabs = new List<Chat>();
		}

		public void AddTab(Chat chat)
		{
			AddChild(chat);
			tabs.Add(chat);
			chat.Move(15, tabPosition);
			tabPosition += 15;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetColor(0, 0, 0, 128);
			dc.DrawRectangle(0, 0, Width, Height);
			dc.ResetColor();
		}
	}
}
