using System.Drawing;
using System.Linq;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerBuddyList : ServerWindow
	{
		private BuddyWindow widget;

		public ServerBuddyList(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("add", Add);
			SetHandler("rm", Remove);
			SetHandler("sel", Select);
			SetHandler("chst", SetStatus);
			SetHandler("chnm", SetName);
			SetHandler("chgrp", SetGroup);
			SetHandler("i-set", SetInfo);
			SetHandler("i-act", args => widget.BuddyInfo.SetActions((BuddyActions)args[0]));
			SetHandler("i-ava", SetAvatar);
			SetHandler("i-atime", args => widget.BuddyInfo.SetLastSeenTime((int)args[0]));
			SetHandler("i-clear", args => widget.BuddyInfo.Clear());
			SetHandler("pwd", args => widget.Secret = (string)args[0]);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static new ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerBuddyList(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			widget = new BuddyWindow(Parent.Widget);
			widget.Move(position);

			widget.AddKin += OnAddKin;
			widget.ChangeSecret += OnChangeSecret;
			widget.BuddyList.SelectedItemChanged += OnSelectBuddy;

			widget.BuddyInfo.RemoveBuddy += () => SendAction("rm");
			widget.BuddyInfo.Chat += () => SendAction("chat");
			widget.BuddyInfo.Describe += () => SendAction("desc");
			widget.BuddyInfo.Exile += () => SendAction("exile");
			widget.BuddyInfo.Invite += () => SendAction("inv");
			widget.BuddyInfo.ChangeName += OnBuddyNameChange;
			widget.BuddyInfo.ChangeGroup += OnBuddyGroupChange;
			widget.Closed += () => SendMessage("close");
		}

		private void Add(object[] args)
		{
			var buddy = new Buddy
			{
				Id = (int)args[0],
				Name = (string)args[1],
				OnlineStatus = (int)args[2],
				Group = (int)args[3]
			};
			widget.BuddyList.Add(buddy);
		}

		private void Remove(object[] args)
		{
			int buddyId = (int)args[0];
			widget.BuddyList.Remove(buddyId);
			if (widget.BuddyInfo.BuddyId == buddyId)
				widget.BuddyInfo.Clear();
		}

		private void Select(object[] args)
		{
			int id = (int)args[0];
			widget.BuddyList.SelectItem(id);
		}

		private void SetStatus(object[] args)
		{
			int id = (int)args[0];
			int status = (int)args[1];
			widget.BuddyList.SetStatus(id, status);
		}

		private void SetName(object[] args)
		{
			int id = (int)args[0];
			var name = (string)args[1];
			widget.BuddyList.SetName(id, name);
		}

		private void SetGroup(object[] args)
		{
			int id = (int)args[0];
			int group = (int)args[1];
			widget.BuddyList.SetGroup(id, group);
		}

		private void SetInfo(object[] args)
		{
			widget.BuddyInfo.BuddyId = (int)args[0];
			widget.BuddyInfo.Name = (string)args[1];
			widget.BuddyInfo.Group = (int)args[2];
		}

		private void SetAvatar(object[] args)
		{
			var layers = args.Select(x => Session.Resources.GetSprite((int)x));
			var avatar = layers.Any() ? new Avatar(layers) : null;
			widget.BuddyInfo.SetAvatar(avatar);
		}

		private void OnAddKin(string secret)
		{
			SendMessage("bypwd", secret);
		}

		private void OnChangeSecret()
		{
			SendMessage("pwd", widget.Secret);
		}

		private void OnBuddyNameChange()
		{
			var buddyInfo = widget.BuddyInfo;
			if (buddyInfo.BuddyId.HasValue)
				SendMessage("nick", buddyInfo.BuddyId.Value, buddyInfo.Name);
		}

		private void OnBuddyGroupChange(int group)
		{
			var buddyInfo = widget.BuddyInfo;
			if (buddyInfo.BuddyId.HasValue)
				SendMessage("grp", buddyInfo.BuddyId.Value, group);
		}

		private void OnSelectBuddy()
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
