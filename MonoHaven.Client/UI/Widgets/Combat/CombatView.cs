using System;
using System.Collections.Generic;
using System.Linq;
using MonoHaven.Input;

namespace MonoHaven.UI.Widgets
{
	public class CombatView : Widget
	{
		private CombatRelation current;
		private readonly LinkedList<CombatRelation> relations;
		private readonly AvatarView avatar;
		private readonly GiveButton btnGive;

		public CombatView(Widget parent) : base(parent)
		{
			relations = new LinkedList<CombatRelation>();

			avatar = new AvatarView(this);
			avatar.Click += OnCurrentAvatarClick;
			avatar.Move(-avatar.Width, 0);

			btnGive = new GiveButton(this);
			btnGive.Move(-avatar.Width - btnGive.Width - 5, 0);
			btnGive.Click += OnCurrentGiveButtonClick;
		}

		public event Action<CombatRelationClickEvent> Give;
		public event Action<CombatRelationClickEvent> Click;

		public CombatRelation Current
		{
			get { return current; }
		}

		public void AddRelation(CombatRelation rel)
		{
			rel.Click += OnRelationClick;
			rel.Give += OnRelationGive;
			relations.AddFirst(rel);
			UpdateLayout();
		}

		public CombatRelation GetRelation(int id)
		{
			return relations.FirstOrDefault(x => x.Id == id);
		}

		public void RemoveRelation(int id)
		{
			var rel = GetRelation(id);
			if (rel == null)
				return;
			
			rel.Click -= OnRelationClick;
			rel.Give -= OnRelationGive;

			relations.Remove(rel);
			rel.Remove();
			rel.Dispose();
			UpdateLayout();
		}

		public void SetCurrentRelation(int id)
		{
			var rel = GetRelation(id);
			if (rel != null)
				SetCurrentRelation(rel);
		}

		protected override void OnDispose()
		{
			if (avatar != null)
			{
				avatar.Remove();
				avatar.Dispose();
			}

			if (btnGive != null)
			{
				btnGive.Remove();
				btnGive.Dispose();
			}
		}

		private void UpdateLayout()
		{
			int y = avatar.Height + 5;
			foreach (var rel in relations)
			{
				if (rel != current)
				{
					rel.Visible = true;
					rel.Move(-rel.Width, y);
					y += rel.Height + 5;
				}
				else
					rel.Visible = false;
			}
		}

		private void SetCurrentRelation(CombatRelation rel)
		{
			if (current != null)
				current.Changed -= OnRelationChanged;

			current = rel;
			UpdateLayout();

			if (current != null)
			{
				current.Changed += OnRelationChanged;
				OnRelationChanged();
			}
		}

		private void OnRelationChanged()
		{
			btnGive.State = current.GiveState;
		}

		private void OnCurrentAvatarClick(AvatarView sender, MouseButtonEvent e)
		{
			Click.Raise(new CombatRelationClickEvent(e.Button, current));
		}

		private void OnCurrentGiveButtonClick(MouseButtonEvent e)
		{
			Give.Raise(new CombatRelationClickEvent(e.Button, current));
		}

		private void OnRelationClick(CombatRelationClickEvent e)
		{
			Click.Raise(e);
		}

		private void OnRelationGive(CombatRelationClickEvent e)
		{
			Give.Raise(e);
		}
	}
}
