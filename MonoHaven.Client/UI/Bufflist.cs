using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class Bufflist : Widget
	{
		private const int Margin = 2;
		private const int Num = 5;

		private static readonly Point imgoff = new Point(3, 3);
		private static readonly Point ameteroff = new Point(3, 36);
		private static readonly Point ametersz = new Point(30, 2);
		private static readonly Drawable frame;
		private static readonly Drawable cframe;

		static Bufflist()
		{
			frame = App.Resources.GetImage("gfx/hud/buffs/frame");
			cframe = App.Resources.GetImage("gfx/hud/buffs/cframe");
		}

		private GameState gstate;

		public Bufflist(Widget parent, GameState gstate) : base(parent)
		{
			this.gstate = gstate;
			base.SetSize(Num * frame.Width + (Num - 1) * Margin, cframe.Width);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			int i = 0;
			int w = frame.Width + Margin;
			foreach (var buff in gstate.GetBuffs())
			{
				if (!buff.Major)
					continue;
				
				int x = i * w;
				int y = 0;

				if (buff.AMeter >= 0)
				{
					dc.Draw(cframe, x, y);
					dc.SetColor(Color.Black);
					dc.DrawRectangle(x + ameteroff.X, y + ameteroff.Y, ametersz.X, ametersz.Y);
					dc.SetColor(Color.White);
					dc.DrawRectangle(x + ameteroff.X, y + ameteroff.Y, (buff.AMeter * ametersz.X) / 100, ametersz.Y);
					dc.ResetColor();
				}
				else
				{
					dc.Draw(frame, x, y);
				}

				if (buff.Image.Value != null)
				{
					dc.Draw(buff.Image.Value, x + imgoff.X, y + imgoff.Y);
				}

				if (++i >= 5)
					break;
			}
		}
	}
}
