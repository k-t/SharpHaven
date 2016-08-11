using System;
using System.Collections.Generic;
using Haven;

namespace SharpHaven.Client
{
	public class Party
	{
		private readonly Dictionary<int, PartyMember> members;
		private int leaderId;

		public Party()
		{
			members = new Dictionary<int, PartyMember>();
		}

		public event Action LeaderChanged;
		public event Action<PartyMember> MemberChanged;
		public event Action Changed;

		public int MemberCount
		{
			get { return members.Count; }
		}

		public IEnumerable<PartyMember> Members
		{
			get { return members.Values; }
		}

		public PartyMember Leader
		{
			get { return GetMember(LeaderId); }
		}

		public int LeaderId
		{
			get { return leaderId; }
			set
			{
				leaderId = value;
				LeaderChanged.Raise();
			}
		}

		public void Update(IEnumerable<int> membersIds)
		{
			var removed = new List<PartyMember>(members.Values);
			
			foreach (var id in membersIds)
			{
				PartyMember member;
				if (members.TryGetValue(id, out member))
					removed.Remove(member);
				else
					AddMember(new PartyMember(id));
			}

			foreach (var id in removed)
				RemoveMember(id);

			Changed.Raise();
		}

		public PartyMember GetMember(int id)
		{
			PartyMember member;
			return members.TryGetValue(id, out member) ? member : null;
		}

		private void AddMember(PartyMember member)
		{
			members.Add(member.Id, member);
			member.Changed += OnMemberChanged;
		}

		private void RemoveMember(PartyMember member)
		{
			if (member.Id == LeaderId)
				LeaderId = -1;

			members.Remove(member.Id);
			member.Changed -= OnMemberChanged;
		}

		private void OnMemberChanged(PartyMember member)
		{
			MemberChanged.Raise(member);
		}
	}
}
