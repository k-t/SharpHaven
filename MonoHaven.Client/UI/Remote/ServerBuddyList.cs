using System.Linq;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerBuddyList : ServerWindow
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new BuddyWindow(parent.Widget);
			return new ServerBuddyList(id, parent, widget);
		}

		private readonly BuddyWindow widget;

		public ServerBuddyList(ushort id, ServerWidget parent, BuddyWindow widget)
			: base(id, parent, widget)
		{
			this.widget = widget;

			this.widget.AddKin += AddKin;
			this.widget.ChangeSecret += ChangeSecret;
			this.widget.BuddyList.SelectedItemChanged += SelectBuddy;

			this.widget.BuddyInfo.RemoveBuddy += () => SendAction("rm");
			this.widget.BuddyInfo.Chat += () => SendAction("chat");
			this.widget.BuddyInfo.Describe += () => SendAction("desc");
			this.widget.BuddyInfo.Exile += () => SendAction("exile");
			this.widget.BuddyInfo.Invite += () => SendAction("inv");
			this.widget.BuddyInfo.ChangeName += ChangeBuddyName;
			this.widget.BuddyInfo.ChangeGroup += ChangeBuddyGroup;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			switch (message)
			{
				case "add":
				{
					var buddy = new Buddy {
						Id = (int)args[0],
						Name = (string)args[1],
						OnlineStatus = (int)args[2],
						Group = (int)args[3]
					};
					widget.BuddyList.Add(buddy);
					break;
				}
				case "rm":
				{
					int buddyId = (int)args[0];
					widget.BuddyList.Remove(buddyId);
					if (widget.BuddyInfo.BuddyId == buddyId)
						widget.BuddyInfo.Clear();
					break;
				}
				case "sel":
				{
					int id = (int)args[0];
					widget.BuddyList.SelectItem(id);
					break;
				}
				case "pwd":
					widget.Secret = (string)args[0];
					break;
				case "chst":
				{
					int id = (int)args[0];
					int status = (int)args[1];
					widget.BuddyList.SetStatus(id, status);
					break;
				}
				case "chnm":
				{
					int id = (int)args[0];
					var name = (string)args[1];
					widget.BuddyList.SetName(id, name);
					break;
				}
				case "chgrp":
				{
					int id = (int)args[0];
					int group = (int)args[1];
					widget.BuddyList.SetGroup(id, group);
					break;
				}
				case "i-set":
				{
					widget.BuddyInfo.BuddyId = (int)args[0];
					widget.BuddyInfo.Name = (string)args[1];
					widget.BuddyInfo.Group = (int)args[2];
					break;
				}
				case "i-act":
					widget.BuddyInfo.SetActions((BuddyActions)args[0]);
					break;
				case "i-atime":
					widget.BuddyInfo.SetLastSeenTime((int)args[0]);
					break;
				case "i-ava":
				{
					var layers = args.Select(x => Session.GetSprite((int)x));
					var avatar = layers.Any() ? new Avatar(layers) : null;
					widget.BuddyInfo.SetAvatar(avatar);
					break;
				}
				case "i-clear":
					widget.BuddyInfo.Clear();
					break;
				default:
					base.ReceiveMessage(message, args);
					break;
			}
		}

		private void AddKin(string secret)
		{
			SendMessage("bypwd", secret);
		}

		private void ChangeSecret()
		{
			SendMessage("pwd", widget.Secret);
		}

		private void ChangeBuddyName()
		{
			var buddyInfo = widget.BuddyInfo;
			if (buddyInfo.BuddyId.HasValue)
				SendMessage("nick", buddyInfo.BuddyId.Value, buddyInfo.Name);
		}

		private void ChangeBuddyGroup(int group)
		{
			var buddyInfo = widget.BuddyInfo;
			if (buddyInfo.BuddyId.HasValue)
				SendMessage("grp", buddyInfo.BuddyId.Value, group);
		}

		private void SelectBuddy()
		{
			var buddy = widget.BuddyList.SelectedItem;
			if (buddy != null)
				SendMessage("ch", buddy.Id);
		}

		private void SendAction(string action)
		{
			if (widget.BuddyInfo.BuddyId.HasValue)
				SendMessage(action, widget.BuddyInfo.BuddyId.Value);
		}
	}
}
