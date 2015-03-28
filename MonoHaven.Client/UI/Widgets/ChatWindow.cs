using System.Collections.Generic;

namespace MonoHaven.UI.Widgets
{
	public class ChatWindow : Widget
	{
		private readonly Dictionary<Chat, Widget> chats;
		private readonly TabWidget tabWidget;

		public ChatWindow(Widget parent) : base(parent)
		{
			chats = new Dictionary<Chat, Widget>();
			tabWidget = new TabWidget(this);
		}

		public void AddChat(Chat chat)
		{
			var tab = tabWidget.AddTab(chat.Title);
			tab.AddChild(chat);
			chats[chat] = tab;
		}

		public void RemoveChat(Chat chat)
		{
			Widget tab;
			if (chats.TryGetValue(chat, out tab))
				tabWidget.RemoveTab(tab);
		}

		protected override void OnSizeChanged()
		{
			tabWidget.Resize(Size);
		}
	}
}
