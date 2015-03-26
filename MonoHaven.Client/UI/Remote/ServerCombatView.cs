using System;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;
using MonoHaven.Utils;

namespace MonoHaven.UI.Remote
{
	public class ServerCombatView : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new CombatView(parent.Widget);
			return new ServerCombatView(id, parent, widget);
		}

		private readonly CombatView widget;

		public ServerCombatView(ushort id, ServerWidget parent, CombatView widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Click += OnClick;
			this.widget.Give += OnGive;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			switch (message)
			{
				case "new":
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
					break;
				}
				case "upd":
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
					break;
				}
				case "del":
					widget.RemoveRelation((int)args[0]);
					break;
				case "updod":
				{
					var rel = widget.GetRelation((int)args[0]);
					if (rel != null)
					{
						rel.Offense = (int)args[1];
						rel.Defense = (int)args[2];
					}
					break;
				}
				case "cur":
					widget.SetCurrentRelation((int)args[0]);
					break;
				case "atkc":
					widget.Window.AttackCooldown = DateTime.Now.AddMilliseconds((int)args[0] * 60);
					break;
				case "blk":
					widget.Window.Maneuver = GetAction((int)args[0]);
					break;
				case "atk":
					widget.Window.Move = GetAction((int)args[0]);
					widget.Window.Attack = GetAction((int)args[1]);
					break;
				case "offdef":
					widget.Offense = (int)args[0];
					widget.Defense = (int)args[1];
					break;
				default:
					base.ReceiveMessage(message, args);
					break;
			}
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
