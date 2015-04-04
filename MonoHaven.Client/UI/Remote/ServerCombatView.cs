using System;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;
using MonoHaven.Utils;
using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ServerCombatView : ServerWidget
	{
		private CombatView widget;

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
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerCombatView(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			widget = new CombatView(Parent.Widget);
			widget.Move(position);
			widget.Click += OnClick;
			widget.Give += OnGive;
		}

		private void New(object[] args)
		{
			var rel = new CombatRelation(widget, (int)args[0]);
			rel.Balance = (int)args[1];
			rel.Intensity = (int)args[2];
			rel.GiveState = (int)args[3];
			rel.Initiative = (int)args[4];
			rel.EnemyInitiative = (int)args[5];
			rel.Offense = (int)args[6];
			rel.Defense = (int)args[7];
			widget.AddRelation(rel);
		}

		private void Update(object[] args)
		{
			var rel = widget.GetRelation((int)args[0]);
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
			widget.RemoveRelation((int)args[0]);
		}

		private void UpdateOffenseDefense(object[] args)
		{
			var rel = widget.GetRelation((int)args[0]);
			if (rel != null)
			{
				rel.Offense = (int)args[1];
				rel.Defense = (int)args[2];
			}
		}

		private void SetCurrent(object[] args)
		{
			widget.SetCurrentRelation((int)args[0]);
		}

		private void SetCooldown(object[] args)
		{
			widget.Window.AttackCooldown = DateTime.Now.AddMilliseconds((int)args[0] * 60);
		}

		private void SetManeuver(object[] args)
		{
			widget.Window.Maneuver = GetAction((int)args[0]);
		}

		private void SetAttack(object[] args)
		{
			widget.Window.Move = GetAction((int)args[0]);
			widget.Window.Attack = GetAction((int)args[1]);
		}

		private void SetOffenseDefense(object[] args)
		{
			widget.Offense = (int)args[0];
			widget.Defense = (int)args[1];
		}

		private void OnClick(CombatRelationClickEventArgs e)
		{
			SendMessage("click", e.Relation.Id, ServerInput.ToServerButton(e.Button));
		}

		private void OnGive(CombatRelationClickEventArgs e)
		{
			SendMessage("give", e.Relation.Id, ServerInput.ToServerButton(e.Button));
		}

		private Delayed<GameAction> GetAction(int resId)
		{
			return resId >= 0 ? Session.Get<GameAction>(resId) : null;
		}
	}
}
