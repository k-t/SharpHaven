using System;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Utils;

namespace MonoHaven.UI.Widgets
{
	public class CombatWindow : Window
	{
		private static readonly Drawable iptex;

		static CombatWindow()
		{
			iptex = App.Resources.Get<Drawable>("gfx/hud/combat/ip");
		}

		private Delayed<GameAction> move = new Delayed<GameAction>();
		private Delayed<GameAction> attack = new Delayed<GameAction>();
		private Delayed<GameAction> maneuver = new Delayed<GameAction>();
		private readonly Label lblCurrentAttack;
		private readonly Label lblCurrentManeuver;
		private readonly Label lblInitiative;

		public CombatWindow(Widget parent)
			: base(parent, "Combat")
		{
			var lblAttack = new Label(this, Fonts.Text);
			lblAttack.AutoSize = true;
			lblAttack.Move(10, 5);
			lblAttack.Text = "Attack:";

			lblCurrentAttack = new Label(this, Fonts.Text);
			lblCurrentAttack.Move(50, 35);

			var lblManeuver = new Label(this, Fonts.Text);
			lblAttack.AutoSize = true;
			lblManeuver.Move(10, 55);
			lblManeuver.Text = "Maneuver:";

			lblCurrentManeuver = new Label(this, Fonts.Text);
			lblCurrentManeuver.Move(50, 85);

			lblInitiative = new Label(this, Fonts.Text);
			lblInitiative.Move(205 + iptex.Width, 30);

			Margin = 5;
			Resize(300, 120);
		}

		public Delayed<GameAction> Attack
		{
			get { return attack; }
			set
			{
				attack = value ?? new Delayed<GameAction>();
				UpdateAttackLabel();
			}
		}

		public DateTime AttackCooldown
		{
			get;
			set;
		}

		public Delayed<GameAction> Move
		{
			get { return move; }
			set
			{
				move = value ?? new Delayed<GameAction>();
				UpdateAttackLabel();
			}
		}

		public Delayed<GameAction> Maneuver
		{
			get { return maneuver; }
			set
			{
				maneuver = value ?? new Delayed<GameAction>();
				UpdateManeuverLabel();
			}
		}

		public int Initiative
		{
			get { return int.Parse(lblInitiative.Text); }
			set { lblInitiative.Text = value.ToString(); }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			base.OnDraw(dc);

			dc.PushMatrix();
			dc.Translate(Margin, Margin);

			var hasMove = (Move.Value != null);
			if (hasMove)
				dc.Draw(Move.Value.Image, 15, 20);

			if (Attack.Value != null)
			{
				var p = hasMove ? new Point(18, 23) : new Point(15, 20);
				dc.Draw(Attack.Value.Image, p);
			}
			if (Maneuver.Value != null)
			{
				dc.Draw(Maneuver.Value.Image, 15, 70);
			}

			dc.Draw(iptex, 200, 32);

			var now = DateTime.Now;
			if (now < AttackCooldown)
			{
				dc.SetColor(255, 0, 128, 255);
				dc.DrawRectangle(200, 55, (int)(AttackCooldown - now).TotalMilliseconds / 100, 20);
				dc.ResetColor();
			}

			dc.PopMatrix();
		}

		private void UpdateAttackLabel()
		{
			if (Attack.Value != null)
				lblCurrentAttack.Text = Attack.Value.Name;
			else if (Move.Value != null)
				lblCurrentAttack.Text = Move.Value.Name;
			else
				lblCurrentAttack.Text = "";
		}

		private void UpdateManeuverLabel()
		{
			if (Maneuver.Value != null)
				lblCurrentManeuver.Text = Maneuver.Value.Name;
			else
				lblCurrentManeuver.Text = "";
		}
	}
}
