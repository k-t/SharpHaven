using System.Collections.Generic;
using Haven;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.UI.Layouts;

namespace SharpHaven.UI.Widgets
{
	public class Bufflist : Widget
	{
		private const int Spacing = 2;
		private const int Num = 5;

		private static readonly Point2D imgoff = new Point2D(3, 3);
		private static readonly Point2D ameteroff = new Point2D(3, 36);
		private static readonly Point2D ametersz = new Point2D(30, 2);
		private static readonly Drawable frame;
		private static readonly Drawable cframe;

		private readonly ClientSession session;
		private readonly Dictionary<int, BuffWidget> widgets;

		static Bufflist()
		{
			frame = App.Resources.Get<Drawable>("gfx/hud/buffs/frame");
			cframe = App.Resources.Get<Drawable>("gfx/hud/buffs/cframe");
		}

		public Bufflist(Widget parent, ClientSession session) : base(parent)
		{
			this.session = session;
			this.session.Buffs.Changed += Update;
			this.widgets = new Dictionary<int, BuffWidget>();
			this.Resize(Num * frame.Width + (Num - 1) * Spacing, cframe.Height);
			Update();
		}

		protected override void OnDispose()
		{
			session.Buffs.Changed -= Update;
		}

		private void Update()
		{
			var removed = new List<int>(widgets.Keys);
			var layout = new GridLayout();
			
			int i = 0;
			foreach (var buff in session.Buffs)
			{
				if (!buff.IsMajor) continue;

				BuffWidget widget;
				if (widgets.TryGetValue(buff.Id, out widget))
					removed.Remove(buff.Id);
				else
				{
					widget = new BuffWidget(this);
					widgets[buff.Id] = widget;
				}

				widget.Buff = buff;
				layout.SetColumnWidth(i, cframe.Width);
				layout.AddWidget(widget, 0, i);

				if (++i >= 5)
					break;
			}

			foreach (var id in removed)
			{
				var widget = widgets[id];
				widget.Dispose();
				widget.Remove();
				widgets.Remove(id);
			}

			layout.Spacing = Spacing;
			layout.UpdateGeometry(0, 0, 0, 0);
		}

		private class BuffWidget : Widget
		{
			private Buff buff;

			public BuffWidget(Widget parent) : base(parent)
			{
				this.Resize(cframe.Width, cframe.Height);
			}

			public Buff Buff
			{
				get { return buff; }
				set
				{
					buff = value;
					// HACK:
					base.Tooltip = null;
				}
			}

			public override Tooltip Tooltip
			{
				get
				{
					if (base.Tooltip == null)
						base.Tooltip = GetTooltip();
					return base.Tooltip;
				}
				set { }
			}

			protected override void OnDraw(DrawingContext dc)
			{
				if (Buff == null)
					return;

				if (Buff.Amount >= 0)
				{
					dc.Draw(cframe, 0, 0);
					dc.SetColor(Color.Black);
					dc.DrawRectangle(ameteroff.X, ameteroff.Y, ametersz.X, ametersz.Y);
					dc.SetColor(Color.White);
					dc.DrawRectangle(ameteroff.X, ameteroff.Y, (Buff.Amount * ametersz.X) / 100, ametersz.Y);
					dc.ResetColor();
				}
				else
				{
					dc.Draw(frame, 0, 0);
				}

				if (Buff.Image != null)
					dc.Draw(Buff.Image, imgoff.X, imgoff.Y);
			}

			private Tooltip GetTooltip()
			{
				if (buff == null || string.IsNullOrEmpty(buff.Tooltip))
					return null;
				return new Tooltip(buff.Tooltip);
			}
		}
	}
}
