using System.Drawing;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Text;

namespace SharpHaven.UI.Widgets
{
	public class CombatMeter : Widget
	{
		private static readonly Color offColor = Color.Red;
		private static readonly Color defColor = Color.Blue;

		private static readonly Drawable sword;
		private static readonly Drawable[] scales;

		static CombatMeter()
		{
			sword = App.Resources.Get<Drawable>("gfx/hud/combat/com/offdeff");
			scales = new Drawable[11];
			for (int i = 0; i < 11; i++)
			{
				var name = string.Format("gfx/hud/combat/com/{0:D2}", i);
				scales[i] = App.Resources.Get<Drawable>(name);
			}
		}

		private readonly Label lblIntensity;
		private CombatRelation relation;

		public CombatMeter(Widget parent) : base(parent)
		{
			Size = sword.Size;

			lblIntensity = new Label(this, Fonts.Heading);
			lblIntensity.TextAlign = TextAlign.Center;
			lblIntensity.Resize(Width, Fonts.Heading.Height);
			lblIntensity.Move(0, (Height - lblIntensity.Height) / 2);
		}

		public int Offense
		{
			get;
			set;
		}

		public int Defense
		{
			get;
			set;
		}

		public CombatRelation Relation
		{
			get { return relation; }
			set
			{
				if (relation != null)
					relation.Changed -= OnRelationChanged;

				relation = value;
				OnRelationChanged();

				if (relation != null)
					relation.Changed += OnRelationChanged;
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Relation == null)
				return;

			dc.Draw(scales[(-Relation.Balance) + 5], 0, 0);
			dc.Draw(sword, 0, 0);
			//g.aimage(intf.render(String.format("%d", rel.intns)).tex(), intc, 0.5, 0.5);

			if (Offense >= 200)
			{
				dc.SetColor(offColor);
				dc.DrawRectangle(54, 61, -Offense / 200, 5);
			}
			if (Defense >= 200)
			{
				dc.SetColor(defColor);
				dc.DrawRectangle(54, 71, -Defense / 200, 5);
			}
			dc.ResetColor();

			if (Relation.Offense >= 200)
			{
				dc.SetColor(offColor);
				dc.DrawRectangle(80, 61, Relation.Offense / 200, 5);
			}
			if (Relation.Offense >= 200)
			{
				dc.SetColor(defColor);
				dc.DrawRectangle(80, 71, Relation.Defense / 200, 5);
			}
			dc.ResetColor();
		}

		private void OnRelationChanged()
		{
			lblIntensity.Text = (Relation != null)
				? Relation.Intensity.ToString()
				: "";
		}
	}
}
