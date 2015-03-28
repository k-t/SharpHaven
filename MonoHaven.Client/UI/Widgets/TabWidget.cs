using System.Collections.Generic;
using System.Linq;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class TabWidget : Widget
	{
		private static readonly Drawable background;
		
		static TabWidget()
		{
			background = App.Resources.Get<Drawable>("custom/ui/wbox2");
		}

		private readonly TabBar bar;
		private readonly List<Widget> tabs;
		private Widget currentTab;

		public TabWidget(Widget parent) : base(parent)
		{
			tabs = new List<Widget>();
			bar = new TabBar(this);
			bar.Click += OnBarClick;
		}

		public Widget CurrentTab
		{
			get { return currentTab; }
			set
			{
				var index = tabs.IndexOf(value);
				if (index != -1)
				{
					currentTab = value;
					bar.SetActiveButton(index);

					foreach (var tab in tabs)
						tab.Visible = (tab == currentTab);
				}
			}
		}

		public Widget AddTab(string text)
		{
			var tab = new Container(this);
			tab.Move(0, bar.Height);
			tab.Resize(Width, Height - bar.Height);
			tab.Visible = false;

			bar.AddButton(text);
			tabs.Add(tab);

			// set current tab
			if (tabs.Count == 1)
				CurrentTab = tabs[0];

			return tab;
		}

		public void RemoveTab(Widget tab)
		{
			int index = tabs.IndexOf(tab);
			if (index != -1)
			{
				if (tab == CurrentTab)
					CurrentTab = tabs.FirstOrDefault();

				tabs.RemoveAt(index);
				bar.RemoveButton(index);

				tab.Remove();
				tab.Dispose();
			}
		}

		private void OnBarClick(int i)
		{
			CurrentTab = tabs[i];
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, bar.Height, Width, Height - bar.Height);
		}

		protected override void OnSizeChanged()
		{
			foreach (var tab in tabs)
				tab.Resize(Width, Height - bar.Height);
		}
	}
}
