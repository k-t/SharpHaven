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
		private readonly CombatMeter meter;
		private readonly CombatWindow window;

		public CombatView(Widget parent) : base(parent)
		{
			relations = new LinkedList<CombatRelation>();

			meter = new CombatMeter(Parent);
			meter.Move(333, 10);

			window = new CombatWindow(Parent);
			window.Move(100, 100);

			avatar = new AvatarView(Parent);
			avatar.Move(700, 10);
			avatar.Click += OnCurrentAvatarClick;

			btnGive = new GiveButton(Parent);
			btnGive.Move(665, 10);
			btnGive.Click += OnCurrentGiveButtonClick;
		}

		public event Action<CombatRelationClickEventArgs> Give;
		public event Action<CombatRelationClickEventArgs> Click;

		public CombatMeter Meter
		{
			get { return meter; }
		}

		public CombatWindow Window
		{
			get { return window; }
		}

		public int Offense
		{
			get { return meter.Offense; }
			set { meter.Offense = value; }
		}

		public int Defense
		{
			get { return meter.Defense; }
			set { meter.Defense = value; }
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

			if (meter.Relation == rel)
				meter.Relation = null;
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

			if (meter != null)
			{
				meter.Remove();
				meter.Dispose();
			}

			if (window != null)
			{
				window.Remove();
				window.Dispose();
			}
		}

		private void UpdateLayout()
		{
			int y = 0;
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
			meter.Relation = rel;
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
			meter.Intensity = current.Intensity;
			window.Initiative = current.Initiative;
		}

		private void OnCurrentAvatarClick(AvatarView sender, MouseButtonEvent e)
		{
			Click.Raise(new CombatRelationClickEventArgs(e.Button, current));
		}

		private void OnCurrentGiveButtonClick(MouseButtonEvent e)
		{
			Give.Raise(new CombatRelationClickEventArgs(e.Button, current));
		}

		private void OnRelationClick(CombatRelationClickEventArgs e)
		{
			Click.Raise(e);
		}

		private void OnRelationGive(CombatRelationClickEventArgs e)
		{
			Give.Raise(e);
		}
	}
}
