using System;
using System.Collections.Generic;
using System.Linq;
using SharpHaven.Game;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
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
			btnLeave.Click += () => Leave.Raise();

			Update();
		}

		public event Action Leave;
		public event Action<PartyMemberClickEvent> PartyMemberClick;

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

		private void OnAvatarClick(AvatarView sender, MouseButtonEvent e)
		{
			var pair = avatars.FirstOrDefault(x => x.Value == sender);
			if (pair.Value != null)
				PartyMemberClick.Raise(new PartyMemberClickEvent(pair.Key, e.Button));
		}
	}
}
