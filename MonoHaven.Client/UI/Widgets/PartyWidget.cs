using System.Collections.Generic;
using System.Linq;
using MonoHaven.Game;
using System;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class PartyWidget : Widget
	{
		private readonly Button btnLeave;
		private readonly GobCache gobCache;
		private readonly Dictionary<int, AvatarView> avatars;
		private int playerId;
		private Party party;

		public PartyWidget(Widget parent, GobCache gobCache) : base(parent)
		{
			this.avatars = new Dictionary<int, AvatarView>();
			this.gobCache = gobCache;

			btnLeave = new Button(this, 84);
			btnLeave.Text = "Leave party";
			btnLeave.Clicked += () => Leave.Raise();

			Update();
		}

		public event Action Leave;
		public event Action<PartyMemberClickEventArgs> PartyMemberClicked;

		public int PlayerId
		{
			get { return playerId; }
			set
			{
				playerId = value;
				Update();
			}
		}

		public Party Party
		{
			get { return party; }
			set
			{
				if (party != null)
				{
					party.Changed -= Update;
					party.MemberChanged -= OnPartyMemberChanged;
				}

				party = value;

				if (party != null)
				{
					party.Changed += Update;
					party.MemberChanged += OnPartyMemberChanged;
				}
				Update();
			}
		}

		private void OnPartyMemberChanged(PartyMember member)
		{
			AvatarView avatar;
			if (avatars.TryGetValue(member.Id, out avatar))
				avatar.BorderColor = member.Color;
		}

		private void Update()
		{
			var removed = new List<int>(avatars.Keys);

			if (Party != null)
			{
				foreach (var member in Party.Members.OrderBy(x => x.Id))
				{
					if (member.Id == PlayerId || removed.Remove(member.Id))
						continue;

					var avatar = new AvatarView(this);
					avatar.Resize(37, 37);
					avatar.BorderColor = member.Color;
					avatar.Click += OnAvatarClick;
					//avatar.Avatar = new Avatar(member.Id, gobCache);
					avatars.Add(member.Id, avatar);

					var gob = gobCache.Get(member.Id);
					if (gob != null)
					{
						avatar.Tooltip = (gob.KinInfo != null)
							? new Tooltip(gob.KinInfo.Name)
							: null;
					}
				}
			}

			foreach (var memberId in removed)
			{
				var avatar = avatars[memberId];
				avatar.Click -= OnAvatarClick;
				avatar.Dispose();
				avatar.Remove();
				avatars.Remove(memberId);
			}

			int i = 0;
			foreach (var avatar in avatars.Values)
			{
				avatar.Move((i % 2) * 43, (i / 2) * 43 + 24);
				i++;
			}

			Visible = avatars.Count > 0;
		}

		private void OnAvatarClick(AvatarView avatar, MouseButton button)
		{
			var pair = avatars.FirstOrDefault(x => x.Value == avatar);
			if (pair.Value != null)
			{
				PartyMemberClicked.Raise(new PartyMemberClickEventArgs(pair.Key, button));
			}
		}
	}
}
