using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class BuddyInfo : Widget
	{
		private AvatarView av;
		private TextBox tbName;
		private Button btnRemove;
		private Button btnInvite;
		private Button btnChat;
		private Button btnDescribe;
		private Button btnExile;
		private Label lblLastSeen;
		private GroupSelector groupSelector;

		public BuddyInfo(Widget parent) : base(parent)
		{
			av = new AvatarView(this);

			tbName = new TextBox(this);
			tbName.Move(10, 100);
			tbName.Resize(150, 20);
			tbName.KeyDown += OnNameTextBoxKeyDown;

			btnChat = new Button(this, 20);
			btnChat.Move(10, 165);
			btnChat.Text = "Private chat";
			btnChat.Click += () => Chat.Raise();

			btnRemove = new Button(this, 20);
			btnRemove.Move(10, 188);
			btnRemove.Click += () => RemoveBuddy.Raise();

			btnInvite = new Button(this, 20);
			btnInvite.Move(10, 211);
			btnInvite.Text = "Invite to party";
			btnInvite.Click += () => Invite.Raise();

			btnDescribe = new Button(this, 20);
			btnDescribe.Move(10, 234);
			btnDescribe.Text = "Describe to...";
			btnDescribe.Click += () => Describe.Raise();

			btnExile = new Button(this, 20);
			btnExile.Move(10, 257);
			btnExile.Text = "Exile";
			btnExile.Click += () => Exile.Raise();

			lblLastSeen = new Label(this, Fonts.LabelText);
			lblLastSeen.AutoSize = true;
			lblLastSeen.Move(10, 150);

			groupSelector = new GroupSelector(this);
			groupSelector.Move(8, 128);
			groupSelector.Select += group => ChangeGroup.Raise(group);

			Clear();
		}

		public event Action Chat;
		public event Action Describe;
		public event Action RemoveBuddy;
		public event Action Invite;
		public event Action Exile;
		public event Action ChangeName;
		public event Action<int> ChangeGroup;

		public int? BuddyId
		{
			get;
			set;
		}

		public string Name
		{
			get { return tbName.Text; }
			set
			{
				tbName.Text = value;
				tbName.Visible = true;
			}
		}

		public int? Group
		{
			get { return groupSelector.Group; }
			set
			{
				groupSelector.Group = value;
				groupSelector.Visible = value.HasValue;
			}
		}

		public void Clear()
		{
			tbName.Visible = false;
			btnChat.Visible = false;
			btnDescribe.Visible = false;
			btnExile.Visible = false;
			btnInvite.Visible = false;
			btnRemove.Visible = false;
			lblLastSeen.Visible = false;

			av.Visible = false;
			av.Avatar = null;

			groupSelector.Visible = false;
			groupSelector.Group = null;
		}

		public void SetActions(BuddyActions actions)
		{
			btnChat.Visible = actions.HasFlag(BuddyActions.PrivateChat);
			btnDescribe.Visible = actions.HasFlag(BuddyActions.Describe);
			btnExile.Visible = actions.HasFlag(BuddyActions.Exile);
			btnInvite.Visible = actions.HasFlag(BuddyActions.Invite);
			if (actions.HasFlag(BuddyActions.Forget))
			{
				btnRemove.Visible = true;
				btnRemove.Text = "Forget";
			}
			if (actions.HasFlag(BuddyActions.EndKinship))
			{
				btnRemove.Visible = true;
				btnRemove.Text = "End kinship";
			}
		}

		public void SetAvatar(Avatar avatar)
		{
			av.Avatar = avatar;
			av.Visible = (avatar != null);
		}

		public void SetLastSeenTime(int time)
		{
			lblLastSeen.Text = string.Format("Last seen: {0} ago", FormatTime(time));
			lblLastSeen.Visible = true;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetColor(Color.Black);
			dc.DrawRectangle(0, 0, Width, Height);
			dc.ResetColor();
		}

		protected override void OnSizeChanged()
		{
			av.Move(Width / 2 - 40, 10);

			btnRemove.Resize(Width - 20, btnRemove.Height);
			btnChat.Resize(Width - 20, btnChat.Height);
			btnDescribe.Resize(Width - 20, btnDescribe.Height);
			btnExile.Resize(Width - 20, btnExile.Height);
			btnInvite.Resize(Width - 20, btnInvite.Height);
		}

		private void OnNameTextBoxKeyDown(KeyEvent args)
		{
			if (args.Key == Key.Enter)
			{
				ChangeName.Raise();
				args.Handled = true;
			}
		}

		private static string FormatTime(int time)
		{
			int am;
			string unit;
			if (time > (604800 * 2))
			{
				am = time / 604800;
				unit = "week";
			}
			else if (time > 86400)
			{
				am = time / 86400;
				unit = "day";
			}
			else if (time > 3600)
			{
				am = time / 3600;
				unit = "hour";
			}
			else if (time > 60)
			{
				am = time / 60;
				unit = "minute";
			}
			else
			{
				am = time;
				unit = "second";
			}
			return string.Format("{0} {1}{2}", am, unit, am > 1 ? "s" : "");
		}
	}
}
