using System.Collections.Generic;
using System.Drawing;
using SharpHaven.Game;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerCharWindow : ServerWindow
	{
		private CharWindow widget;

		public ServerCharWindow(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("btime", args => widget.BeliefTimer.Time = (int)args[0]);
			SetHandler("exp", args => widget.SetExp((int)args[0]));
			SetHandler("food", args => widget.FoodMeter.Update(args));
			SetHandler("studynum", args => widget.SetAttention((int)args[0]));
			SetHandler("nsk", SetAvailableSkills);
			SetHandler("psk", SetCurrentSkills);
			SetHandler("numen", SetNumen);
			SetHandler("wish", SetWish);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static new ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerCharWindow(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var studyId = args.Length > 0 ? (int)args[0] : -1;

			widget = new CharWindow(Parent.Widget, Parent.Session.State);
			widget.Move(position);
			widget.AttributesChanged += OnAttributesChanged;
			widget.BeliefChanged += OnBeliefChanged;
			widget.SkillLearned += OnSkillLearned;
			widget.Worship.Forfeit += () => SendMessage("forfeit", 0);
			widget.Closed += () => SendMessage("close");

			// HACK: study widget is not created through a server message
			//       but passed in the arguments to the char window
			if (studyId != -1)
			{
				var study = new ServerContainer((ushort)studyId, this, widget.Study);
				Parent.Session.Widgets.Add(study);
			}
		}

		private void SetAvailableSkills(object[] args)
		{
			widget.AvailableSkills.SetSkills(GetSkills(args, true));
		}

		private void SetCurrentSkills(object[] args)
		{
			widget.CurrentSkills.SetSkills(GetSkills(args, false));
		}

		private void SetNumen(object[] args)
		{
			int ent = (int)args[0];
			int numen = (int)args[1];
			if (ent == 0)
				widget.Worship.SetNumenCount(numen);
		}

		private void SetWish(object[] args)
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
		}

		private void OnAttributesChanged(object[] args)
		{
			SendMessage("sattr", args);
		}

		private void OnBeliefChanged(BeliefChangeEvent args)
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
