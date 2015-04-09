using System;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;
using MonoHaven.Utils;

namespace MonoHaven.UI.Remote
{
	public class ServerCombatView : ServerWidget
	{
		private CombatView view;
		private CombatMeter meter;
		private CombatWindow window;

		public ServerCombatView(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("new", New);
			SetHandler("upd", Update);
			SetHandler("del", Delete);
			SetHandler("updod", UpdateOffenseDefense);
			SetHandler("cur", SetCurrent);
			SetHandler("atkc", SetCooldown);
			SetHandler("blk", SetManeuver);
			SetHandler("atk", SetAttack);
			SetHandler("offdef", SetOffenseDefense);
		}

		public override Widget Widget
		{
			get { return null; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerCombatView(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			view = Session.State.Screen.CombatView;
			view.Visible = true;
			view.Click += OnClick;
			view.Give += OnGive;

			meter = Session.State.Screen.CombatMeter;
			meter.Visible = true;

			window = Session.State.Screen.CombatWindow;
			window.Visible = true;
		}

		protected override void OnDestroy()
		{
			view.Click -= OnClick;
			view.Give -= OnGive;
			view.Visible = false;
			meter.Visible = false;
			window.Visible = false;
		}

		private void New(object[] args)
		{
			var rel = new CombatRelation(view, (int)args[0]);
			rel.Balance = (int)args[1];
			rel.Intensity = (int)args[2];
			rel.GiveState = (int)args[3];
			rel.Initiative = (int)args[4];
			rel.EnemyInitiative = (int)args[5];
			rel.Offense = (int)args[6];
			rel.Defense = (int)args[7];

			view.AddRelation(rel);
		}

		private void Update(object[] args)
		{
			var rel = view.GetRelation((int)args[0]);
			if (rel != null)
			{
				rel.Balance = (int)args[1];
				rel.Intensity = (int)args[2];
				rel.GiveState = (int)args[3];
				rel.Initiative = (int)args[4];
				rel.EnemyInitiative = (int)args[5];
			}
		}

		private void Delete(object[] args)
		{
			int id = (int)args[0];

			view.RemoveRelation(id);

			if (meter.Relation != null && meter.Relation.Id == id)
				meter.Relation = null;
		}

		private void UpdateOffenseDefense(object[] args)
		{
			var rel = view.GetRelation((int)args[0]);
			if (rel != null)
			{
				rel.Offense = (int)args[1];
				rel.Defense = (int)args[2];
			}
		}

		private void SetCurrent(object[] args)
		{
			view.SetCurrentRelation((int)args[0]);
			meter.Relation = view.Current;
			window.Relation = view.Current;
		}

		private void SetCooldown(object[] args)
		{
			window.AttackCooldown = DateTime.Now.AddMilliseconds((int)args[0] * 60);
		}

		private void SetManeuver(object[] args)
		{
			window.Maneuver = GetAction((int)args[0]);
		}

		private void SetAttack(object[] args)
		{
			window.AttackMove = GetAction((int)args[0]);
			window.Attack = GetAction((int)args[1]);
		}

		private void SetOffenseDefense(object[] args)
		{
			meter.Offense = (int)args[0];
			meter.Defense = (int)args[1];
		}

		private void OnClick(CombatRelationClickEvent e)
		{
			SendMessage("click", e.Relation.Id, ServerInput.ToServerButton(e.Button));
		}

		private void OnGive(CombatRelationClickEvent e)
		{
			SendMessage("give", e.Relation.Id, ServerInput.ToServerButton(e.Button));
		}

		private Delayed<GameAction> GetAction(int resId)
		{
			return resId >= 0 ? Session.Get<GameAction>(resId) : null;
		}
	}
}
