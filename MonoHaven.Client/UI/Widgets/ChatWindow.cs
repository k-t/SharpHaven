using System.Collections.Generic;
using System.Linq;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class ChatWindow : Widget
	{
		private readonly Dictionary<Chat, Widget> chats;
		private readonly TabWidget tabWidget;
		private readonly TextBox inputBox;

		public ChatWindow(Widget parent) : base(parent)
		{
			chats = new Dictionary<Chat, Widget>();
			tabWidget = new TabWidget(this);
			inputBox = new TextBox(this);
			inputBox.KeyDown += OnInputBoxKeyDown;
		}

		public void AddChat(Chat chat)
		{
			var tab = tabWidget.AddTab(chat.Title);
			tab.AddChild(chat);
			chat.Resize(tab.Size);
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
			inputBox.Move(0, Height - inputBox.Height);
			inputBox.Resize(Width, inputBox.Height);

			tabWidget.Resize(Width, Height - inputBox.Height - 5);
		}

		private void OnInputBoxKeyDown(KeyEvent e)
		{
			if (e.Key == Key.Enter)
			{
				e.Handled = true;
				if (!string.IsNullOrEmpty(inputBox.Text))
				{
					var chat = GetCurrentChat();
					if (chat != null)
					{
						chat.SendMessage(inputBox.Text);
						inputBox.Text = "";
					}
				}
			}
		}

		private Chat GetCurrentChat()
		{
			if (tabWidget.CurrentTab == null)
				return null;
			return tabWidget.CurrentTab.Children.OfType<Chat>().FirstOrDefault();
		}
	}
}
