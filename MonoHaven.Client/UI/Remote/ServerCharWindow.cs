using System.Collections.Generic;
using MonoHaven.Game;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerCharWindow : ServerWindow
	{
		public static new ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var studyId = args.Length > 0 ? (int)args[0] : -1;
			var widget = new CharWindow(parent.Widget, parent.Session.State);
			var serverWidget = new ServerCharWindow(id, parent, widget);

			// HACK: study widget is not created through a server message
			//       but passed in the arguments to the char window
			if (studyId != -1)
			{
				var study = new ServerContainer((ushort)studyId, serverWidget, widget.Study);
				parent.Session.Screen.Bind(study.Id, study);
			}

			return serverWidget;
		}

		private readonly CharWindow widget;

		public ServerCharWindow(ushort id, ServerWidget parent, CharWindow widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.AttributesChanged += OnAttributesChanged;
			this.widget.BeliefChanged += OnBeliefChanged;
			this.widget.SkillLearned += OnSkillLearned;
			this.widget.Worship.Forfeit += () => SendMessage("forfeit", 0);
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			switch (message)
			{
				case "exp":
					widget.SetExp((int)args[0]);
					break;
				case "food":
					widget.FoodMeter.Update(args);
					break;
				case "btime":
					widget.BeliefTimer.Time = (int)args[0];
					break;
				case "studynum":
					widget.SetAttention((int)args[0]);
					break;
				case "nsk":
					widget.AvailableSkills.SetSkills(GetSkills(args, true));
					break;
				case "psk":
					widget.CurrentSkills.SetSkills(GetSkills(args, false));
					break;
				case "numen":
				{
					int ent = (int)args[0];
					int numen = (int)args[1];
					if (ent == 0)
						widget.Worship.SetNumenCount(numen);
					break;
				}
				case "wish":
				{
					int ent = (int)args[0];
					int wish = (int)args[1];
					int resid = (int)args[2];
					int amount = (int)args[3];
					if (ent == 0)
					{
						var item = new Item(Session.Get<ItemMold>(resid));
						item.Amount = amount;
						widget.Worship.SetWish(wish, item);
					}
					break;
				}
				default:
					base.ReceiveMessage(message, args);
					break;
			}
		}

		private void OnAttributesChanged(object[] args)
		{
			SendMessage("sattr", args);
		}

		private void OnBeliefChanged(BeliefChangeEventArgs args)
		{
			SendMessage("believe", args.Name, args.Delta);
		}

		private void OnSkillLearned(Skill skill)
		{
			SendMessage("buy", skill.Id);
		}

		private IEnumerable<Skill> GetSkills(object[] args, bool withCost)
		{
			var skills = new List<Skill>();
			int step = withCost ? 2 : 1;
			for (int i = 0; i < args.Length; i += step)
			{
				var name = (string)args[i];
				var sk = App.Resources.Get<Skill>("gfx/hud/skills/" + name);
				sk.Cost = withCost ? (int?)args[i+1] : null;
				skills.Add(sk);
			}
			return skills;
		}
	}
}
